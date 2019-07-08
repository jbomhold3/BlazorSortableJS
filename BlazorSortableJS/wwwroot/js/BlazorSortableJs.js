/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 0);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ (function(module, exports) {

let whitelist = ["id", "tagName", "className", "childNodes"];
const domWindow = window;
const namespace = "BlazorSortableJS";
function GetSafeEventArgs(refId, e, x = {}) {
    var result = {
        refId: refId || "",
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
        clientY: x['clientY'] || 0,
        clientX: x['clientX'] || 0,
    };
    return result;
}
domWindow.BlazorSortableJS = { SortableJs: Function };
domWindow.BlazorSortableJS.SortableJs = (refId, elId, opt) => {
    var el = document.getElementById(elId);
    opt.onChoose = (e) => DotNet.invokeMethodAsync(namespace, "OnChoose", GetSafeEventArgs(refId, e));
    opt.onUnchoose = (e) => DotNet.invokeMethodAsync(namespace, "OnUnchoose", GetSafeEventArgs(refId, e));
    opt.onStart = (e) => DotNet.invokeMethodAsync(namespace, "OnStart", GetSafeEventArgs(refId, e));
    opt.onEnd = (e) => DotNet.invokeMethodAsync(namespace, "OnEnd", GetSafeEventArgs(refId, e));
    opt.onAdd = (e) => DotNet.invokeMethodAsync(namespace, "OnAdd", GetSafeEventArgs(refId, e));
    opt.onSort = (e) => DotNet.invokeMethodAsync(namespace, "OnSort", GetSafeEventArgs(refId, e));
    opt.onRemove = (e) => DotNet.invokeMethodAsync(namespace, "OnRemove", GetSafeEventArgs(refId, e));
    opt.onFilter = (e) => DotNet.invokeMethodAsync(namespace, "OnFilter", GetSafeEventArgs(refId, e));
    opt.onMove = (e) => DotNet.invokeMethodAsync(namespace, "OnMove", GetSafeEventArgs(refId, e));
    opt.onClone = (e) => DotNet.invokeMethodAsync(namespace, "OnClone", GetSafeEventArgs(refId, e));
    opt.onChange = (e) => DotNet.invokeMethodAsync(namespace, "OnChange", GetSafeEventArgs(refId, e));
    Sortable.create(el, opt);
};


/***/ })
/******/ ]);