var dataGrid;
function initGrid()
{
    dataGrid=new TS.Grid("gridContainer",{
    selectSelect:true,
    data:TS.rootPath+"placeAction.do?action=View",
    render:{root:'rows',totalProperty:'totalCount',id:'ID'},
    cm:[
    {id:"Name",text:"名称",width:50},
    {id:"Code",text:"编码"},
    {id:"Memo",text:"备注",width:60}
    ],
    idColumn:{type:"int",viewType:"checkbox"},
    defaultSort:{name:'Code',direction:'asc'},
    pageNo:1,
    pageSize:25
    });
    dataGrid.render();
}
