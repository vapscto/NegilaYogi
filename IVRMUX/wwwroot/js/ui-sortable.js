﻿/**
 * angular-ui-sortable - This directive allows you to jQueryUI Sortable.
 * @version v0.19.0 - 2018-01-14
 * @link https://angular-ui.github.com
 * @license MIT
 */
(function (window, angular, undefined) {
    'use strict'; angular.module('ui.sortable', []).value('uiSortableConfig', { items: '> [ng-repeat],> [data-ng-repeat],> [x-ng-repeat]' }).directive('uiSortable', ['uiSortableConfig', '$timeout', '$log', function (uiSortableConfig, $timeout, $log) {
        return {
            require: '?ngModel', scope: { ngModel: '=', uiSortable: '=', create: '&uiSortableCreate', start: '&uiSortableStart', activate: '&uiSortableActivate', beforeStop: '&uiSortableBeforeStop', update: '&uiSortableUpdate', remove: '&uiSortableRemove', receive: '&uiSortableReceive', deactivate: '&uiSortableDeactivate', stop: '&uiSortableStop' }, link: function (scope, element, attrs, ngModel) {
                var savedNodes; var helper; function combineCallbacks(first, second) {
                    var firstIsFunc = typeof first === 'function'; var secondIsFunc = typeof second === 'function'; if (firstIsFunc && secondIsFunc) { return function () { first.apply(this, arguments); second.apply(this, arguments) } } else if (secondIsFunc) { return second }
                    return first
                }
                function getSortableWidgetInstance(element) {
                    var data = element.data('ui-sortable'); if (data && typeof data === 'object' && data.widgetFullName === 'ui-sortable') { return data }
                    return null
                }
                function setItemChildrenWidth(item) { item.children().each(function () { var $el = angular.element(this); $el.width($el.width()) }) }
                function dummyHelper(e, item) { return item }
                function patchSortableOption(key, value) {
                    if (callbacks[key]) {
                        if (key === 'stop') { value = combineCallbacks(value, function () { scope.$apply() }); value = combineCallbacks(value, afterStop) }
                        value = combineCallbacks(callbacks[key], value)
                    } else if (wrappers[key]) { value = wrappers[key](value) }
                    if (!value && (key === 'items' || key === 'ui-model-items')) { value = uiSortableConfig.items }
                    return value
                }
                function patchUISortableOptions(newOpts, oldOpts, sortableWidgetInstance) {
                    function addDummyOptionKey(value, key) { if (!(key in opts)) { opts[key] = null } }
                    angular.forEach(callbacks, addDummyOptionKey); var optsDiff = null; if (oldOpts) {
                        var defaultOptions; angular.forEach(oldOpts, function (oldValue, key) {
                            if (!newOpts || !(key in newOpts)) {
                                if (key in directiveOpts) {
                                    if (key === 'ui-floating') { opts[key] = 'auto' } else { opts[key] = patchSortableOption(key, undefined) }
                                    return
                                }
                                if (!defaultOptions) { defaultOptions = angular.element.ui.sortable().options }
                                var defaultValue = defaultOptions[key]; defaultValue = patchSortableOption(key, defaultValue); if (!optsDiff) { optsDiff = {} }
                                optsDiff[key] = defaultValue; opts[key] = defaultValue
                            }
                        })
                    }
                    newOpts = angular.extend({}, newOpts); angular.forEach(newOpts, function (value, key) {
                        if (key in directiveOpts) {
                            if (key === 'ui-floating' && (value === !1 || value === !0) && sortableWidgetInstance) { sortableWidgetInstance.floating = value }
                            if (key === 'ui-preserve-size' && (value === !1 || value === !0)) {
                                var userProvidedHelper = opts.helper; newOpts.helper = function (e, item) {
                                    if (opts['ui-preserve-size'] === !0) { setItemChildrenWidth(item) }
                                    return (userProvidedHelper || dummyHelper).apply(this, arguments)
                                }
                            }
                            opts[key] = patchSortableOption(key, value)
                        }
                    }); angular.forEach(newOpts, function (value, key) {
                        if (key in directiveOpts) { return }
                        value = patchSortableOption(key, value); if (!optsDiff) { optsDiff = {} }
                        optsDiff[key] = value; opts[key] = value
                    }); return optsDiff
                }
                function getPlaceholderElement(element) {
                    var placeholder = element.sortable('option', 'placeholder'); if (placeholder && placeholder.element && typeof placeholder.element === 'function') { var result = placeholder.element(); result = angular.element(result); return result }
                    return null
                }
                function getPlaceholderExcludesludes(element, placeholder) { var notCssSelector = opts['ui-model-items'].replace(/[^,]*>/g, ''); var excludes = element.find('[class="' + placeholder.attr('class') + '"]:not(' + notCssSelector + ')'); return excludes }
                function hasSortingHelper(element, ui) { var helperOption = element.sortable('option', 'helper'); return (helperOption === 'clone' || (typeof helperOption === 'function' && ui.item.sortable.isCustomHelperUsed())) }
                function getSortingHelper(element, ui) {
                    var result = null; if (hasSortingHelper(element, ui) && element.sortable('option', 'appendTo') === 'parent') { result = helper }
                    return result
                }
                function isFloating(item) { return (/left|right/.test(item.css('float')) || /inline|table-cell/.test(item.css('display'))) }
                function getElementContext(elementScopes, element) { for (var i = 0; i < elementScopes.length; i++) { var c = elementScopes[i]; if (c.element[0] === element[0]) { return c } } }
                function afterStop(e, ui) { ui.item.sortable._destroy() }
                function getItemIndex(item) { return item.parent().find(opts['ui-model-items']).index(item) }
                var opts = {}; var directiveOpts = { 'ui-floating': undefined, 'ui-model-items': uiSortableConfig.items, 'ui-preserve-size': undefined }; var callbacks = { create: null, start: null, activate: null, beforeStop: null, update: null, remove: null, receive: null, deactivate: null, stop: null }; var wrappers = { helper: null }; angular.extend(opts, directiveOpts, uiSortableConfig, scope.uiSortable); if (!angular.element.fn || !angular.element.fn.jquery) { $log.error('ui.sortable: jQuery should be included before AngularJS!'); return }
                function wireUp() {
                    scope.$watchCollection('ngModel', function () { $timeout(function () { if (!!getSortableWidgetInstance(element)) { element.sortable('refresh') } }, 0, !1) }); callbacks.start = function (e, ui) {
                        if (opts['ui-floating'] === 'auto') { var siblings = ui.item.siblings(); var sortableWidgetInstance = getSortableWidgetInstance(angular.element(e.target)); sortableWidgetInstance.floating = isFloating(siblings) }
                        var index = getItemIndex(ui.item); ui.item.sortable = { model: ngModel.$modelValue[index], index: index, source: element, sourceList: ui.item.parent(), sourceModel: ngModel.$modelValue, cancel: function () { ui.item.sortable._isCanceled = !0 }, isCanceled: function () { return ui.item.sortable._isCanceled }, isCustomHelperUsed: function () { return !!ui.item.sortable._isCustomHelperUsed }, _isCanceled: !1, _isCustomHelperUsed: ui.item.sortable._isCustomHelperUsed, _destroy: function () { angular.forEach(ui.item.sortable, function (value, key) { ui.item.sortable[key] = undefined }) }, _connectedSortables: [], _getElementContext: function (element) { return getElementContext(this._connectedSortables, element) } }
                    }; callbacks.activate = function (e, ui) { var isSourceContext = ui.item.sortable.source === element; var savedNodesOrigin = isSourceContext ? ui.item.sortable.sourceList : element; var elementContext = { element: element, scope: scope, isSourceContext: isSourceContext, savedNodesOrigin: savedNodesOrigin }; ui.item.sortable._connectedSortables.push(elementContext); savedNodes = savedNodesOrigin.contents(); helper = ui.helper; var placeholder = getPlaceholderElement(element); if (placeholder && placeholder.length) { var excludes = getPlaceholderExcludesludes(element, placeholder); savedNodes = savedNodes.not(excludes) } }; callbacks.update = function (e, ui) {
                        if (!ui.item.sortable.received) { ui.item.sortable.dropindex = getItemIndex(ui.item); var droptarget = ui.item.parent().closest('[ui-sortable], [data-ui-sortable], [x-ui-sortable]'); ui.item.sortable.droptarget = droptarget; ui.item.sortable.droptargetList = ui.item.parent(); var droptargetContext = ui.item.sortable._getElementContext(droptarget); ui.item.sortable.droptargetModel = droptargetContext.scope.ngModel; element.sortable('cancel') }
                        var sortingHelper = !ui.item.sortable.received && getSortingHelper(element, ui, savedNodes); if (sortingHelper && sortingHelper.length) { savedNodes = savedNodes.not(sortingHelper) }
                        var elementContext = ui.item.sortable._getElementContext(element); savedNodes.appendTo(elementContext.savedNodesOrigin); if (ui.item.sortable.received) { savedNodes = null }
                        if (ui.item.sortable.received && !ui.item.sortable.isCanceled()) { scope.$apply(function () { ngModel.$modelValue.splice(ui.item.sortable.dropindex, 0, ui.item.sortable.moved) }); scope.$emit('ui-sortable:moved', ui) }
                    }; callbacks.stop = function (e, ui) {
                        var wasMoved = 'dropindex' in ui.item.sortable && !ui.item.sortable.isCanceled(); if (wasMoved && !ui.item.sortable.received) { scope.$apply(function () { ngModel.$modelValue.splice(ui.item.sortable.dropindex, 0, ngModel.$modelValue.splice(ui.item.sortable.index, 1)[0]) }); scope.$emit('ui-sortable:moved', ui) } else if (!wasMoved && !angular.equals(element.contents().toArray(), savedNodes.toArray())) {
                            var sortingHelper = getSortingHelper(element, ui, savedNodes); if (sortingHelper && sortingHelper.length) { savedNodes = savedNodes.not(sortingHelper) }
                            var elementContext = ui.item.sortable._getElementContext(element); savedNodes.appendTo(elementContext.savedNodesOrigin)
                        }
                        savedNodes = null; helper = null
                    }; callbacks.receive = function (e, ui) { ui.item.sortable.received = !0 }; callbacks.remove = function (e, ui) {
                        if (!('dropindex' in ui.item.sortable)) { element.sortable('cancel'); ui.item.sortable.cancel() }
                        if (!ui.item.sortable.isCanceled()) { scope.$apply(function () { ui.item.sortable.moved = ngModel.$modelValue.splice(ui.item.sortable.index, 1)[0] }) }
                    }; angular.forEach(callbacks, function (value, key) { callbacks[key] = combineCallbacks(callbacks[key], function () { var attrHandler = scope[key]; var attrHandlerFn; if (typeof attrHandler === 'function' && ('uiSortable' + key.substring(0, 1).toUpperCase() + key.substring(1)).length && typeof (attrHandlerFn = attrHandler()) === 'function') { attrHandlerFn.apply(this, arguments) } }) }); wrappers.helper = function (inner) {
                        if (inner && typeof inner === 'function') { return function (e, item) { var oldItemSortable = item.sortable; var index = getItemIndex(item); item.sortable = { model: ngModel.$modelValue[index], index: index, source: element, sourceList: item.parent(), sourceModel: ngModel.$modelValue, _restore: function () { angular.forEach(item.sortable, function (value, key) { item.sortable[key] = undefined }); item.sortable = oldItemSortable } }; var innerResult = inner.apply(this, arguments); item.sortable._restore(); item.sortable._isCustomHelperUsed = item !== innerResult; return innerResult } }
                        return inner
                    }; scope.$watchCollection('uiSortable', function (newOpts, oldOpts) { var sortableWidgetInstance = getSortableWidgetInstance(element); if (!!sortableWidgetInstance) { var optsDiff = patchUISortableOptions(newOpts, oldOpts, sortableWidgetInstance); if (optsDiff) { element.sortable('option', optsDiff) } } }, !0); patchUISortableOptions(opts)
                }
                function init() {
                    if (ngModel) { wireUp() } else { $log.info('ui.sortable: ngModel not provided!', element) }
                    element.sortable(opts)
                }
                function initIfEnabled() {
                    if (scope.uiSortable && scope.uiSortable.disabled) { return !1 }
                    init(); initIfEnabled.cancelWatcher(); initIfEnabled.cancelWatcher = angular.noop; return !0
                }
                initIfEnabled.cancelWatcher = angular.noop; if (!initIfEnabled()) { initIfEnabled.cancelWatcher = scope.$watch('uiSortable.disabled', initIfEnabled) }
            }
        }
    }])
})(window, window.angular)