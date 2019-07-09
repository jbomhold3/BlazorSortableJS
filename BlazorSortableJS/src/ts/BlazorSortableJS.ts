import IBlazorSortableJsOptions from "./IBlazorSortableJsOptions";
let SortableLists: any = [];
let whitelist = ["id", "tagName", "className", "childNodes"];
declare var Sortable: any;
declare var DotNet: any;
const domWindow = window as any;
const namespace: string = "BlazorSortableJS";

function GetSafeEventArgs(refId: string, e: any, x: any = {}) {
    var result = {
        refId: refId || "",
        dataId: (e.item) ? e.item['dataset']['id'] || "-1" : e.dragged['dataset']['id'] ,
        data: SortableLists[refId]['ChoiceItem'] || "",
        item: JSON.stringify(e.item, whitelist),
        to: JSON.stringify(e.to, whitelist),
        from: JSON.stringify(e.from, whitelist),
        oldIndex: e.oldIndex || -10,
        newIndex: e.newIndex || -10,
        oldDraggableIndex: e.oldDraggableIndex || -10,
        newDraggableIndex: e.newDraggableIndex || -1,
        clone: e.clone || "",
        pullMode: e.pullMode || "",
        dragged: e.dragged || "",
        draggedRect: e.draggedRect || {left:0,right:0,top:0,bottom:0},
        related: e.related || "",
        relatedRect: e.relatedRect || {left:0,right:0,top:0,bottom:0},
        willInsertAfter: e.willInsertAfter || "",
        clientY: x['clientY'] || 0,
        clientX: x['clientX'] || 0,
    }
    return result;
}

function GetSafeAddEventArgs(refId: string, e: any) {
    console.log(e);
    var result = {

        refId: refId || "",
        dataId: (e.item) ? e.item['dataset']['id'] || "-1" : e.dragged['dataset']['id'],
        data: SortableLists[e.from['refId']]['ChoiceItem'] || "",
        item: JSON.stringify(e.item, whitelist),
        to: JSON.stringify(e.to, whitelist),
        from: JSON.stringify(e.from, whitelist),
        oldIndex: e.oldIndex || -10,
        newIndex: e.newIndex || -10,
        oldDraggableIndex: e.oldDraggableIndex || -10,
        newDraggableIndex: e.newDraggableIndex || -1,
        clone: e.clone || "",
        pullMode: e.pullMode || "",
        dragged: e.dragged || "",
        draggedRect: e.draggedRect || { left: 0, right: 0, top: 0, bottom: 0 },
        related: e.related || "",
        relatedRect: e.relatedRect || { left: 0, right: 0, top: 0, bottom: 0 },
        willInsertAfter: e.willInsertAfter || "",
    }
    return result;
}
domWindow.BlazorSortableJS = { SortableJs: Function }
domWindow.BlazorSortableJS.Create = (refId:string, elId: string, opt: IBlazorSortableJsOptions) => {
    var el = document.getElementById(elId);
    opt.onChoose = (e: any) => DotNet.invokeMethodAsync(namespace, "OnChoose",GetSafeEventArgs(refId,e));
    opt.onUnchoose = (e: any) => DotNet.invokeMethodAsync(namespace, "OnUnchoose", GetSafeEventArgs(refId,e));
    opt.onStart = (e: any) => DotNet.invokeMethodAsync(namespace, "OnStart", GetSafeEventArgs(refId,e));
    opt.onEnd = (e: any) => DotNet.invokeMethodAsync(namespace, "OnEnd", GetSafeEventArgs(refId,e));
    opt.onAdd = (e: any) => { DotNet.invokeMethodAsync(namespace, "OnAdd", GetSafeAddEventArgs(refId, e)); };
    opt.onSort = (e: any) => DotNet.invokeMethodAsync(namespace, "OnSort", GetSafeEventArgs(refId,e));
    opt.onRemove = (e: any) => DotNet.invokeMethodAsync(namespace, "OnRemove", GetSafeEventArgs(refId,e));
    opt.onFilter = (e: any) => DotNet.invokeMethodAsync(namespace, "OnFilter", GetSafeEventArgs(refId,e));
    opt.onMove = (e: any) => DotNet.invokeMethodAsync(namespace, "OnMove", GetSafeEventArgs(refId,e));
    opt.onClone = (e: any) => DotNet.invokeMethodAsync(namespace, "OnClone", GetSafeEventArgs(refId,e));
    opt.onChange = (e: any) => DotNet.invokeMethodAsync(namespace, "OnChange", GetSafeEventArgs(refId, e));
    SortableLists[refId] = { Sortable: null, ChoiceItem: null };
    SortableLists[refId]['Sortable'] = Sortable.create(el, opt);
};

domWindow.BlazorSortableJS.SetChoiceItem = (refId: string, data: string) => {
    return SortableLists[refId]['ChoiceItem'] = data;
}

domWindow.BlazorSortableJS.ToArray = (refId: string) => {
    return SortableLists[refId]['Sortable'].toArray();
}

domWindow.BlazorSortableJS.Sort = (refId: string, order:string[]) => {
    return SortableLists[refId]['Sortable'].sort(order);
}

domWindow.BlazorSortableJS.Destroy = (refId: string, order: string[]) => {
    SortableLists[refId]['Sortable'].destroy();
    var index = SortableLists.indexOf(SortableLists[refId]);
    SortableLists.splice(index,1);
}