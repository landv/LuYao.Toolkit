using NewLife.Threading;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace LuYao.Toolkit.ViewStates;

public class ViewStateBag : SortedDictionary<string, ViewStateItem>
{
    private static readonly ConcurrentQueue<ViewStateBag> _bags = new ConcurrentQueue<ViewStateBag>();
    private static readonly TimerX _timer;
    static ViewStateBag()
    {
        _timer = new TimerX(Flush, _bags, int.MaxValue, int.MaxValue);
    }
    public static void Flush() => Flush(_bags);
    private static void Flush(object locker)
    {
        lock (locker)
        {
            var set = new HashSet<ViewStateBag>();
            while (_bags.TryDequeue(out var item)) set.Add(item);
            foreach (var item in set)
            {
                var entity = item.FindEntity() ?? new Entities.ViewModelState { CreatedAt = DateTime.Now, Type = item.HostType.FullName };
                entity.UpdatedAt = DateTime.Now;
                entity.Value = item.ToJson();
                entity.Save();
            }
        }
    }
    public ViewStateBag(IViewStateHost host)
    {
        this._host = host;
        this.HostType = host.GetType();
        var entity = FindEntity();
        if (entity != null) JsonConvert.PopulateObject(entity.Value, this);
        var fields = this.HostType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (var field in fields)
        {
            var attributes = field.GetCustomAttributes<WatchViewStateAttribute>();
            foreach (var item in attributes) maps[item.Property] = field;
        }
        if (maps.Count > 0 && this.Count > 0)
        {
            foreach (var item in maps)
            {
                if (this.TryGetValue(item.Key, out var value))
                {
                    var fld = item.Value;
                    var type = fld.FieldType;
                    if (type.FullName == value.Type) item.Value.SetValue(host, value.Read(type));
                }
            }
        }
        if (this._host.InstanceId == 1)
        {
            this._host.PropertyChanged += this.PropertyChanged;
        }
    }
    private SortedDictionary<string, FieldInfo> maps = new SortedDictionary<string, FieldInfo>();
    public Type HostType { get; }
    private Entities.ViewModelState FindEntity() => Entities.ViewModelState.FindByType(this.HostType.FullName);
    private void PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (!this.maps.TryGetValue(e.PropertyName, out var field)) return;
        var value = field.GetValue(this._host);
        if (!this.TryGetValue(e.PropertyName, out var item)) item = this[e.PropertyName] = new ViewStateItem();
        item.Write(value);
        _bags.Enqueue(this);
        _timer.SetNext(1000);
    }

    private readonly IViewStateHost _host;

    public string ToJson() => JsonConvert.SerializeObject(this);
}
