/*[GUID("561A6F5BC8E745439266885631CD20BE")]*/
String.prototype.format = function () {
    var args = arguments;
    return this.replace(/\{(\d+)\}/g,
        function (m, i) {
            return args[i];
        });
}

String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

var keywords = ["abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "in (generic", "modifier)", "int", "interface", "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "", "override", "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void", "volatile", "while", "add", "alias", "ascending", "descending", "dynamic", "from", "get", "global", "group", "into", "join", "let", "orderby", "partial", "remove", "select", "set"];

function isKeyWords(keyw) {
    keyw = keyw.toLowerCase();
    for (var i = 0; i < keywords.length; i++) {
        if (keyw == keywords[i]) {
            return true;
        }
    }
    return false;
}

JSON2CSharp = {
    _allClass: [],
    _genClassCode: function (obj, name) {
        var clas = "public class {0}\r\n{\r\n".format(name || "Root");
        for (var n in obj) {
            var v = obj[n];
            n = n.trim();
            if (isKeyWords(n)) {
                n = "@" + n;
            }
            clas += "    {0}    public {1} {2} { get; set; }<br/>".format(this._genComment(v), this._genTypeByProp(n, v), n);
        }
        clas += "}<br/>";
        this._allClass.push(clas);
        return this._allClass.join("<br/>");
    },
    _genTypeByProp: function (name, val) {
        switch (Object.prototype.toString.apply(val)) {
            case "[object Number]":
                {
                    return val.toString().indexOf(".") > -1 ? "double" : "int";
                }
            case "[object Date]":
                {
                    return "DateTime";
                }
            case "[object Object]":
                {
                    name = name.substring(0, 1).toUpperCase() + name.substring(1);
                    this._genClassCode(val, name);
                    return name;
                }
            case "[object Array]":
                {
                    return "List&#60;{0}&#62;".format(this._genTypeByProp(name + "Item", val[0]));
                }
            case "[object Boolean]":
                {
                    return "bool";
                }
            default:
                {
                    var d = Date.parse(val);
                    //如果是日期格式
                    if (isNaN(d) == false) return 'DateTime';
                    return "string";
                }
        }
    },
    _genComment: function (val) {
        //return "";
        var commm = typeof (val) == "string" && /.*[\u4e00-\u9fa5]+.*$/.test(val) ? val : "";
        if (commm.length > 0) return "/// &#60;summary&#62;\r\n    /// " + commm + "\r\n    /// &#60;/summary&#62;\r\n";
        return "";
    },
    convert: function (jsonObj) {
        this._allClass = [];
        return this._genClassCode(jsonObj);
    }
}
