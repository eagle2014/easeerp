var ECalendar=function(){
	var iframeID="ECalendarIframe";	//iframeID
	var divID="ECalendarDiv";		//divID
	var divObj=null;				//div对象
	var iframeObj=null;				//iframe对象
	var outText;
	var outButton;
	var oldDate=new Date();			//旧值
	var elxy = [0,0];
	var defaultelwh = {w:175,h:215};
	var elwh = {w:175,h:215};
	
	var xy=[0,0];
	var bDrag= false;
	return{
		defaultDateformat :"Y-m-d",
		defaultDateTimeformat :"Y-m-d H:i:s",
		monthFormat :"Y年m月",
		dayNames : ["日","一","二","三", "四",  "五",   "六"],
		monthNames:["1月","2月","3月","4月","5月","6月","7月","8月","9月","10月","11月","12月"],
		startDay : 1,//一周的第一天是? 1--星期一 0--星期日
		activeDate : new Date(),
		value : new Date(),
		todayText : "今日",
		timeTable:null,
		setDay:function(){
			outButton = $(arguments[0]);
			outText = $(arguments.length>1?arguments[1]:arguments[0]);
			
			//设置输入和返回的格式
			this.format = outText.readAttribute("format")||(/datetime/i.test(outText.className)?this.defaultDateTimeformat:this.defaultDateformat);

			if(iframeObj==null || divObj==null){
				this.init();
			}

			this.setDateTime(Date.parseDate(outText.value,this.format)||new Date());

			var arrfromat = /(H|h|g|g|A|a|i|s|O|Z)/;
			
			var isShowTime = arrfromat.test(this.format);
									
			if(this.timeTable){				
				if(!isShowTime){
					this.timeTable.hide();
					elwh.h=defaultelwh.h-20;
					this.setSize();
				}else{
					this.timeTable.show();
					elwh.h = defaultelwh.h;
					this.setSize();
				}
			}
			this.show();
		},
		show:function(){this.setXY();
			this.setSize();
			
			this.hideMonthPicker();
			this.hideTimePicker();
			iframeObj.style.visibility = 'visible';
			divObj.style.visibility = 'visible';
			iframeObj.show();
	    	divObj.show();
		},
		//统一设置容器的宽度和高度
		setSize:function(){
			[divObj,this.monthPicker,this.timePicker].each(function(e){
				e.style.width = elwh.w+"px";
				e.style.height = elwh.h+"px";
			})
			Position.clone(divObj,iframeObj,{
			    setTop:   false,
			    setLeft:  false
			});
		},
		setXY : function(){
	    	if(!outText) return ;
	   		var hei = outText.getHeight();
			var cw = divObj.getWidth(), ch = divObj.getHeight();	
    		var dw = document.body.clientWidth, dl = document.body.scrollLeft, dt = document.body.scrollTop;
    		var otxy = Position.page(outText);
    		var offTop,offLeft;
			if (document.body.clientHecight + dt - otxy[1] - hei >= ch) offTop = hei;
			else {offTop  = (otxy[1] - dt < ch) ? hei: - ch};
			if (dw + dl - otxy[0] >= cw) offLeft = 0; else offLeft = ((dw >= cw) ? dw - cw + dl : dl)-otxy[0];
	        var opt = {
			    setWidth:   false,
			    setHeight:  false,
      			offsetTop:  offTop,
      			offsetLeft: offLeft
			};
			Position.clone(outText,iframeObj,opt);
			Position.clone(outText,divObj,opt);
	    },
		init:function(){		
			var start  = new Date();
			//框架,为了挡住重型控件
			var ifr = document.createElement('iframe');
			ifr.id=iframeID;
			document.body.appendChild(ifr);
			iframeObj = $(iframeID);
			iframeObj.setStyle({position:'absolute',display:'block','z-index':14998}); 
			iframeObj.frameBorder="0";

			//主框架
			var div = document.createElement('div');
			document.body.appendChild(div);
			divObj=$(div);
			divObj.className='egd-date-picker';
			divObj.setStyle({'z-index':14999,'-moz-user-select':'none','-khtml-user-select':'none'}); 
			divObj.unselectable = "on";
			
			iframeObj.style.visibility = 'hidden';
			divObj.style.visibility = 'hidden';
			
			var m = [
             '<table cellspacing="0"><tr><td id="egd-date-moveBar" class="egd-date-moveBar"></td></tr>',
                '<tr><td><table cellspacing="0" width="100%"><tr><td class="egd-date-pyear"><a href="#" id="egd-date-pyear-btn" title="向前翻 1 年">&#160;</a></td>' ,
                '<td class="egd-date-pmonth"><a href="#" id="egd-date-pmonth-btn" title="向前翻 1 月">&#160;</a></td>' ,
                '<td class="egd-date-middle" align="center"><a href="#" id="egd-date-middle-btn">&#160;</a></td>' ,
                '<td class="egd-date-nmonth"><a href="#" id="egd-date-nmonth-btn" title="向后翻 1 月">&#160;</a></td>' ,
                '<td class="egd-date-nyear"><a href="#" id="egd-date-nyear-btn" title="向后翻 1 年">&#160;</a></td></tr></table></td></tr>',
                '<tr><td><table class="egd-date-inner" cellspacing="0"><thead><tr>'];
	        var dn = this.dayNames;
	        for(var i = 0; i < 7; i++){
	            var d = this.startDay+i;
	            if(d > 6){
	                d = d-7;
	            }
	            m.push("<th><span>", dn[d].substr(0,1), "</span></th>");
	        }
	        m[m.length] = "</tr></thead><tbody id='egd-date-inner'><tr>";
	        for(var i = 0; i < 42; i++) {
	            if(i % 7 == 0 && i != 0){
	                m[m.length] = "</tr><tr>";
	            }
	            m[m.length] = '<td><a href="#" hidefocus="on" class="egd-date-date" tabIndex="1"><em><span></span></em></a></td>';
	        }
	        m[m.length] = ['</tr></tbody></table></td></tr><tr><td colspan="3" id="egd-date-time-table" class="egd-date-time" align="center">' ,
	        		'<table cellspacing="0" style="width:100%;height:100%"><tr>' ,
	        		'<td  align="center">时间</td>' ,
	        		'<td width="30px"><a href="#" id="egd-date-hour-lable" class="egd-hour">00</a></td>' ,
	        		'<td width="10px"  align="center">:</td>' ,
	        		'<td width="30px"><a href="#" id="egd-date-minute-lable" class="egd-minute">00</a></td>' ,
	        		'<td width="10px"  align="center">:</td>' ,
	        		'<td width="30px"><a href="#" id="egd-date-second-lable" class="egd-second">00</a></td></tr></table>' ,
	        		'</td></tr><tr><td class="egd-date-bottom" align="center">' ,
	        		'<span id="egd-date-set-btn" class="egd-date-button">确定</span>' ,
	        		'<span id="egd-date-today-btn" class="egd-date-button">今天</span>' ,
	        		'<span id="egd-date-none-btn" class="egd-date-button">清空</span>' ,
	        		'<span id="egd-date-close-btn" class="egd-date-button">关闭</span>' ,
	        		'</td></tr></table>' ,
	        		'<div class="egd-date-mp"></div>'].join('');

	        divObj.innerHTML = m.join("");
			
			//月份选择div
			this.monthPicker = document.createElement( "div" );
			this.monthPicker.className = "egd-date-mp";
			divObj.appendChild(this.monthPicker);
			this.monthPicker = $(this.monthPicker);	
			this.monthPicker.hide();
			
			//月份选择div
			this.timePicker = document.createElement( "div" );
			this.timePicker.className = "egd-date-tp";
			divObj.appendChild(this.timePicker);
			this.timePicker = $(this.timePicker);	
			this.timePicker.hide();
			
			this.timeTable=$('egd-date-time-table');//时间框

			this.hourTd = $("egd-date-hour-lable");
			this.minuteTd = $("egd-date-minute-lable");
			this.secondTd = $("egd-date-second-lable");
			
			this.moveBar = $("egd-date-moveBar");
			
			this.mLabel = $("egd-date-middle-btn");
			
			var dateInner = $("egd-date-inner");
			this.cells = $A(dateInner.getElementsByTagName("td"));	//框
			this.textNodes = $A(dateInner.getElementsByTagName("span"));//数字区域
			
			var ed = this;
			$A(dateInner.getElementsByTagName("a")).each(function(element){				
				element.ondblclick=function(e){
					Event.stop(e);
					this.setValue(new Date(element.dateValue));
					return false;
				}.bindAsEventListener(ed);	
				element.onclick=function(e){
					Event.stop(e);
					this.setDate(new Date(element.dateValue));
					return false;
				}.bindAsEventListener(ed);			
			}.bind(this))
			
			$(this.mLabel).observe("click",this.showMonthPicker.bind(this));
			
			this.hourTd.observe("click",this.showTimePicker.bindAsEventListener(this,0,23,Date.HOUR));
			
			this.minuteTd.observe("click",this.showTimePicker.bindAsEventListener(this,0,59,Date.MINUTE));
			
			this.secondTd.observe("click",this.showTimePicker.bindAsEventListener(this,0,59,Date.SECOND));

			//今天 
			$("egd-date-today-btn").observe("click",function(){
				this.setValue(new Date(),true)
			}.bind(this));
			//清空
			$("egd-date-none-btn").observe("click",function(){
				outText.value = "";
				this.hide();
			}.bind(this));
			//确定
			$("egd-date-set-btn").observe("click",function(){
				this.setValue(this.activeDate)
			}.bind(this));	
			//关闭
			$("egd-date-close-btn").observe("click",function(){
				this.hide();
			}.bind(this));
				
			//上一年 
			$("egd-date-pyear-btn").observe("click",function(e){
				Event.stop(e);this.update(this.activeDate.add(Date.YEAR,-1)
			)}.bind(this));
			//下一年
			$("egd-date-nyear-btn").observe("click",function(e){
				Event.stop(e);this.update(this.activeDate.add(Date.YEAR,1))
			}.bindAsEventListener(this));

			//上一月 
			$("egd-date-pmonth-btn").observe("click",function(e){
				Event.stop(e);this.update(this.activeDate.add(Date.MONTH,-1)
			)}.bind(this));
			//下一月			
			$("egd-date-nmonth-btn").observe("click",function(e){
				Event.stop(e);this.update(this.activeDate.add(Date.MONTH,1))
			}.bindAsEventListener(this));
			
			Event.observe(document,'click', function(e){
				var srce = Event.element(e);
				if( srce!=outButton && !$(srce).childOf(divObj)){
					this.hide();
				}				
			}.bindAsEventListener(this), false);
			
			Event.observe(document,'mousemove', function(e){
				if(bDrag){
					divObj.style.left = Event.pointerX(e)+xy[0]+"px";
					divObj.style.top = Event.pointerY(e)+xy[1]+"px";
					Position.clone(divObj,iframeObj,{setWidth:false,setHeight:false});
				}
			}.bindAsEventListener(this), true);
			
			this.moveBar.observe('mousedown', function(e){	
				xy = [divObj.offsetLeft-Event.pointerX(e),divObj.offsetTop-Event.pointerY(e)];
				bDrag =true;
				document.body.ondrag = function() {return false;};
				document.body.onselectstart = function() {return false;};	
				
			}.bindAsEventListener(this), true);
						
			this.moveBar.observe('mouseup', function(e){				
				bDrag =false;
				document.body.ondrag = null;
				document.body.onselectstart = null;	
			}.bindAsEventListener(this), true);
		},
		//月份选择器
		createMonthPicker:function(){
			if(!this.monthPicker.firstChild){
				var buf = ['<table border="0" cellspacing="0">'];
	            for(var i = 0; i < 6; i++){
	                buf.push(
	                    '<tr><td class="egd-date-mp-month"><a href="#">', this.monthNames[i].substr(0, 3), '</a></td>',
	                    '<td class="egd-date-mp-month egd-date-mp-sep"><a href="#">', this.monthNames[i+6].substr(0, 3), '</a></td>',
	                    i == 0 ?
	                    '<td class="egd-date-mp-ybtn" align="center"><a class="egd-date-mp-prev"></a></td><td class="egd-date-mp-ybtn" align="center"><a class="egd-date-mp-next"></a></td></tr>' :
	                    '<td class="egd-date-mp-year"><a href="#"></a></td><td class="egd-date-mp-year"><a href="#"></a></td></tr>'
	                );
	            }
	            buf.push(
	                '<tr class="egd-date-mp-btns"><td colspan="4"><button type="button" class="egd-date-mp-ok">确定</button><button type="button" class="egd-date-mp-cancel">取消</button></td></tr>',
	                '</table>'
	            );
	            
	            this.monthPicker.update(buf.join(''));				
	
				this.monthPicker.observe('click', this.onMonthClick.bind(this));
	            this.monthPicker.observe('dblclick', this.onMonthDblClick.bind(this));
	            
				this.mpMonths = this.monthPicker.getElementsBySelector('td.egd-date-mp-month');
	            this.mpYears = this.monthPicker.getElementsBySelector('td.egd-date-mp-year');
	            
				this.monthPicker.getElementsBySelector('td.egd-date-mp-year');
				
	            this.mpMonths.each(function(m,i){
	                i += 1;
	                if((i%2) == 0){
	                    m.xmonth = 5 + Math.round(i * .5);
	                }else{
	                    m.xmonth = Math.round((i-1) * .5);
	                }
	            });
			}
		},
		showMonthPicker : function(e){
			Event.stop(e);
			this.createMonthPicker();
			this.monthPicker.show();
	        this.mpSelMonth = (this.activeDate || this.value).getMonth();
	        this.updateMPMonth(this.mpSelMonth);
	        this.mpSelYear = (this.activeDate || this.value).getFullYear();
	        this.updateMPYear(this.mpSelYear);	
	    },
	    updateMPYear : function(y){
      	  	this.mpyear = y;
	        var ys = this.mpYears;
	        for(var i = 1; i <= 10; i++){
	            var td = ys[i-1], y2;
	            if((i%2) == 0){
	                y2 = y + Math.round(i * .5);
	                td.firstChild.innerHTML = y2;
	                td.xyear = y2;
	            }else{
	                y2 = y - (5-Math.round(i * .5));
	                td.firstChild.innerHTML = y2;
	                td.xyear = y2;
	            }
	            this.mpYears[i-1][y2 == this.mpSelYear ? 'addClassName' : 'removeClassName']('egd-date-mp-sel');
	        }
	    },
		updateMPMonth : function(sm){
	        this.mpMonths.each(function(m,i){
	            m[m.xmonth == sm ? 'addClassName' : 'removeClassName']('egd-date-mp-sel');
	        });
	    },
	    onMonthClick : function(e){
	        Event.stop(e);
	        var el = Event.element(e), pn;
	        if(el==this.monthPicker.getElementsBySelector('button.egd-date-mp-cancel')[0]){
	            this.hideMonthPicker();
	        }
	        else if(el==this.monthPicker.getElementsBySelector('button.egd-date-mp-ok')[0]){
	            this.update(new Date(this.mpSelYear, this.mpSelMonth, (this.activeDate || this.value).getDate()));
	            this.hideMonthPicker();
	        }
	        else if(this.monthPicker.getElementsBySelector('td.egd-date-mp-month').include(el.parentNode)){
	            this.mpMonths.each(function(element){element.removeClassName('egd-date-mp-sel')});
	            $(el.parentNode).addClassName('egd-date-mp-sel');
	            this.mpSelMonth = el.parentNode.xmonth;
	        }
	        else if(this.monthPicker.getElementsBySelector('td.egd-date-mp-year').include(el.parentNode)){
	            this.mpYears.each(function(element){element.removeClassName('egd-date-mp-sel')});
	            $(el.parentNode).addClassName('egd-date-mp-sel');
	            this.mpSelYear = el.parentNode.xyear;
	        }
	        else if(el==this.monthPicker.getElementsBySelector('a.egd-date-mp-prev')[0]){
	            this.updateMPYear(this.mpyear-10);
	        }
	        else if(el==this.monthPicker.getElementsBySelector('a.egd-date-mp-next')[0]){
	            this.updateMPYear(this.mpyear+10);
	        }
	    },
	
	    onMonthDblClick : function(e, t){
	      	Event.stop(e);
	        var el = Event.element(e), pn;
	        if(this.monthPicker.getElementsBySelector('td.egd-date-mp-month').include(el.parentNode)){
	            this.update(new Date(this.mpSelYear, el.parentNode.xmonth, (this.activeDate || this.value).getDate()));
	            this.hideMonthPicker();
	        }
	        else if(this.monthPicker.getElementsBySelector('td.egd-date-mp-year').include(el.parentNode)){
	            this.update(new Date(el.parentNode.xyear, this.mpSelMonth, (this.activeDate || this.value).getDate()));
	            this.hideMonthPicker();
	        }
	    },
	     hideMonthPicker : function(disableAnim){
	        if(this.monthPicker){
	           this.monthPicker.hide();
	        }
	    },
	    
	    
	    //时间选择器
		createTimePicker:function(){
			if(!this.timePicker.firstChild){
				var buf = ['<table border="0" cellspacing="0">'];
	            for(var i = 0; i < 10; i++){
	                buf.push(
	                    '<tr><td class="egd-date-tp-time egd-date-tp-sep"><a href="#">0</a></td>',
	                    '<td class="egd-date-tp-time egd-date-tp-sep"><a href="#">0</a></td>',
	                    '<td class="egd-date-tp-time egd-date-tp-sep"><a href="#">0</a></td>',
	                    '<td class="egd-date-tp-time egd-date-tp-sep"><a href="#">0</a></td>',
	                    '<td class="egd-date-tp-time egd-date-tp-sep"><a href="#">0</a></td>',
	                    '<td class="egd-date-tp-time "><a href="#">0</a></td></tr>'
	                );
	            }
	            buf.push(
	                '<tr class="egd-date-tp-btns"><td colspan="6"><button type="button" class="egd-date-tp-cancel">取消</button></td></tr>',
	                '</table>'
	            );
	            this.timePicker.update(buf.join(''));				
				this.timePicker.observe('click', this.onTimeClick.bind(this));
				this.tpTimes = this.timePicker.getElementsBySelector('td.egd-date-tp-time');
			}
		},
		showTimePicker : function(e,start,end,interval){
			this.interval = interval;
			tpobj = Event.element(e);			
			Event.stop(e);
			this.createTimePicker();
			this.timePicker.show();
	        this.mpSelTime = parseInt(tpobj.innerHTML);
	        var ts = this.tpTimes;

      		for(var i = 0; i < 60; i++){
				var val= (i%6)*10+ Math.floor(i/6)+start;
				var td = ts[i], y2;
				if(val<=end){					
		            y2 = String.leftPad(val, 2, '0');
		            td.firstChild.innerHTML = y2;
		            td.xtime = val;
		            td[td.xtime == this.mpSelTime ? 'addClassName' : 'removeClassName']('egd-date-tp-sel');
				}else{
		            td.firstChild.innerHTML = '&#160';
		            td.xtime = -1;
		            td.removeClassName('egd-date-tp-sel');
				}
	           
	        }
	    },
	    onTimeClick : function(e){
	        Event.stop(e);
	        var el = Event.element(e), pn;
	        if(el==this.timePicker.getElementsBySelector('button.egd-date-tp-cancel')[0]){
	            this.hideTimePicker();
	        }
	        else if(el==this.timePicker.getElementsBySelector('button.egd-date-tp-ok')[0]){
	            this.update(new Date(this.mpSelYear, this.mpSelMonth, (this.activeDate || this.value).getDate()));
	            this.hideTimePicker();
	        }
	        else if(this.timePicker.getElementsBySelector('td.egd-date-tp-time').include(el.parentNode)){
	        	if(el.parentNode.xtime==-1) return ;
	            this.tpTimes.each(function(element){element.removeClassName('egd-date-tp-sel')});
	            $(el.parentNode).addClassName('egd-date-tp-sel');
	            switch(this.interval.toLowerCase()){
					case Date.SECOND:
				      this.value.setSeconds(el.parentNode.xtime);
				      break;
				    case Date.MINUTE:
				      this.value.setMinutes(el.parentNode.xtime);
				      break;
				    case Date.HOUR:
				      this.value.setHours(el.parentNode.xtime);
				      break;
				}
				this.hideTimePicker();
				this.updateTime();
	        }
	    },
	    hideTimePicker : function(disableAnim){
	        if(this.timePicker){
	           this.timePicker.hide();
	        }
	    },	    
		setValue:function(date,flag){
			flag?this.setDateTime(date):this.setDate(date);
			outText.value = this.value.format(this.format);
			this.hide();
		},
		update : function(date){
			this.activeDate = date;
			var days = date.getDaysInMonth();

	        var firstOfMonth = date.getFirstDateOfMonth();
	        var startingPos = firstOfMonth.getDay()-this.startDay;
	
	        if(startingPos <= this.startDay){
	            startingPos += 7;
	        }
	
	        var pm = date.add("mo", -1);
	        var prevStart = pm.getDaysInMonth()-startingPos;
	
	        var cells = this.cells;
	        var textEls = this.textNodes;
	        days += startingPos;
	        
	        var d = (new Date(pm.getFullYear(), pm.getMonth(), prevStart)).clearTime();
        	var today = new Date().clearTime().getTime();
	        var sel = this.value.clearTime(true).getTime();
	        
	        var setCellClass = function(cal, cell){
	            cell.title = "";
	            var t = d.getTime();
	            cell.firstChild.dateValue = t;
	            if(t == today){
	                cell.className += " egd-date-today";
	                cell.title = cal.todayText;
	            }
	            if(t == sel){
	                cell.className += " egd-date-selected";
	                setTimeout(function(){
	                    try{cell.firstChild.focus();}catch(e){}
	                }, 50);
	            }
	        };
        
	        var i = 0;
	        for(; i < startingPos; i++) {
	            textEls[i].innerHTML = (++prevStart);
	            d.setDate(d.getDate()+1);
	            cells[i].className = "egd-date-prevday";
	            setCellClass(this, cells[i]);
	        }
	        for(; i < days; i++){
	            intDay = i - startingPos + 1;
	            textEls[i].innerHTML = (intDay);
	            d.setDate(d.getDate()+1);
	            cells[i].className = "egd-date-active";
	            setCellClass(this, cells[i]);
	        }
	        var extraDays = 0;
	        for(; i < 42; i++) {
	             textEls[i].innerHTML = (++extraDays);
	             d.setDate(d.getDate()+1);
	             cells[i].className = "egd-date-nextday";
	             setCellClass(this, cells[i]);
	        }
	        
	        var str = date.format(this.monthFormat);
			if ( this.mLabel != null ) this.mLabel.innerHTML = str;
		},
		updateTime:function(){			
			this.hourTd.update(this.value.format('H'));
			this.minuteTd.update(this.value.format('i'));
			this.secondTd.update(this.value.format('s'));
		},
		setDate : function(date){
			this.value.setMonth(date.getMonth());
			this.value.setDate(date.getDate());
			this.value.setFullYear(date.getFullYear()) ;			
			this.update(this.value);
			this.updateTime();
		},
		setDateTime : function(dateTime){
			this.value = dateTime ;
			this.update(this.value);
			this.updateTime();
		},
		hide:function(){
	    	iframeObj.style.visibility = 'hidden';
			divObj.style.visibility = 'hidden';
	    },
	    initFields:function(){
	    	$$("input.egd-warpDateField").each(function(element){
			var selectBtn = document.createElement( "div" );
			selectBtn.className = "egd-selectBtn";
			selectBtn.appendChild( document.createTextNode( "X" ) );
			selButton = $(selButton);
			element.parentNode.insertBefore(selButton,element.nextSibling);
			element.style.width = element.clientWidth - 20;
				element.observe('mousedown',function(e){
					ECalendar.setDay(element);
					return false;
				},true);						
			});	
			
			$$("input.egd-dateTimeField").each(function(element){
				element.observe('mousedown',function(e){
					ECalendar.setDay(element);
					return false;
				},true);						
			});	
			
			$$("input.egd-dateField").each(function(element){
				element.observe('mousedown',function(e){
					ECalendar.setDay(element);
					return false;
				},true);						
			});	
			
			$$("div.egd-selDate").each(function(element){
				var pre = element.previous();
				if(pre && pre.tagName.toLowerCase() == 'input'&& pre.tagName.toLowerCase() == 'input'){
					element.observe('click',function(e){
						ECalendar.setDay(element,pre);
					},true);
					pre.observe('focus',function(e){
						ECalendar.setDay(pre);
					},true);
				}								
			});
			$$("div.egd-selDateTime").each(function(element){
				var pre = element.previous();
				if(pre && pre.tagName.toLowerCase() == 'input'&& pre.tagName.toLowerCase() == 'input'){
					element.observe('click',function(e){
						ECalendar.setDay(element,pre);
					},true);
					pre.observe('focus',function(e){
						ECalendar.setDay(pre);
					},true);
				}								
			});
	    },
		bindFields:function(){
			var arg = [];
			if(arguments.length>1){
				arg = $A(arguments);
			}else if(arguments.length===1 && (typeof arguments[0]=='string')){
				arg = $w(arguments[0]);
			}else if(arguments.length===1 && (arguments[0] instanceof Array)){
				arg = arguments[0];
			}
			var cal = this;
			arg.each(function(obj){
				if($(obj)){
					cal.bindFiled(obj);
				}				
			})			
		},
		bindFiled:function(element){
			element = $(element);
			if(element.hasClassName("egd-dateTimeField")){
				element.observe('mousedown',function(e){
					ECalendar.setDay(element);
					return false;
				},true);						
			}else if(element.hasClassName("egd-dateField")){
				element.observe('mousedown',function(e){
					ECalendar.setDay(element);
					return false;
				},true);						
			};	
		}
	}
}();
Event.observe(window,'load', function(){			
//			ECalendar.initFields();
//			var allScript = document.getElementsByTagName("script");
//			var theOne = null,s;
//			for (var i = 0, length = allScript.length; i < length; i++){
//				s = allScript[i];
//				if (s.src && s.src.match(/Calendar\.js(\?.*)?$/i)){
//					theOne = s;
//					break;
//				}
//			}
//			if (theOne != null){
//			  var preinit = theOne.src.replace(/.*Calendar\.js(\?preinit=)?/i,'');
//			  if (preinit != "false"){
//			  	//alert("init data...");			  	   
//					ECalendar.init();
//				}
//			}		

	ECalendar.init();	

	}, false);