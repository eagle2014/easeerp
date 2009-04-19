/**
 * TS通用工具方法
 * Tony 2007-07-10
 * singleton 
 */
    
var _ua_ = navigator.userAgent.toLowerCase();
var _rootPath_ = (typeof contextPath == "string" ? contextPath : "..") + "/";
var _browser_ = (function(){  
	var isSafari = (/webkit|khtml/).test(_ua_);
	var isIE = _ua_.indexOf("msie") > -1;
	var isStrict = document.compatMode == "CSS1Compat";
	var isBorderBox = isIE && !isStrict;
	return {  
	    isStrict : isStrict,
	    isSecure : window.location.href.toLowerCase().indexOf("https") === 0,
	    isOpera : _ua_.indexOf("opera") > -1,
	    isSafari : isSafari,
	    isIE : isIE,
	    isIE7 : _ua_.indexOf("msie 7") > -1,
	    isGecko : !isSafari && _ua_.indexOf("gecko") > -1,
	    isBorderBox : isBorderBox,
	    isWindows : (_ua_.indexOf("windows") != -1 || _ua_.indexOf("win32") != -1),
	    isLinux : (_ua_.indexOf("linux") != -1),
	    isMac : (_ua_.indexOf("macintosh") != -1 || _ua_.indexOf("mac os x") != -1)
	};
})();

var TS = {
	_id: 0,
	id: function(){
		return TS._id++;
	},
	
	/* 系统是否处于调试状态 */
	debug: false,
	
	/* 系统根路径 */
	rootPath: _rootPath_,
	
	BLANK_IMAGE_URL: _rootPath_ + "TS_platform/images/s.gif",
	
	/* 系统action路径 */
	actionBase: _rootPath_ + "",
	
	/*
	 * 复制配置对象config中的所有属性到对象obj.
	 * @param {Object} obj 配置属性的接收对象
	 * @param {Object} config 指定的配置
	 * @param {Object} defaults 默认配置
	 * @return {Object} returns obj
	 * @member TS apply
	 */
	apply: function(obj, config, defaults){
	    if(defaults){
	        TS.apply(obj, defaults);
	    }
	    if(obj && config && typeof config == 'object'){
	        for(var p in config){
	            obj[p] = config[p];
	        }
	    }
	    return obj;
	},
	
    /*
	 * 复制配置对象config中的所有属性到对象obj(如果属性在obj中还没有定义).
	 * @param {Object} obj 配置属性的接收对象
	 * @param {Object} config 指定的配置
	 * @return {Object} returns obj
	 * @member TS applyIf
     */
	applyIf: function(obj, config){
        if(obj && config){
            for(var p in config){
                if(typeof obj[p] == "undefined"){ obj[p] = config[p]; }
            }
        }
        return obj;
	},
	
    /**
     * 创建指定名称的命名空间,多个命名空间间以参数的形式用逗号隔开
     * @param {String} namespace1
     * @param {String} namespace2
     * @param {String} etc
     * @method namespace
     */
    namespace : function(){
        var a=arguments, o=null, i, j, d, rt;
        for (i=0; i<a.length; ++i) {
            d=a[i].split(".");
            rt = d[0];
            eval('if (typeof ' + rt + ' == "undefined"){' + rt + ' = {};} o = ' + rt + ';');
            for (j=1; j<d.length; ++j) {
                o[d[j]]=o[d[j]] || {};
                o=o[d[j]];
            }
        }
    },

    /**
     * Extends one class with another class and optionally overrides members with the passed literal. This class
     * also adds the function "override()" to the class that can be used to override
     * members on an instance.
     * @param {Object} subclass The class inheriting the functionality
     * @param {Object} superclass The class being extended
     * @param {Object} overrides (optional) A literal with members
     * @method extend
     */
    extend : function(){
        // inline overrides
        var io = function(o){
            for(var m in o){
                this[m] = o[m];
            }
        };
        return function(sb, sp, overrides){
            if(typeof sp == 'object'){
                overrides = sp;
                sp = sb;
                sb = function(){sp.apply(this, arguments);};
            }
            var F = function(){}, sbp, spp = sp.prototype;
            F.prototype = spp;
            sbp = sb.prototype = new F();
            sbp.constructor=sb;
            sb.superclass=spp;
            if(spp.constructor == Object.prototype.constructor){
                spp.constructor=sp;
            }
            sb.override = function(o){
                TS.override(sb, o);
            };
            sbp.override = io;
            TS.override(sb, overrides);
            return sb;
        };
    }(),

    /**
     * Adds a list of functions to the prototype of an existing class, overwriting any existing methods with the same name.
     * Usage:<pre><code>
					TS.override(MyClass, {
					newMethod1: function(){
					    // etc.
					},
					newMethod2: function(foo){
					    // etc.
					}
					});
					</code></pre>
     * @param {Object} origclass The class to override
     * @param {Object} overrides The list of functions to add to origClass.  This should be specified as an object literal
     * containing one or more methods.
     * @method override
     */
    override : function(origclass, overrides){
        if(overrides){
            var p = origclass.prototype;
            for(var method in overrides){
                p[method] = overrides[method];
            }
        }
    },
    
	isStrict : _browser_.isStrict,
    isSecure : _browser_.isSecure,
    isOpera : _browser_.isOpera,
    isSafari : _browser_.isSafari,
    isIE :_browser_.isIE,
    isIE7 : _browser_.isIE7,
    isGecko : _browser_.isGecko,
    isBorderBox : _browser_.isBorderBox,
    isWindows : _browser_.isWindows,
    isLinux : _browser_.isLinux,
    isMac : _browser_.isMac
};

/*
 * 返回指定的url添加一个时间挫参数后的url
 * @param strUrl 要处理的url
 */
TS.addTimeStamp = function(strUrl){
	if (strUrl.indexOf("timeStamp") == -1){
		if(strUrl.indexOf("?") != -1)
			return strUrl + "&timeStamp=" + new Date().getTime();
		else
			return strUrl + "?timeStamp=" + new Date().getTime();
	}else{
		return strUrl;
	}
};

/*
 * 获取错误的调试信息
 * @param response ajax回调的json响应对象
 */
TS.getDebugInfo = function(response){
	if (TS.debug != true || response == null || (typeof response == "undefined")) return "";
	if (response.msg) return response.msg;
	if (response.responseText) return response.responseText;
	return response;
};

TS.getPros = function(obj){
	var s = "";
	for(var p in obj){
		s += p + ";";
	}
	return s;
};

(function(){
    // 移除低于IE7的IE版本的css图像闪烁
	if(TS.isIE && !TS.isIE7){
        try{
            document.execCommand("BackgroundImageCache", false, true);
        }catch(e){}
    }
})();

TS.namespace("TS.util.JSON");
/**
 * @class TS.util.JSON
 * Modified version of Douglas Crockford"s json.js that doesn"t
 * mess with the Object prototype 
 * http://www.json.org/js.html
 * @singleton
 */
TS.util.JSON = new (function(){
    var useHasOwn = {}.hasOwnProperty ? true : false;
    
    // crashes Safari in some instances
    //var validRE = /^("(\\.|[^"\\\n\r])*?"|[,:{}\[\]0-9.\-+Eaeflnr-u \n\r\t])+?$/;
    
    var pad = function(n) {
        return n < 10 ? "0" + n : n;
    };
    
    var m = {
        "\b": '\\b',
        "\t": '\\t',
        "\n": '\\n',
        "\f": '\\f',
        "\r": '\\r',
        '"' : '\\"',
        "\\": '\\\\'
    };

    var encodeString = function(s){
        if (/["\\\x00-\x1f]/.test(s)) {
            return '"' + s.replace(/([\x00-\x1f\\"])/g, function(a, b) {
                var c = m[b];
                if(c){
                    return c;
                }
                c = b.charCodeAt();
                return "\\u00" +
                    Math.floor(c / 16).toString(16) +
                    (c % 16).toString(16);
            }) + '"';
        }
        return '"' + s + '"';
    };
    
    var encodeArray = function(o){
        var a = ["["], b, i, l = o.length, v;
            for (i = 0; i < l; i += 1) {
                v = o[i];
                switch (typeof v) {
                    case "undefined":
                    case "function":
                    case "unknown":
                        break;
                    default:
                        if (b) {
                            a.push(',');
                        }
                        a.push(v === null ? "null" : TS.util.JSON.encode(v));
                        b = true;
                }
            }
            a.push("]");
            return a.join("");
    };
    
    var encodeDate = function(o){
        return '"' + o.getFullYear() + "-" +
                pad(o.getMonth() + 1) + "-" +
                pad(o.getDate()) + "T" +
                pad(o.getHours()) + ":" +
                pad(o.getMinutes()) + ":" +
                pad(o.getSeconds()) + '"';
    };
    
    /**
     * Encodes an Object, Array or other value
     * @param {Mixed} o The variable to encode
     * @return {String} The JSON string
     */
    this.encode = function(o){
        if(typeof o == "undefined" || o === null){
            return "null";
        }else if(o instanceof Array){
            return encodeArray(o);
        }else if(o instanceof Date){
            return encodeDate(o);
        }else if(typeof o == "string"){
            return encodeString(o);
        }else if(typeof o == "number"){
            return isFinite(o) ? String(o) : "null";
        }else if(typeof o == "boolean"){
            return String(o);
        }else {
            var a = ["{"], b, i, v;
            for (i in o) {
                if(!useHasOwn || o.hasOwnProperty(i)) {
                    v = o[i];
                    switch (typeof v) {
                    case "undefined":
                    case "function":
                    case "unknown":
                        break;
                    default:
                        if(b){
                            a.push(',');
                        }
                        a.push(this.encode(i), ":",
                                v === null ? "null" : this.encode(v));
                        b = true;
                    }
                }
            }
            a.push("}");
            return a.join("");
        }
    };
    
    /**
     * Decodes (parses) a JSON string to an object. If the JSON is invalid, this function throws a SyntaxError.
     * @param {String} json The JSON string
     * @return {Object} The resulting object
     */
    this.decode = function(json){
        return eval("(" + json + ')');
    };
})();

/** 
 * Shorthand for {@link TS.util.JSON#encode}
 * @member TS encode 
 * @method */
TS.encode = TS.util.JSON.encode;
/** 
 * Shorthand for {@link TS.util.JSON#decode}
 * @member TS decode 
 * @method */
TS.decode = TS.util.JSON.decode;

TS.getLeft = function(element){
	return TS.getXY(element)[0];
};
TS.getTop=function(element) {
	return TS.getXY(element)[1];
}
TS.getXY=function(element) {
	var left = element.offsetLeft;
	var top = element.offsetTop;
	var parentE = element.offsetParent;
	while(parentE){
		left += parentE.offsetLeft;// - parentE.scrollLeft;
		top += parentE.offsetTop;// - parentE.scrollTop;
		parentE = parentE.offsetElement
	}
	//alert(element.id + ";" + left + ";" + top);
	return [left,top];
}


Object.extend(String, {
    escape : function(string) {
        return string.replace(/('|\\)/g, "\\$1");
    },


    leftPad : function (val, size, ch) {
        var result = new String(val);
        if(ch === null || ch === undefined || ch === '') {
            ch = " ";
        }
        while (result.length < size) {
            result = ch + result;
        }
        return result;
    },

    format : function(format){
        var args = Array.prototype.slice.call(arguments, 1);
        return format.replace(/\{(\d+)\}/g, function(m, i){
            return args[i];
        });
    }
});


String.prototype.toggle = function(value, other){
    return this == value ? other : value;
};

Object.extend(Number.prototype, {
    constrain : function(min, max){
        return Math.min(Math.max(this, min), max);
    }
});

Object.extend(Array.prototype, {
    indexOf : function(o){
       for (var i = 0, len = this.length; i < len; i++){
 	      if(this[i] == o) return i;
       }
 	   return -1;
    },
    remove : function(o){
       var index = this.indexOf(o);
       if(index != -1){
           this.splice(index, 1);
       }
    }
});

/**
 * Prototype中函数Form.serialize的改写,使序列化Form时包含已disabled的元素
 */
TS.Form = {
    serializeElements: function(elements, getHash) {
				var data = elements.inject({}, function(result, element) {
				  if (element.name) {
				    var key = element.name, value = $(element).getValue();
				    if (value != undefined) {
				      if (result[key]) {
				        if (result[key].constructor != Array) result[key] = [result[key]];
				        result[key].push(value);
				      }
				      else result[key] = value;
				    }
				  }
				  return result;
				});
				
				return getHash ? data : Hash.toQueryString(data);
		},
		
		serialize: function(form, getHash) {
    		return TS.Form.serializeElements(Form.getElements(form), getHash);
  	}
};

// 得到字符串的真实长度（双字节换算为两个单字节）  
TS.getStringActualLen = function(sChars)  
{  
    return sChars.replace(/[^\x00-\xff]/g,"xx").length;  
};

/* 
 * 截取固定长度子字符串
 *
 */
TS.getShortString = function(sourceString, maxLen, useEllipsis)  
{  
    if(sourceString.replace(/[^\x00-\xff]/g,"xx").length <= maxLen)  
    {  
        return sourceString;  
    }  

    var str = "";  
    var l = 0;  
    var schar; 
    if(useEllipsis != false) maxLen = maxLen - 3;
    for(var i=0; schar=sourceString.charAt(i); i++)  
    {  
        str += schar;  
        l += (schar.match(/[^\x00-\xff]/) != null ? 2 : 1);  
        if(l >= maxLen)  
        {  
            break;  
        }  
    }  
    return (useEllipsis != false ? str + "..." : str);  
};

/* 
 * 获取指定url加上绝对路径后的url
 *
 */
TS.getAbsoluteUrl = function(sourceUrl)  
{  
    var start = sourceUrl.substr(0,1);
    if(start == '/' || start == '../')  
    {  
        return sourceUrl;
    }else{
        return TS.rootPath + sourceUrl;
    }
};

String.prototype.trim= function(){  
    // 用正则表达式将前后空格  
    // 用空字符串替代。  
    return this.replace(/(^\s*)|(\s*$)/g, "");  
}

TS.getRadioValue = function(id){
	var obj = document.forms[0].elements[id];
	for(var i = 0; i < obj.length; i++){
		if(obj[i].checked){
			return obj[i].value;
		}
	}
	return null;
}

/* 
 * 自动调整IFrame的高度
 * @param iframeObj 所要调整高度的IFrame
 */
TS.adjustIframeHeight = function(iframeObj){
	if(null != iframeObj){
		try{
			if(iframeObj.contentDocument && iframeObj.contentDocument.body.scrollHeight){
				//如果用户的浏览器是NetScape
				iframeObj.height = iframeObj.contentDocument.body.scrollHeight;
				//alert("NetScape=" + iframeObj.height);
			}else if (iframeObj.Document && iframeObj.Document.body.scrollHeight){
				//如果用户的浏览器是IE
				iframeObj.height = iframeObj.Document.body.scrollHeight;
				//alert("IE=" + iframeObj.height);
			}
		}catch(err){
			iframeObj.scrolling = "auto";
		}
	}	
}

// 获取json对象的属性列表字符串
TS.getJSONFields = function (json){
    var names = [];
    for(var name in json){
        names.push(name);
    }
    return names.join('_');
}

// 获取json对象的格式化字符串
TS.formatJSON = function (json,prefix){
    prefix = prefix || "";
    var len = 0;
    var names = [];
    for(var name in json){
        len = Math.max(len, name.length);
        names.push(name);
    }
    names.sort();
    var info = "";
    for(var i=0;i < names.length;i++){
        if(typeof json[names[i]] == "object")
            info += TS.formatJSON(json[names[i]], prefix + names[i] + ".");
        else
            info += String.leftPad(prefix + names[i], len, '-') + " = " + json[names[i]] + "\n";
    }
    return info;
}

TS.Ajax = {
    /* 
     * 封装Ajax的请求处理
	 * @param {Object} config 配置
	 * @config {String} url 请求的url
	 * @config {String} addTimeStamp 可选：是否在请求的url后附加时间戳，默认为true
     * @config {Function} callback 可选：Ajax请求处理成功后的回调函数，第一个参数为请求的相应对象transport
	 * @config {String} method 可选：请求类型，post或get，默认为post
	 * @config {Object} parameters 可选：Ajax请求附加的参数
	 * @config {String|Object} serialize 可选：Ajax请求窗口序列化配置，
	 *      如果serialize为String类型，则为所要序列化的控件id，Ajax请求时会将序列化结果合并到parameters中
	 *      如果serialize为Object类型，则按{elID: yourElementID,getHash: true(默认值)|false}进行配置
	 * @config {Boolean} showSuccessMsg 可选：是否显示处理成功的信息，默认为true
	 * @config {Boolean} showFailureMsg 可选：是否显示处理失败的信息，默认为true
	 * @config {String} closeMe 可选：处理成功后是否关闭当前窗口，默认为false
	 * @config {String} refreshMe 可选：处理成功后是否执行当前窗口的refresh函数，默认为false
	 * @config {String} title 可选：信息提示窗口的标题,默认为“系统提示”
	 * @config {String} failureMsg 可选：未知异常的提示信息,默认为“页面请求处理失败，请尝试重新登录或联系管理员！”
	 * @config {Boolean} debug 可选：用于调试，请求成功后是否用alert显示transport.responseText，默认为false
     */
    request: function(config){
        // 附加默认配置
        TS.applyIf(config,{
            method: 'post', 
	        
	        showSuccessMsg: true,
	        showFailureMsg: true,
	        closeMe: false,
            title: '系统提示',
	        addTimeStamp: true,
	        wait: {
	            msg: '正在处理，请稍候...',
	            iconCls: TS.Msg.IconCls.INFO
	        },
	        debug: false
        });

        if(!config.url){
            alert('必须配置请求的url参数！');
        }else{
            if(config.addTimeStamp == true)
                config.url = TS.addTimeStamp(config.url);  // 添加时间戳
            config.url = TS.getAbsoluteUrl(config.url);    // 将相对路径转换为绝对路径
        }
        
        if(!config.failureMsg){
            config.failureMsg = '<b>页面请求处理失败，请尝试重新登录或联系管理员！</b><br>请求的url为：<br>' + config.url ;
            config.failureMsg += '<br>*收到这个信息是因为执行Ajax处理过程出现了未处理的异常!';
        }
        
        // 显示处理过程的等待信息
        if(config.wait) TS.Msg.wait(config.wait);
        
        // 显示调试信息
        if(config.debug == true) alert("url=" + config.url);
        
        var onSuccess;
        if (typeof(config.onSuccess) == 'function'){
            onSuccess = config.onSuccess;
        }else{
            onSuccess = function(transport){
	            // 显示调试信息
		        if(config.debug == true) alert(transport.responseText);
		        
		        var response = eval("("+transport.responseText+")");  
		        if (response.result == true){   // Ajax请求处理成功
		            // 显示处理成功的信息
		            if (config.showSuccessMsg == true)
    				    TS.Msg.msgBox({title:config.title,msg: response.msg, iconCls: TS.Msg.IconCls.INFO,
    				        onOk: function(){
		                        if (config.closeMe == true){
		                            TS.closeWindow();
		                        }else if (config.refreshMe == true){
		                            if(typeof refresh == 'function') refresh();
		                        }
    				        }
    				    });
    				    
    				// 调用回调函数
		            if (typeof(config.callback) == 'function')
			            config.callback.call(this,response);
    				    
    				// 隐藏等待信息
		            if (config.wait && config.showSuccessMsg != true)
			            TS.Msg.hide();
			            
			        // 关闭当前窗口
		            if (config.closeMe == true && config.showSuccessMsg != true)
			            TS.closeWindow();
		        }else{                          // Ajax请求处理失败
		            // 显示处理失败的信息
		            if (config.showFailureMsg == true)
    				    TS.Msg.msgBox({title:config.title,msg: response.msg, iconCls: TS.Msg.IconCls.ERROR});
		        }
	        }
        }
        
        var _parameters = config.parameters || {};
        if(typeof _parameters == 'string')
            _parameters = _parameters.toQueryParams();  // url字符串转换为json对象结构
            
        // 附加需要额外序列化的页面控件的值参数
        if(config.serialize){
            var serializeObj = $(config.serialize);
            if(serializeObj){
                TS.applyIf(_parameters,TS.Form.serialize(serializeObj, true));
            }else{
                alert('id=' + config.serialize + '的对象不存在，config.serialize参数配置错误！');
            }
        }
        
        // 调用Prototype的Ajax.Request进行处理
        var myRequest = new Ajax.Request(config.url, 
        { 
	        method: config.method, 
	        parameters: _parameters,
	        onSuccess: onSuccess,
            onFailure: function(transport){
	            // 显示处理失败的信息
	            if (config.showFailureMsg == true)
    			    TS.Msg.msgBox({title:"系统错误",msg: config.failureMsg, iconCls: TS.Msg.IconCls.ERROR});
            }
        });
        return myRequest;
    }
}