/* 
 * 页签右键菜单
 * 2007-12-01 Tony
 */

Ext.ux.TabCloseMenu = function(){
    var tabs, menu, ctxItem;
    this.init = function(tp){
        tabs = tp;
        tabs.on('contextmenu', onContextMenu);
        //tabs.body.on('click', function(){if(menu) menu.hide();});
    }

    function onContextMenu(ts, item, e){
        if(!menu){ // create context menu on first right click
            menu = new Ext.menu.Menu([{
                id: tabs.id + '-close',
                text: '关闭',
                handler : function(){
                    tabs.remove(ctxItem);
                },
                iconCls: "egd-menuItem-close"
            },{
                id: tabs.id + '-close-others',
                text: '关闭其它窗口',
                handler : function(){
                    tabs.items.each(function(item){
                        if(item.closable && item != ctxItem){
                            tabs.remove(item);
                        }
                    });
                },
                iconCls: "egd-menuItem-close-others"
            },{
                id: tabs.id + '-close-all',
                text: '关闭所有窗口',
                handler : function(){
                    tabs.items.each(function(item){
                        if(item.closable){
                            tabs.remove(item);
                        }
                    });
                },
                iconCls: "egd-menuItem-close-all"
            }]);
        }
        ctxItem = item;
        var items = menu.items;
        items.get(tabs.id + '-close').setDisabled(!item.closable);
        var disableOthers = true;
        tabs.items.each(function(){
            if(this != item && this.closable){
                disableOthers = false;
                return false;
            }
        });
        items.get(tabs.id + '-close-others').setDisabled(disableOthers);
        items.get(tabs.id + '-close-all').setDisabled(disableOthers);
        menu.showAt(e.getPoint());
    }
};