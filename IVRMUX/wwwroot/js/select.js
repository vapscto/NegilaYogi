/*!
 * ui-select
 * http://github.com/angular-ui/ui-select
 * Version: 0.19.8 - 2017-04-18T05:43:43.673Z
 * License: MIT
 */
(function () {
    "use strict"; var KEY = {
        TAB: 9, ENTER: 13, ESC: 27, SPACE: 32, LEFT: 37, UP: 38, RIGHT: 39, DOWN: 40, SHIFT: 16, CTRL: 17, ALT: 18, PAGE_UP: 33, PAGE_DOWN: 34, HOME: 36, END: 35, BACKSPACE: 8, DELETE: 46, COMMAND: 91, MAP: { 91: "COMMAND", 8: "BACKSPACE", 9: "TAB", 13: "ENTER", 16: "SHIFT", 17: "CTRL", 18: "ALT", 19: "PAUSEBREAK", 20: "CAPSLOCK", 27: "ESC", 32: "SPACE", 33: "PAGE_UP", 34: "PAGE_DOWN", 35: "END", 36: "HOME", 37: "LEFT", 38: "UP", 39: "RIGHT", 40: "DOWN", 43: "+", 44: "PRINTSCREEN", 45: "INSERT", 46: "DELETE", 48: "0", 49: "1", 50: "2", 51: "3", 52: "4", 53: "5", 54: "6", 55: "7", 56: "8", 57: "9", 59: ";", 61: "=", 65: "A", 66: "B", 67: "C", 68: "D", 69: "E", 70: "F", 71: "G", 72: "H", 73: "I", 74: "J", 75: "K", 76: "L", 77: "M", 78: "N", 79: "O", 80: "P", 81: "Q", 82: "R", 83: "S", 84: "T", 85: "U", 86: "V", 87: "W", 88: "X", 89: "Y", 90: "Z", 96: "0", 97: "1", 98: "2", 99: "3", 100: "4", 101: "5", 102: "6", 103: "7", 104: "8", 105: "9", 106: "*", 107: "+", 109: "-", 110: ".", 111: "/", 112: "F1", 113: "F2", 114: "F3", 115: "F4", 116: "F5", 117: "F6", 118: "F7", 119: "F8", 120: "F9", 121: "F10", 122: "F11", 123: "F12", 144: "NUMLOCK", 145: "SCROLLLOCK", 186: ";", 187: "=", 188: ",", 189: "-", 190: ".", 191: "/", 192: "`", 219: "[", 220: "\\", 221: "]", 222: "'" }, isControl: function (e) {
            var k = e.which; switch (k) { case KEY.COMMAND: case KEY.SHIFT: case KEY.CTRL: case KEY.ALT: return !0 }
            if (e.metaKey || e.ctrlKey || e.altKey) return !0; return !1
        }, isFunctionKey: function (k) { k = k.which ? k.which : k; return k >= 112 && k <= 123 }, isVerticalMovement: function (k) { return ~[KEY.UP, KEY.DOWN].indexOf(k) }, isHorizontalMovement: function (k) { return ~[KEY.LEFT, KEY.RIGHT, KEY.BACKSPACE, KEY.DELETE].indexOf(k) }, toSeparator: function (k) { var sep = { ENTER: "\n", TAB: "\t", SPACE: " " }[k]; if (sep) return sep; return KEY[k] ? undefined : k }
    }; function isNil(value) { return angular.isUndefined(value) || value === null }
    if (angular.element.prototype.querySelectorAll === undefined) { angular.element.prototype.querySelectorAll = function (selector) { return angular.element(this[0].querySelectorAll(selector)) } }
    if (angular.element.prototype.closest === undefined) {
        angular.element.prototype.closest = function (selector) {
            var elem = this[0]; var matchesSelector = elem.matches || elem.webkitMatchesSelector || elem.mozMatchesSelector || elem.msMatchesSelector; while (elem) { if (matchesSelector.bind(elem)(selector)) { return elem } else { elem = elem.parentElement } }
            return !1
        }
    }
    var latestId = 0; var uis = angular.module('ui.select', []).constant('uiSelectConfig', { theme: 'bootstrap', searchEnabled: !0, sortable: !1, placeholder: '', refreshDelay: 1000, closeOnSelect: !0, skipFocusser: !1, dropdownPosition: 'auto', removeSelected: !0, resetSearchInput: !0, generateId: function () { return latestId++ }, appendToBody: !1, spinnerEnabled: !1, spinnerClass: 'glyphicon glyphicon-refresh ui-select-spin', backspaceReset: !0 }).service('uiSelectMinErr', function () { var minErr = angular.$$minErr('ui.select'); return function () { var error = minErr.apply(this, arguments); var message = error.message.replace(new RegExp('\nhttp://errors.angularjs.org/.*'), ''); return new Error(message) } }).directive('uisTranscludeAppend', function () { return { link: function (scope, element, attrs, ctrl, transclude) { transclude(scope, function (clone) { element.append(clone) }) } } }).filter('highlight', function () {
        function escapeRegexp(queryToEscape) { return ('' + queryToEscape).replace(/([.?*+^$[\]\\(){}|-])/g, '\\$1') }
        return function (matchItem, query) { return query && matchItem ? ('' + matchItem).replace(new RegExp(escapeRegexp(query), 'gi'), '<span class="ui-select-highlight">$&</span>') : matchItem }
    }).factory('uisOffset', ['$document', '$window', function ($document, $window) { return function (element) { var boundingClientRect = element[0].getBoundingClientRect(); return { width: boundingClientRect.width || element.prop('offsetWidth'), height: boundingClientRect.height || element.prop('offsetHeight'), top: boundingClientRect.top + ($window.pageYOffset || $document[0].documentElement.scrollTop), left: boundingClientRect.left + ($window.pageXOffset || $document[0].documentElement.scrollLeft) } } }]); uis.directive('uiSelectChoices', ['uiSelectConfig', 'uisRepeatParser', 'uiSelectMinErr', '$compile', '$window', function (uiSelectConfig, RepeatParser, uiSelectMinErr, $compile, $window) {
        return {
            restrict: 'EA', require: '^uiSelect', replace: !0, transclude: !0, templateUrl: function (tElement) { tElement.addClass('ui-select-choices'); var theme = tElement.parent().attr('theme') || uiSelectConfig.theme; return theme + '/choices.tpl.html' }, compile: function (tElement, tAttrs) {
                if (!tAttrs.repeat) throw uiSelectMinErr('repeat', "Expected 'repeat' expression."); var groupByExp = tAttrs.groupBy; var groupFilterExp = tAttrs.groupFilter; if (groupByExp) { var groups = tElement.querySelectorAll('.ui-select-choices-group'); if (groups.length !== 1) throw uiSelectMinErr('rows', "Expected 1 .ui-select-choices-group but got '{0}'.", groups.length); groups.attr('ng-repeat', RepeatParser.getGroupNgRepeatExpression()) }
                var parserResult = RepeatParser.parse(tAttrs.repeat); var choices = tElement.querySelectorAll('.ui-select-choices-row'); if (choices.length !== 1) { throw uiSelectMinErr('rows', "Expected 1 .ui-select-choices-row but got '{0}'.", choices.length) }
                choices.attr('ng-repeat', parserResult.repeatExpression(groupByExp)).attr('ng-if', '$select.open'); var rowsInner = tElement.querySelectorAll('.ui-select-choices-row-inner'); if (rowsInner.length !== 1) { throw uiSelectMinErr('rows', "Expected 1 .ui-select-choices-row-inner but got '{0}'.", rowsInner.length) }
                rowsInner.attr('uis-transclude-append', ''); var clickTarget = $window.document.addEventListener ? choices : rowsInner; clickTarget.attr('ng-click', '$select.select(' + parserResult.itemName + ',$select.skipFocusser,$event)'); return function link(scope, element, attrs, $select) { $select.parseRepeatAttr(attrs.repeat, groupByExp, groupFilterExp); $select.disableChoiceExpression = attrs.uiDisableChoice; $select.onHighlightCallback = attrs.onHighlight; $select.minimumInputLength = parseInt(attrs.minimumInputLength) || 0; $select.dropdownPosition = attrs.position ? attrs.position.toLowerCase() : uiSelectConfig.dropdownPosition; scope.$watch('$select.search', function (newValue) { if (newValue && !$select.open && $select.multiple) $select.activate(!1, !0); $select.activeIndex = $select.tagging.isActivated ? -1 : 0; if (!attrs.minimumInputLength || $select.search.length >= attrs.minimumInputLength) { $select.refresh(attrs.refresh) } else { $select.items = [] } }); attrs.$observe('refreshDelay', function () { var refreshDelay = scope.$eval(attrs.refreshDelay); $select.refreshDelay = refreshDelay !== undefined ? refreshDelay : uiSelectConfig.refreshDelay }); scope.$watch('$select.open', function (open) { if (open) { tElement.attr('role', 'listbox'); $select.refresh(attrs.refresh) } else { element.removeAttr('role') } }) }
            }
        }
    }]); uis.controller('uiSelectCtrl', ['$scope', '$element', '$timeout', '$filter', '$$uisDebounce', 'uisRepeatParser', 'uiSelectMinErr', 'uiSelectConfig', '$parse', '$injector', '$window', function ($scope, $element, $timeout, $filter, $$uisDebounce, RepeatParser, uiSelectMinErr, uiSelectConfig, $parse, $injector, $window) {
        var ctrl = this; var EMPTY_SEARCH = ''; ctrl.placeholder = uiSelectConfig.placeholder; ctrl.searchEnabled = uiSelectConfig.searchEnabled; ctrl.sortable = uiSelectConfig.sortable; ctrl.refreshDelay = uiSelectConfig.refreshDelay; ctrl.paste = uiSelectConfig.paste; ctrl.resetSearchInput = uiSelectConfig.resetSearchInput; ctrl.refreshing = !1; ctrl.spinnerEnabled = uiSelectConfig.spinnerEnabled; ctrl.spinnerClass = uiSelectConfig.spinnerClass; ctrl.removeSelected = uiSelectConfig.removeSelected; ctrl.closeOnSelect = !0; ctrl.skipFocusser = !1; ctrl.search = EMPTY_SEARCH; ctrl.activeIndex = 0; ctrl.items = []; ctrl.open = !1; ctrl.focus = !1; ctrl.disabled = !1; ctrl.selected = undefined; ctrl.dropdownPosition = 'auto'; ctrl.focusser = undefined; ctrl.multiple = undefined; ctrl.disableChoiceExpression = undefined; ctrl.tagging = { isActivated: !1, fct: undefined }; ctrl.taggingTokens = { isActivated: !1, tokens: undefined }; ctrl.lockChoiceExpression = undefined; ctrl.clickTriggeredSelect = !1; ctrl.$filter = $filter; ctrl.$element = $element; ctrl.$animate = (function () { try { return $injector.get('$animate') } catch (err) { return null } })(); ctrl.searchInput = $element.querySelectorAll('input.ui-select-search'); if (ctrl.searchInput.length !== 1) { throw uiSelectMinErr('searchInput', "Expected 1 input.ui-select-search but got '{0}'.", ctrl.searchInput.length) }
        ctrl.isEmpty = function () { return isNil(ctrl.selected) || ctrl.selected === '' || (ctrl.multiple && ctrl.selected.length === 0) }; function _findIndex(collection, predicate, thisArg) {
            if (collection.findIndex) { return collection.findIndex(predicate, thisArg) } else {
                var list = Object(collection); var length = list.length >>> 0; var value; for (var i = 0; i < length; i++) { value = list[i]; if (predicate.call(thisArg, value, i, list)) { return i } }
                return -1
            }
        }
        function _resetSearchInput() { if (ctrl.resetSearchInput) { ctrl.search = EMPTY_SEARCH; if (ctrl.selected && ctrl.items.length && !ctrl.multiple) { ctrl.activeIndex = _findIndex(ctrl.items, function (item) { return angular.equals(this, item) }, ctrl.selected) } } }
        function _groupsFilter(groups, groupNames) {
            var i, j, result = []; for (i = 0; i < groupNames.length; i++) { for (j = 0; j < groups.length; j++) { if (groups[j].name == [groupNames[i]]) { result.push(groups[j]) } } }
            return result
        }
        ctrl.activate = function (initSearchValue, avoidReset) {
            if (!ctrl.disabled && !ctrl.open) {
                if (!avoidReset) _resetSearchInput(); $scope.$broadcast('uis:activate'); ctrl.open = !0; ctrl.activeIndex = ctrl.activeIndex >= ctrl.items.length ? 0 : ctrl.activeIndex; if (ctrl.activeIndex === -1 && ctrl.taggingLabel !== !1) { ctrl.activeIndex = 0 }
                var container = $element.querySelectorAll('.ui-select-choices-content'); var searchInput = $element.querySelectorAll('.ui-select-search'); if (ctrl.$animate && ctrl.$animate.on && ctrl.$animate.enabled(container[0])) { var animateHandler = function (elem, phase) { if (phase === 'start' && ctrl.items.length === 0) { ctrl.$animate.off('removeClass', searchInput[0], animateHandler); $timeout(function () { ctrl.focusSearchInput(initSearchValue) }) } else if (phase === 'close') { ctrl.$animate.off('enter', container[0], animateHandler); $timeout(function () { ctrl.focusSearchInput(initSearchValue) }) } }; if (ctrl.items.length > 0) { ctrl.$animate.on('enter', container[0], animateHandler) } else { ctrl.$animate.on('removeClass', searchInput[0], animateHandler) } } else { $timeout(function () { ctrl.focusSearchInput(initSearchValue); if (!ctrl.tagging.isActivated && ctrl.items.length > 1) { _ensureHighlightVisible() } }) }
            } else if (ctrl.open && !ctrl.searchEnabled) { ctrl.close() }
        }; ctrl.focusSearchInput = function (initSearchValue) { ctrl.search = initSearchValue || ctrl.search; ctrl.searchInput[0].focus() }; ctrl.findGroupByName = function (name) { return ctrl.groups && ctrl.groups.filter(function (group) { return group.name === name })[0] }; ctrl.parseRepeatAttr = function (repeatAttr, groupByExp, groupFilterExp) {
            function updateGroups(items) {
                var groupFn = $scope.$eval(groupByExp); ctrl.groups = []; angular.forEach(items, function (item) { var groupName = angular.isFunction(groupFn) ? groupFn(item) : item[groupFn]; var group = ctrl.findGroupByName(groupName); if (group) { group.items.push(item) } else { ctrl.groups.push({ name: groupName, items: [item] }) } }); if (groupFilterExp) { var groupFilterFn = $scope.$eval(groupFilterExp); if (angular.isFunction(groupFilterFn)) { ctrl.groups = groupFilterFn(ctrl.groups) } else if (angular.isArray(groupFilterFn)) { ctrl.groups = _groupsFilter(ctrl.groups, groupFilterFn) } }
                ctrl.items = []; ctrl.groups.forEach(function (group) { ctrl.items = ctrl.items.concat(group.items) })
            }
            function setPlainItems(items) { ctrl.items = items || [] }
            ctrl.setItemsFn = groupByExp ? updateGroups : setPlainItems; ctrl.parserResult = RepeatParser.parse(repeatAttr); ctrl.isGrouped = !!groupByExp; ctrl.itemProperty = ctrl.parserResult.itemName; var originalSource = ctrl.parserResult.source; var createArrayFromObject = function () { var origSrc = originalSource($scope); $scope.$uisSource = Object.keys(origSrc).map(function (v) { var result = {}; result[ctrl.parserResult.keyName] = v; result.value = origSrc[v]; return result }) }; if (ctrl.parserResult.keyName) { createArrayFromObject(); ctrl.parserResult.source = $parse('$uisSource' + ctrl.parserResult.filters); $scope.$watch(originalSource, function (newVal, oldVal) { if (newVal !== oldVal) createArrayFromObject() }, !0) }
            ctrl.refreshItems = function (data) {
                data = data || ctrl.parserResult.source($scope); var selectedItems = ctrl.selected; if (ctrl.isEmpty() || (angular.isArray(selectedItems) && !selectedItems.length) || !ctrl.multiple || !ctrl.removeSelected) { ctrl.setItemsFn(data) } else { if (data !== undefined && data !== null) { var filteredItems = data.filter(function (i) { return angular.isArray(selectedItems) ? selectedItems.every(function (selectedItem) { return !angular.equals(i, selectedItem) }) : !angular.equals(i, selectedItems) }); ctrl.setItemsFn(filteredItems) } }
                if (ctrl.dropdownPosition === 'auto' || ctrl.dropdownPosition === 'up') { $scope.calculateDropdownPos() }
                $scope.$broadcast('uis:refresh')
            }; $scope.$watchCollection(ctrl.parserResult.source, function (items) { if (items === undefined || items === null) { ctrl.items = [] } else { if (!angular.isArray(items)) { throw uiSelectMinErr('items', "Expected an array but got '{0}'.", items) } else { ctrl.refreshItems(items); if (angular.isDefined(ctrl.ngModel.$modelValue)) { ctrl.ngModel.$modelValue = null } } } })
        }; var _refreshDelayPromise; ctrl.refresh = function (refreshAttr) {
            if (refreshAttr !== undefined) {
                if (_refreshDelayPromise) { $timeout.cancel(_refreshDelayPromise) }
                _refreshDelayPromise = $timeout(function () { if ($scope.$select.search.length >= $scope.$select.minimumInputLength) { var refreshPromise = $scope.$eval(refreshAttr); if (refreshPromise && angular.isFunction(refreshPromise.then) && !ctrl.refreshing) { ctrl.refreshing = !0; refreshPromise.finally(function () { ctrl.refreshing = !1 }) } } }, ctrl.refreshDelay)
            }
        }; ctrl.isActive = function (itemScope) {
            if (!ctrl.open) { return !1 }
            var itemIndex = ctrl.items.indexOf(itemScope[ctrl.itemProperty]); var isActive = itemIndex == ctrl.activeIndex; if (!isActive || itemIndex < 0) { return !1 }
            if (isActive && !angular.isUndefined(ctrl.onHighlightCallback)) { itemScope.$eval(ctrl.onHighlightCallback) }
            return isActive
        }; var _isItemSelected = function (item) { return (ctrl.selected && angular.isArray(ctrl.selected) && ctrl.selected.filter(function (selection) { return angular.equals(selection, item) }).length > 0) }; var disabledItems = []; function _updateItemDisabled(item, isDisabled) {
            var disabledItemIndex = disabledItems.indexOf(item); if (isDisabled && disabledItemIndex === -1) { disabledItems.push(item) }
            if (!isDisabled && disabledItemIndex > -1) { disabledItems.splice(disabledItemIndex, 1) }
        }
        function _isItemDisabled(item) { return disabledItems.indexOf(item) > -1 }
        ctrl.isDisabled = function (itemScope) {
            if (!ctrl.open) return; var item = itemScope[ctrl.itemProperty]; var itemIndex = ctrl.items.indexOf(item); var isDisabled = !1; if (itemIndex >= 0 && (angular.isDefined(ctrl.disableChoiceExpression) || ctrl.multiple)) {
                if (item.isTag) return !1; if (ctrl.multiple) { isDisabled = _isItemSelected(item) }
                if (!isDisabled && angular.isDefined(ctrl.disableChoiceExpression)) { isDisabled = !!(itemScope.$eval(ctrl.disableChoiceExpression)) }
                _updateItemDisabled(item, isDisabled)
            }
            return isDisabled
        }; ctrl.select = function (item, skipFocusser, $event) {
            if (isNil(item) || !_isItemDisabled(item)) {
                if (!ctrl.items && !ctrl.search && !ctrl.tagging.isActivated) return; if (!item || !_isItemDisabled(item)) {
                ctrl.clickTriggeredSelect = !1; if ($event && ($event.type === 'click' || $event.type === 'touchend') && item)
                    ctrl.clickTriggeredSelect = !0; if (ctrl.tagging.isActivated && ctrl.clickTriggeredSelect === !1) {
                        if (ctrl.taggingLabel === !1) {
                            if (ctrl.activeIndex < 0) {
                                if (item === undefined) { item = ctrl.tagging.fct !== undefined ? ctrl.tagging.fct(ctrl.search) : ctrl.search }
                                if (!item || angular.equals(ctrl.items[0], item)) { return }
                            } else { item = ctrl.items[ctrl.activeIndex] }
                        } else { if (ctrl.activeIndex === 0) { if (item === undefined) return; if (ctrl.tagging.fct !== undefined && typeof item === 'string') { item = ctrl.tagging.fct(item); if (!item) return } else if (typeof item === 'string') { item = item.replace(ctrl.taggingLabel, '').trim() } } }
                        if (_isItemSelected(item)) { ctrl.close(skipFocusser); return }
                    }
                    _resetSearchInput(); $scope.$broadcast('uis:select', item); if (ctrl.closeOnSelect) { ctrl.close(skipFocusser) }
                }
            }
        }; ctrl.close = function (skipFocusser) { if (!ctrl.open) return; if (ctrl.ngModel && ctrl.ngModel.$setTouched) ctrl.ngModel.$setTouched(); ctrl.open = !1; _resetSearchInput(); $scope.$broadcast('uis:close', skipFocusser) }; ctrl.setFocus = function () { if (!ctrl.focus) ctrl.focusInput[0].focus() }; ctrl.clear = function ($event) { ctrl.select(null); $event.stopPropagation(); $timeout(function () { ctrl.focusser[0].focus() }, 0, !1) }; ctrl.toggle = function (e) { if (ctrl.open) { ctrl.close(); e.preventDefault(); e.stopPropagation() } else { ctrl.activate() } }; ctrl.isLocked = function () { return !1 }; $scope.$watch(function () { return angular.isDefined(ctrl.lockChoiceExpression) && ctrl.lockChoiceExpression !== "" }, _initaliseLockedChoices); function _initaliseLockedChoices(doInitalise) {
            if (!doInitalise) return; var lockedItems = []; function _updateItemLocked(item, isLocked) {
                var lockedItemIndex = lockedItems.indexOf(item); if (isLocked && lockedItemIndex === -1) { lockedItems.push(item) }
                if (!isLocked && lockedItemIndex > -1) { lockedItems.splice(lockedItemIndex, 1) }
            }
            function _isItemlocked(item) { return lockedItems.indexOf(item) > -1 }
            ctrl.isLocked = function (itemScope, itemIndex) {
                var isLocked = !1, item = ctrl.selected[itemIndex]; if (item) { if (itemScope) { isLocked = !!(itemScope.$eval(ctrl.lockChoiceExpression)); _updateItemLocked(item, isLocked) } else { isLocked = _isItemlocked(item) } }
                return isLocked
            }
        }
        var sizeWatch = null; var updaterScheduled = !1; ctrl.sizeSearchInput = function () {
            var input = ctrl.searchInput[0], container = ctrl.$element[0], calculateContainerWidth = function () { return container.clientWidth * !!input.offsetParent }, updateIfVisible = function (containerWidth) {
                if (containerWidth === 0) { return !1 }
                var inputWidth = containerWidth - input.offsetLeft; if (inputWidth < 50) inputWidth = containerWidth; ctrl.searchInput.css('width', inputWidth + 'px'); return !0
            }; ctrl.searchInput.css('width', '10px'); $timeout(function () { if (sizeWatch === null && !updateIfVisible(calculateContainerWidth())) { sizeWatch = $scope.$watch(function () { if (!updaterScheduled) { updaterScheduled = !0; $scope.$$postDigest(function () { updaterScheduled = !1; if (updateIfVisible(calculateContainerWidth())) { sizeWatch(); sizeWatch = null } }) } }, angular.noop) } })
        }; function _handleDropDownSelection(key) {
            var processed = !0; switch (key) {
                case KEY.DOWN: if (!ctrl.open && ctrl.multiple) ctrl.activate(!1, !0); else if (ctrl.activeIndex < ctrl.items.length - 1) { var idx = ++ctrl.activeIndex; while (_isItemDisabled(ctrl.items[idx]) && idx < ctrl.items.length) { ctrl.activeIndex = ++idx } }
                    break; case KEY.UP: var minActiveIndex = (ctrl.search.length === 0 && ctrl.tagging.isActivated) ? -1 : 0; if (!ctrl.open && ctrl.multiple) ctrl.activate(!1, !0); else if (ctrl.activeIndex > minActiveIndex) { var idxmin = --ctrl.activeIndex; while (_isItemDisabled(ctrl.items[idxmin]) && idxmin > minActiveIndex) { ctrl.activeIndex = --idxmin } }
                    break; case KEY.TAB: if (!ctrl.multiple || ctrl.open) ctrl.select(ctrl.items[ctrl.activeIndex], !0); break; case KEY.ENTER: if (ctrl.open && (ctrl.tagging.isActivated || ctrl.activeIndex >= 0)) { ctrl.select(ctrl.items[ctrl.activeIndex], ctrl.skipFocusser) } else { ctrl.activate(!1, !0) }
                    break; case KEY.ESC: ctrl.close(); break; default: processed = !1
            }
            return processed
        }
        ctrl.searchInput.on('keydown', function (e) {
            var key = e.which; if (~[KEY.ENTER, KEY.ESC].indexOf(key)) { e.preventDefault(); e.stopPropagation() }
            $scope.$apply(function () {
                var tagged = !1; if (ctrl.items.length > 0 || ctrl.tagging.isActivated) {
                    if (!_handleDropDownSelection(key) && !ctrl.searchEnabled) { e.preventDefault(); e.stopPropagation() }
                    if (ctrl.taggingTokens.isActivated) {
                        for (var i = 0; i < ctrl.taggingTokens.tokens.length; i++) { if (ctrl.taggingTokens.tokens[i] === KEY.MAP[e.keyCode]) { if (ctrl.search.length > 0) { tagged = !0 } } }
                        if (tagged) {
                            $timeout(function () {
                                ctrl.searchInput.triggerHandler('tagged'); var newItem = ctrl.search.replace(KEY.MAP[e.keyCode], '').trim(); if (ctrl.tagging.fct) { newItem = ctrl.tagging.fct(newItem) }
                                if (newItem) ctrl.select(newItem, !0)
                            })
                        }
                    }
                }
            }); if (KEY.isVerticalMovement(key) && ctrl.items.length > 0) { _ensureHighlightVisible() }
            if (key === KEY.ENTER || key === KEY.ESC) { e.preventDefault(); e.stopPropagation() }
        }); ctrl.searchInput.on('paste', function (e) {
            var data; if (window.clipboardData && window.clipboardData.getData) { data = window.clipboardData.getData('Text') } else { data = (e.originalEvent || e).clipboardData.getData('text/plain') }
            data = ctrl.search + data; if (data && data.length > 0) {
                if (ctrl.taggingTokens.isActivated) {
                    var items = []; for (var i = 0; i < ctrl.taggingTokens.tokens.length; i++) { var separator = KEY.toSeparator(ctrl.taggingTokens.tokens[i]) || ctrl.taggingTokens.tokens[i]; if (data.indexOf(separator) > -1) { items = data.split(separator); break } }
                    if (items.length === 0) { items = [data] }
                    var oldsearch = ctrl.search; angular.forEach(items, function (item) { var newItem = ctrl.tagging.fct ? ctrl.tagging.fct(item) : item; if (newItem) { ctrl.select(newItem, !0) } }); ctrl.search = oldsearch || EMPTY_SEARCH; e.preventDefault(); e.stopPropagation()
                } else if (ctrl.paste) { ctrl.paste(data); ctrl.search = EMPTY_SEARCH; e.preventDefault(); e.stopPropagation() }
            }
        }); ctrl.searchInput.on('tagged', function () { $timeout(function () { _resetSearchInput() }) }); function _ensureHighlightVisible() {
            var container = $element.querySelectorAll('.ui-select-choices-content'); var choices = container.querySelectorAll('.ui-select-choices-row'); if (choices.length < 1) { throw uiSelectMinErr('choices', "Expected multiple .ui-select-choices-row but got '{0}'.", choices.length) }
            if (ctrl.activeIndex < 0) { return }
            var highlighted = choices[ctrl.activeIndex]; var posY = highlighted.offsetTop + highlighted.clientHeight - container[0].scrollTop; var height = container[0].offsetHeight; if (posY > height) { container[0].scrollTop += posY - height } else if (posY < highlighted.clientHeight) {
                if (ctrl.isGrouped && ctrl.activeIndex === 0)
                    container[0].scrollTop = 0; else container[0].scrollTop -= highlighted.clientHeight - posY
            }
        }
        var onResize = $$uisDebounce(function () { ctrl.sizeSearchInput() }, 50); angular.element($window).bind('resize', onResize); $scope.$on('$destroy', function () { ctrl.searchInput.off('keyup keydown tagged blur paste'); angular.element($window).off('resize', onResize) }); $scope.$watch('$select.activeIndex', function (activeIndex) {
            if (activeIndex)
                $element.find('input').attr('aria-activedescendant', 'ui-select-choices-row-' + ctrl.generatedId + '-' + activeIndex)
        }); $scope.$watch('$select.open', function (open) {
            if (!open)
                $element.find('input').removeAttr('aria-activedescendant')
        })
    }]); uis.directive('uiSelect', ['$document', 'uiSelectConfig', 'uiSelectMinErr', 'uisOffset', '$compile', '$parse', '$timeout', function ($document, uiSelectConfig, uiSelectMinErr, uisOffset, $compile, $parse, $timeout) {
        return {
            restrict: 'EA', templateUrl: function (tElement, tAttrs) { var theme = tAttrs.theme || uiSelectConfig.theme; return theme + (angular.isDefined(tAttrs.multiple) ? '/select-multiple.tpl.html' : '/select.tpl.html') }, replace: !0, transclude: !0, require: ['uiSelect', '^ngModel'], scope: !0, controller: 'uiSelectCtrl', controllerAs: '$select', compile: function (tElement, tAttrs) {
                var match = /{(.*)}\s*{(.*)}/.exec(tAttrs.ngClass); if (match) { var combined = '{' + match[1] + ', ' + match[2] + '}'; tAttrs.ngClass = combined; tElement.attr('ng-class', combined) }
                if (angular.isDefined(tAttrs.multiple))
                    tElement.append('<ui-select-multiple/>').removeAttr('multiple'); else tElement.append('<ui-select-single/>'); if (tAttrs.inputId)
                    tElement.querySelectorAll('input.ui-select-search')[0].id = tAttrs.inputId; return function (scope, element, attrs, ctrls, transcludeFn) {
                        var $select = ctrls[0]; var ngModel = ctrls[1]; $select.generatedId = uiSelectConfig.generateId(); $select.baseTitle = attrs.title || 'Select box'; $select.focusserTitle = $select.baseTitle + ' focus'; $select.focusserId = 'focusser-' + $select.generatedId; $select.closeOnSelect = function () { if (angular.isDefined(attrs.closeOnSelect)) { return $parse(attrs.closeOnSelect)() } else { return uiSelectConfig.closeOnSelect } }(); scope.$watch('skipFocusser', function () { var skipFocusser = scope.$eval(attrs.skipFocusser); $select.skipFocusser = skipFocusser !== undefined ? skipFocusser : uiSelectConfig.skipFocusser }); $select.onSelectCallback = $parse(attrs.onSelect); $select.onRemoveCallback = $parse(attrs.onRemove); $select.ngModel = ngModel; $select.choiceGrouped = function (group) { return $select.isGrouped && group && group.name }; if (attrs.tabindex) { attrs.$observe('tabindex', function (value) { $select.focusInput.attr('tabindex', value); element.removeAttr('tabindex') }) }
                        scope.$watch(function () { return scope.$eval(attrs.searchEnabled) }, function (newVal) { $select.searchEnabled = newVal !== undefined ? newVal : uiSelectConfig.searchEnabled }); scope.$watch('sortable', function () { var sortable = scope.$eval(attrs.sortable); $select.sortable = sortable !== undefined ? sortable : uiSelectConfig.sortable }); attrs.$observe('backspaceReset', function () { var backspaceReset = scope.$eval(attrs.backspaceReset); $select.backspaceReset = backspaceReset !== undefined ? backspaceReset : !0 }); attrs.$observe('limit', function () { $select.limit = (angular.isDefined(attrs.limit)) ? parseInt(attrs.limit, 10) : undefined }); scope.$watch('removeSelected', function () { var removeSelected = scope.$eval(attrs.removeSelected); $select.removeSelected = removeSelected !== undefined ? removeSelected : uiSelectConfig.removeSelected }); attrs.$observe('disabled', function () { $select.disabled = attrs.disabled !== undefined ? attrs.disabled : !1 }); attrs.$observe('resetSearchInput', function () { var resetSearchInput = scope.$eval(attrs.resetSearchInput); $select.resetSearchInput = resetSearchInput !== undefined ? resetSearchInput : !0 }); attrs.$observe('paste', function () { $select.paste = scope.$eval(attrs.paste) }); attrs.$observe('tagging', function () { if (attrs.tagging !== undefined) { var taggingEval = scope.$eval(attrs.tagging); $select.tagging = { isActivated: !0, fct: taggingEval !== !0 ? taggingEval : undefined } } else { $select.tagging = { isActivated: !1, fct: undefined } } }); attrs.$observe('taggingLabel', function () { if (attrs.tagging !== undefined) { if (attrs.taggingLabel === 'false') { $select.taggingLabel = !1 } else { $select.taggingLabel = attrs.taggingLabel !== undefined ? attrs.taggingLabel : '(new)' } } }); attrs.$observe('taggingTokens', function () { if (attrs.tagging !== undefined) { var tokens = attrs.taggingTokens !== undefined ? attrs.taggingTokens.split('|') : [',', 'ENTER']; $select.taggingTokens = { isActivated: !0, tokens: tokens } } }); attrs.$observe('spinnerEnabled', function () { var spinnerEnabled = scope.$eval(attrs.spinnerEnabled); $select.spinnerEnabled = spinnerEnabled !== undefined ? spinnerEnabled : uiSelectConfig.spinnerEnabled }); attrs.$observe('spinnerClass', function () { var spinnerClass = attrs.spinnerClass; $select.spinnerClass = spinnerClass !== undefined ? attrs.spinnerClass : uiSelectConfig.spinnerClass }); if (angular.isDefined(attrs.autofocus)) { $timeout(function () { $select.setFocus() }) }
                        if (angular.isDefined(attrs.focusOn)) { scope.$on(attrs.focusOn, function () { $timeout(function () { $select.setFocus() }) }) }
                        function onDocumentClick(e) {
                            if (!$select.open) return; var contains = !1; if (window.jQuery) { contains = window.jQuery.contains(element[0], e.target) } else { contains = element[0].contains(e.target) }
                            if (!contains && !$select.clickTriggeredSelect) {
                                var skipFocusser; if (!$select.skipFocusser) { var focusableControls = ['input', 'button', 'textarea', 'select']; var targetController = angular.element(e.target).controller('uiSelect'); skipFocusser = targetController && targetController !== $select; if (!skipFocusser) skipFocusser = ~focusableControls.indexOf(e.target.tagName.toLowerCase()) } else { skipFocusser = !0 }
                                $select.close(skipFocusser); scope.$digest()
                            }
                            $select.clickTriggeredSelect = !1
                        }
                        $document.on('click', onDocumentClick); scope.$on('$destroy', function () { $document.off('click', onDocumentClick) }); transcludeFn(scope, function (clone) {
                            var transcluded = angular.element('<div>').append(clone); var transcludedMatch = transcluded.querySelectorAll('.ui-select-match'); transcludedMatch.removeAttr('ui-select-match'); transcludedMatch.removeAttr('data-ui-select-match'); if (transcludedMatch.length !== 1) { throw uiSelectMinErr('transcluded', "Expected 1 .ui-select-match but got '{0}'.", transcludedMatch.length) }
                            element.querySelectorAll('.ui-select-match').replaceWith(transcludedMatch); var transcludedChoices = transcluded.querySelectorAll('.ui-select-choices'); transcludedChoices.removeAttr('ui-select-choices'); transcludedChoices.removeAttr('data-ui-select-choices'); if (transcludedChoices.length !== 1) { throw uiSelectMinErr('transcluded', "Expected 1 .ui-select-choices but got '{0}'.", transcludedChoices.length) }
                            element.querySelectorAll('.ui-select-choices').replaceWith(transcludedChoices); var transcludedNoChoice = transcluded.querySelectorAll('.ui-select-no-choice'); transcludedNoChoice.removeAttr('ui-select-no-choice'); transcludedNoChoice.removeAttr('data-ui-select-no-choice'); if (transcludedNoChoice.length == 1) { element.querySelectorAll('.ui-select-no-choice').replaceWith(transcludedNoChoice) }
                        }); var appendToBody = scope.$eval(attrs.appendToBody); if (appendToBody !== undefined ? appendToBody : uiSelectConfig.appendToBody) { scope.$watch('$select.open', function (isOpen) { if (isOpen) { positionDropdown() } else { resetDropdown() } }); scope.$on('$destroy', function () { resetDropdown() }) }
                        var placeholder = null, originalWidth = ''; function positionDropdown() { var offset = uisOffset(element); placeholder = angular.element('<div class="ui-select-placeholder"></div>'); placeholder[0].style.width = offset.width + 'px'; placeholder[0].style.height = offset.height + 'px'; element.after(placeholder); originalWidth = element[0].style.width; $document.find('body').append(element); element[0].style.position = 'absolute'; element[0].style.left = offset.left + 'px'; element[0].style.top = offset.top + 'px'; element[0].style.width = offset.width + 'px' }
                        function resetDropdown() {
                            if (placeholder === null) { return }
                            placeholder.replaceWith(element); placeholder = null; element[0].style.position = ''; element[0].style.left = ''; element[0].style.top = ''; element[0].style.width = originalWidth; $select.setFocus()
                        }
                        var dropdown = null, directionUpClassName = 'direction-up'; scope.$watch('$select.open', function () { if ($select.dropdownPosition === 'auto' || $select.dropdownPosition === 'up') { scope.calculateDropdownPos() } }); var setDropdownPosUp = function (offset, offsetDropdown) { offset = offset || uisOffset(element); offsetDropdown = offsetDropdown || uisOffset(dropdown); dropdown[0].style.position = 'absolute'; dropdown[0].style.top = (offsetDropdown.height * -1) + 'px'; element.addClass(directionUpClassName) }; var setDropdownPosDown = function (offset, offsetDropdown) { element.removeClass(directionUpClassName); offset = offset || uisOffset(element); offsetDropdown = offsetDropdown || uisOffset(dropdown); dropdown[0].style.position = ''; dropdown[0].style.top = '' }; var calculateDropdownPosAfterAnimation = function () {
                            $timeout(function () {
                                if ($select.dropdownPosition === 'up') { setDropdownPosUp() } else { element.removeClass(directionUpClassName); var offset = uisOffset(element); var offsetDropdown = uisOffset(dropdown); var scrollTop = $document[0].documentElement.scrollTop || $document[0].body.scrollTop; if (offset.top + offset.height + offsetDropdown.height > scrollTop + $document[0].documentElement.clientHeight) { setDropdownPosUp(offset, offsetDropdown) } else { setDropdownPosDown(offset, offsetDropdown) } }
                                dropdown[0].style.opacity = 1
                            })
                        }; var opened = !1; scope.calculateDropdownPos = function () {
                            if ($select.open) {
                                dropdown = angular.element(element).querySelectorAll('.ui-select-dropdown'); if (dropdown.length === 0) { return }
                                if ($select.search === '' && !opened) { dropdown[0].style.opacity = 0; opened = !0 }
                                if (!uisOffset(dropdown).height && $select.$animate && $select.$animate.on && $select.$animate.enabled(dropdown)) { var needsCalculated = !0; $select.$animate.on('enter', dropdown, function (elem, phase) { if (phase === 'close' && needsCalculated) { calculateDropdownPosAfterAnimation(); needsCalculated = !1 } }) } else { calculateDropdownPosAfterAnimation() }
                            } else {
                                if (dropdown === null || dropdown.length === 0) { return }
                                dropdown[0].style.opacity = 0; dropdown[0].style.position = ''; dropdown[0].style.top = ''; element.removeClass(directionUpClassName)
                            }
                        }
                    }
            }
        }
    }]); uis.directive('uiSelectMatch', ['uiSelectConfig', function (uiSelectConfig) {
        return {
            restrict: 'EA', require: '^uiSelect', replace: !0, transclude: !0, templateUrl: function (tElement) { tElement.addClass('ui-select-match'); var parent = tElement.parent(); var theme = getAttribute(parent, 'theme') || uiSelectConfig.theme; var multi = angular.isDefined(getAttribute(parent, 'multiple')); return theme + (multi ? '/match-multiple.tpl.html' : '/match.tpl.html') }, link: function (scope, element, attrs, $select) {
            $select.lockChoiceExpression = attrs.uiLockChoice; attrs.$observe('placeholder', function (placeholder) { $select.placeholder = placeholder !== undefined ? placeholder : uiSelectConfig.placeholder }); function setAllowClear(allow) { $select.allowClear = (angular.isDefined(allow)) ? (allow === '') ? !0 : (allow.toLowerCase() === 'true') : !1 }
                attrs.$observe('allowClear', setAllowClear); setAllowClear(attrs.allowClear); if ($select.multiple) { $select.sizeSearchInput() }
            }
        }; function getAttribute(elem, attribute) {
            if (elem[0].hasAttribute(attribute))
                return elem.attr(attribute); if (elem[0].hasAttribute('data-' + attribute))
                return elem.attr('data-' + attribute); if (elem[0].hasAttribute('x-' + attribute))
                return elem.attr('x-' + attribute)
        }
    }]); uis.directive('uiSelectMultiple', ['uiSelectMinErr', '$timeout', function (uiSelectMinErr, $timeout) {
        return {
            restrict: 'EA', require: ['^uiSelect', '^ngModel'], controller: ['$scope', '$timeout', function ($scope, $timeout) {
                var ctrl = this, $select = $scope.$select, ngModel; if (angular.isUndefined($select.selected))
                    $select.selected = []; $scope.$evalAsync(function () { ngModel = $scope.ngModel }); ctrl.activeMatchIndex = -1; ctrl.updateModel = function () { ngModel.$setViewValue(Date.now()); ctrl.refreshComponent() }; ctrl.refreshComponent = function () {
                        if ($select.refreshItems) { $select.refreshItems() }
                        if ($select.sizeSearchInput) { $select.sizeSearchInput() }
                    }; ctrl.removeChoice = function (index) { if ($select.isLocked(null, index)) return !1; var removedChoice = $select.selected[index]; var locals = {}; locals[$select.parserResult.itemName] = removedChoice; $select.selected.splice(index, 1); ctrl.activeMatchIndex = -1; $select.sizeSearchInput(); $timeout(function () { $select.onRemoveCallback($scope, { $item: removedChoice, $model: $select.parserResult.modelMapper($scope, locals) }) }); ctrl.updateModel(); return !0 }; ctrl.getPlaceholder = function () { if ($select.selected && $select.selected.length) return; return $select.placeholder }
            }], controllerAs: '$selectMultiple', link: function (scope, element, attrs, ctrls) {
                var $select = ctrls[0]; var ngModel = scope.ngModel = ctrls[1]; var $selectMultiple = scope.$selectMultiple; $select.multiple = !0; $select.focusInput = $select.searchInput; ngModel.$isEmpty = function (value) { return !value || value.length === 0 }; ngModel.$parsers.unshift(function () {
                    var locals = {}, result, resultMultiple = []; for (var j = $select.selected.length - 1; j >= 0; j--) { locals = {}; locals[$select.parserResult.itemName] = $select.selected[j]; result = $select.parserResult.modelMapper(scope, locals); resultMultiple.unshift(result) }
                    return resultMultiple
                }); ngModel.$formatters.unshift(function (inputValue) {
                    var data = $select.parserResult && $select.parserResult.source(scope, { $select: { search: '' } }), locals = {}, result; if (!data) return inputValue; var resultMultiple = []; var checkFnMultiple = function (list, value) {
                        if (!list || !list.length) return; for (var p = list.length - 1; p >= 0; p--) {
                        locals[$select.parserResult.itemName] = list[p]; result = $select.parserResult.modelMapper(scope, locals); if ($select.parserResult.trackByExp) { var propsItemNameMatches = /(\w*)\./.exec($select.parserResult.trackByExp); var matches = /\.([^\s]+)/.exec($select.parserResult.trackByExp); if (propsItemNameMatches && propsItemNameMatches.length > 0 && propsItemNameMatches[1] == $select.parserResult.itemName) { if (matches && matches.length > 0 && result[matches[1]] == value[matches[1]]) { resultMultiple.unshift(list[p]); return !0 } } }
                            if (angular.equals(result, value)) { resultMultiple.unshift(list[p]); return !0 }
                        }
                        return !1
                    }; if (!inputValue) return resultMultiple; for (var k = inputValue.length - 1; k >= 0; k--) { if (!checkFnMultiple($select.selected, inputValue[k])) { if (!checkFnMultiple(data, inputValue[k])) { resultMultiple.unshift(inputValue[k]) } } }
                    return resultMultiple
                }); scope.$watchCollection(function () { return ngModel.$modelValue }, function (newValue, oldValue) {
                    if (oldValue != newValue) {
                        if (angular.isDefined(ngModel.$modelValue)) { ngModel.$modelValue = null }
                        $selectMultiple.refreshComponent()
                    }
                }); ngModel.$render = function () {
                    if (!angular.isArray(ngModel.$viewValue)) { if (isNil(ngModel.$viewValue)) { ngModel.$viewValue = [] } else { throw uiSelectMinErr('multiarr', "Expected model value to be array but got '{0}'", ngModel.$viewValue) } }
                    $select.selected = ngModel.$viewValue; $selectMultiple.refreshComponent(); scope.$evalAsync()
                }; scope.$on('uis:select', function (event, item) {
                    if ($select.selected.length >= $select.limit) { return }
                    $select.selected.push(item); var locals = {}; locals[$select.parserResult.itemName] = item; $timeout(function () { $select.onSelectCallback(scope, { $item: item, $model: $select.parserResult.modelMapper(scope, locals) }) }); $selectMultiple.updateModel()
                }); scope.$on('uis:activate', function () { $selectMultiple.activeMatchIndex = -1 }); scope.$watch('$select.disabled', function (newValue, oldValue) { if (oldValue && !newValue) $select.sizeSearchInput() }); $select.searchInput.on('keydown', function (e) {
                    var key = e.which; scope.$apply(function () {
                        var processed = !1; if (KEY.isHorizontalMovement(key)) { processed = _handleMatchSelection(key) }
                        if (processed && key != KEY.TAB) { e.preventDefault(); e.stopPropagation() }
                    })
                }); function _getCaretPosition(el) { if (angular.isNumber(el.selectionStart)) return el.selectionStart; else return el.value.length }
                function _handleMatchSelection(key) {
                    var caretPosition = _getCaretPosition($select.searchInput[0]), length = $select.selected.length, first = 0, last = length - 1, curr = $selectMultiple.activeMatchIndex, next = $selectMultiple.activeMatchIndex + 1, prev = $selectMultiple.activeMatchIndex - 1, newIndex = curr; if (caretPosition > 0 || ($select.search.length && key == KEY.RIGHT)) return !1; $select.close(); function getNewActiveMatchIndex() {
                        switch (key) {
                            case KEY.LEFT: if (~$selectMultiple.activeMatchIndex) return prev; else return last; break; case KEY.RIGHT: if (!~$selectMultiple.activeMatchIndex || curr === last) { $select.activate(); return !1 } else return next; break; case KEY.BACKSPACE: if (~$selectMultiple.activeMatchIndex) { if ($selectMultiple.removeChoice(curr)) { return prev } else { return curr } } else { return last }
                                break; case KEY.DELETE: if (~$selectMultiple.activeMatchIndex) { $selectMultiple.removeChoice($selectMultiple.activeMatchIndex); return curr } else return !1
                        }
                    }
                    newIndex = getNewActiveMatchIndex(); if (!$select.selected.length || newIndex === !1) $selectMultiple.activeMatchIndex = -1; else $selectMultiple.activeMatchIndex = Math.min(last, Math.max(first, newIndex)); return !0
                }
                $select.searchInput.on('keyup', function (e) {
                    if (!KEY.isVerticalMovement(e.which)) { scope.$evalAsync(function () { $select.activeIndex = $select.taggingLabel === !1 ? -1 : 0 }) }
                    if ($select.tagging.isActivated && $select.search.length > 0) {
                        if (e.which === KEY.TAB || KEY.isControl(e) || KEY.isFunctionKey(e) || e.which === KEY.ESC || KEY.isVerticalMovement(e.which)) { return }
                        $select.activeIndex = $select.taggingLabel === !1 ? -1 : 0; if ($select.taggingLabel === !1) return; var items = angular.copy($select.items); var stashArr = angular.copy($select.items); var newItem; var item; var hasTag = !1; var dupeIndex = -1; var tagItems; var tagItem; if ($select.tagging.fct !== undefined) {
                            tagItems = $select.$filter('filter')(items, { 'isTag': !0 }); if (tagItems.length > 0) { tagItem = tagItems[0] }
                            if (items.length > 0 && tagItem) { hasTag = !0; items = items.slice(1, items.length); stashArr = stashArr.slice(1, stashArr.length) }
                            newItem = $select.tagging.fct($select.search); if (stashArr.some(function (origItem) { return angular.equals(origItem, newItem) }) || $select.selected.some(function (origItem) { return angular.equals(origItem, newItem) })) { scope.$evalAsync(function () { $select.activeIndex = 0; $select.items = items }); return }
                            if (newItem) newItem.isTag = !0
                        } else {
                            tagItems = $select.$filter('filter')(items, function (item) { return item.match($select.taggingLabel) }); if (tagItems.length > 0) { tagItem = tagItems[0] }
                            item = items[0]; if (item !== undefined && items.length > 0 && tagItem) { hasTag = !0; items = items.slice(1, items.length); stashArr = stashArr.slice(1, stashArr.length) }
                            newItem = $select.search + ' ' + $select.taggingLabel; if (_findApproxDupe($select.selected, $select.search) > -1) { return }
                            if (_findCaseInsensitiveDupe(stashArr.concat($select.selected))) {
                                if (hasTag) { items = stashArr; scope.$evalAsync(function () { $select.activeIndex = 0; $select.items = items }) }
                                return
                            }
                            if (_findCaseInsensitiveDupe(stashArr)) {
                                if (hasTag) { $select.items = stashArr.slice(1, stashArr.length) }
                                return
                            }
                        }
                        if (hasTag) dupeIndex = _findApproxDupe($select.selected, newItem); if (dupeIndex > -1) { items = items.slice(dupeIndex + 1, items.length - 1) } else { items = []; if (newItem) items.push(newItem); items = items.concat(stashArr) }
                        scope.$evalAsync(function () { $select.activeIndex = 0; $select.items = items; if ($select.isGrouped) { var itemsWithoutTag = newItem ? items.slice(1) : items; $select.setItemsFn(itemsWithoutTag); if (newItem) { $select.items.unshift(newItem); $select.groups.unshift({ name: '', items: [newItem], tagging: !0 }) } } })
                    }
                }); function _findCaseInsensitiveDupe(arr) {
                    if (arr === undefined || $select.search === undefined) { return !1 }
                    var hasDupe = arr.filter(function (origItem) {
                        if ($select.search.toUpperCase() === undefined || origItem === undefined) { return !1 }
                        return origItem.toUpperCase() === $select.search.toUpperCase()
                    }).length > 0; return hasDupe
                }
                function _findApproxDupe(haystack, needle) {
                    var dupeIndex = -1; if (angular.isArray(haystack)) {
                        var tempArr = angular.copy(haystack); for (var i = 0; i < tempArr.length; i++) {
                            if ($select.tagging.fct === undefined) { if (tempArr[i] + ' ' + $select.taggingLabel === needle) { dupeIndex = i } } else {
                                var mockObj = tempArr[i]; if (angular.isObject(mockObj)) { mockObj.isTag = !0 }
                                if (angular.equals(mockObj, needle)) { dupeIndex = i }
                            }
                        }
                    }
                    return dupeIndex
                }
                $select.searchInput.on('blur', function () { $timeout(function () { $selectMultiple.activeMatchIndex = -1 }) })
            }
        }
    }]); uis.directive('uiSelectNoChoice', ['uiSelectConfig', function (uiSelectConfig) { return { restrict: 'EA', require: '^uiSelect', replace: !0, transclude: !0, templateUrl: function (tElement) { tElement.addClass('ui-select-no-choice'); var theme = tElement.parent().attr('theme') || uiSelectConfig.theme; return theme + '/no-choice.tpl.html' } } }]); uis.directive('uiSelectSingle', ['$timeout', '$compile', function ($timeout, $compile) {
        return {
            restrict: 'EA', require: ['^uiSelect', '^ngModel'], link: function (scope, element, attrs, ctrls) {
                var $select = ctrls[0]; var ngModel = ctrls[1]; ngModel.$parsers.unshift(function (inputValue) {
                    if (isNil(inputValue)) { return inputValue }
                    var locals = {}, result; locals[$select.parserResult.itemName] = inputValue; result = $select.parserResult.modelMapper(scope, locals); return result
                }); ngModel.$formatters.unshift(function (inputValue) {
                    if (isNil(inputValue)) { return inputValue }
                    var data = $select.parserResult && $select.parserResult.source(scope, { $select: { search: '' } }), locals = {}, result; if (data) {
                        var checkFnSingle = function (d) { locals[$select.parserResult.itemName] = d; result = $select.parserResult.modelMapper(scope, locals); return result === inputValue }; if ($select.selected && checkFnSingle($select.selected)) { return $select.selected }
                        for (var i = data.length - 1; i >= 0; i--) { if (checkFnSingle(data[i])) return data[i] }
                    }
                    return inputValue
                }); scope.$watch('$select.selected', function (newValue) { if (ngModel.$viewValue !== newValue) { ngModel.$setViewValue(newValue) } }); ngModel.$render = function () { $select.selected = ngModel.$viewValue }; scope.$on('uis:select', function (event, item) { $select.selected = item; var locals = {}; locals[$select.parserResult.itemName] = item; $timeout(function () { $select.onSelectCallback(scope, { $item: item, $model: isNil(item) ? item : $select.parserResult.modelMapper(scope, locals) }) }) }); scope.$on('uis:close', function (event, skipFocusser) { $timeout(function () { $select.focusser.prop('disabled', !1); if (!skipFocusser) $select.focusser[0].focus() }, 0, !1) }); scope.$on('uis:activate', function () { focusser.prop('disabled', !0) }); var focusser = angular.element("<input ng-disabled='$select.disabled' class='ui-select-focusser ui-select-offscreen' type='text' id='{{ $select.focusserId }}' aria-label='{{ $select.focusserTitle }}' aria-haspopup='true' role='button' />"); $compile(focusser)(scope); $select.focusser = focusser; $select.focusInput = focusser; element.parent().append(focusser); focusser.bind("focus", function () { scope.$evalAsync(function () { $select.focus = !0 }) }); focusser.bind("blur", function () { scope.$evalAsync(function () { $select.focus = !1 }) }); focusser.bind("keydown", function (e) {
                    if (e.which === KEY.BACKSPACE && $select.backspaceReset !== !1) { e.preventDefault(); e.stopPropagation(); $select.select(undefined); scope.$apply(); return }
                    if (e.which === KEY.TAB || KEY.isControl(e) || KEY.isFunctionKey(e) || e.which === KEY.ESC) { return }
                    if (e.which == KEY.DOWN || e.which == KEY.UP || e.which == KEY.ENTER || e.which == KEY.SPACE) { e.preventDefault(); e.stopPropagation(); $select.activate() }
                    scope.$digest()
                }); focusser.bind("keyup input", function (e) {
                    if (e.which === KEY.TAB || KEY.isControl(e) || KEY.isFunctionKey(e) || e.which === KEY.ESC || e.which == KEY.ENTER || e.which === KEY.BACKSPACE) { return }
                    $select.activate(focusser.val()); focusser.val(''); scope.$digest()
                })
            }
        }
    }]); uis.directive('uiSelectSort', ['$timeout', 'uiSelectConfig', 'uiSelectMinErr', function ($timeout, uiSelectConfig, uiSelectMinErr) {
        return {
            require: ['^^uiSelect', '^ngModel'], link: function (scope, element, attrs, ctrls) {
                if (scope[attrs.uiSelectSort] === null) { throw uiSelectMinErr('sort', 'Expected a list to sort') }
                var $select = ctrls[0]; var $ngModel = ctrls[1]; var options = angular.extend({ axis: 'horizontal' }, scope.$eval(attrs.uiSelectSortOptions)); var axis = options.axis; var draggingClassName = 'dragging'; var droppingClassName = 'dropping'; var droppingBeforeClassName = 'dropping-before'; var droppingAfterClassName = 'dropping-after'; scope.$watch(function () { return $select.sortable }, function (newValue) { if (newValue) { element.attr('draggable', !0) } else { element.removeAttr('draggable') } }); element.on('dragstart', function (event) { element.addClass(draggingClassName); (event.dataTransfer || event.originalEvent.dataTransfer).setData('text', scope.$index.toString()) }); element.on('dragend', function () { removeClass(draggingClassName) }); var move = function (from, to) { this.splice(to, 0, this.splice(from, 1)[0]) }; var removeClass = function (className) { angular.forEach($select.$element.querySelectorAll('.' + className), function (el) { angular.element(el).removeClass(className) }) }; var dragOverHandler = function (event) { event.preventDefault(); var offset = axis === 'vertical' ? event.offsetY || event.layerY || (event.originalEvent ? event.originalEvent.offsetY : 0) : event.offsetX || event.layerX || (event.originalEvent ? event.originalEvent.offsetX : 0); if (offset < (this[axis === 'vertical' ? 'offsetHeight' : 'offsetWidth'] / 2)) { removeClass(droppingAfterClassName); element.addClass(droppingBeforeClassName) } else { removeClass(droppingBeforeClassName); element.addClass(droppingAfterClassName) } }; var dropTimeout; var dropHandler = function (event) { event.preventDefault(); var droppedItemIndex = parseInt((event.dataTransfer || event.originalEvent.dataTransfer).getData('text'), 10); $timeout.cancel(dropTimeout); dropTimeout = $timeout(function () { _dropHandler(droppedItemIndex) }, 20) }; var _dropHandler = function (droppedItemIndex) {
                    var theList = scope.$eval(attrs.uiSelectSort); var itemToMove = theList[droppedItemIndex]; var newIndex = null; if (element.hasClass(droppingBeforeClassName)) { if (droppedItemIndex < scope.$index) { newIndex = scope.$index - 1 } else { newIndex = scope.$index } } else { if (droppedItemIndex < scope.$index) { newIndex = scope.$index } else { newIndex = scope.$index + 1 } }
                    move.apply(theList, [droppedItemIndex, newIndex]); $ngModel.$setViewValue(Date.now()); scope.$apply(function () { scope.$emit('uiSelectSort:change', { array: theList, item: itemToMove, from: droppedItemIndex, to: newIndex }) }); removeClass(droppingClassName); removeClass(droppingBeforeClassName); removeClass(droppingAfterClassName); element.off('drop', dropHandler)
                }; element.on('dragenter', function () {
                    if (element.hasClass(draggingClassName)) { return }
                    element.addClass(droppingClassName); element.on('dragover', dragOverHandler); element.on('drop', dropHandler)
                }); element.on('dragleave', function (event) {
                    if (event.target != element) { return }
                    removeClass(droppingClassName); removeClass(droppingBeforeClassName); removeClass(droppingAfterClassName); element.off('dragover', dragOverHandler); element.off('drop', dropHandler)
                })
            }
        }
    }]); uis.factory('$$uisDebounce', ['$timeout', function ($timeout) {
        return function (callback, debounceTime) {
            var timeoutPromise; return function () {
                var self = this; var args = Array.prototype.slice.call(arguments); if (timeoutPromise) { $timeout.cancel(timeoutPromise) }
                timeoutPromise = $timeout(function () { callback.apply(self, args) }, debounceTime)
            }
        }
    }]); uis.directive('uisOpenClose', ['$parse', '$timeout', function ($parse, $timeout) { return { restrict: 'A', require: 'uiSelect', link: function (scope, element, attrs, $select) { $select.onOpenCloseCallback = $parse(attrs.uisOpenClose); scope.$watch('$select.open', function (isOpen, previousState) { if (isOpen !== previousState) { $timeout(function () { $select.onOpenCloseCallback(scope, { isOpen: isOpen }) }) } }) } } }]); uis.service('uisRepeatParser', ['uiSelectMinErr', '$parse', function (uiSelectMinErr, $parse) {
        var self = this; self.parse = function (expression) {
            var match; match = expression.match(/^\s*(?:([\s\S]+?)\s+as\s+)?(?:([\$\w][\$\w]*)|(?:\(\s*([\$\w][\$\w]*)\s*,\s*([\$\w][\$\w]*)\s*\)))\s+in\s+(\s*[\s\S]+?)?(?:\s+track\s+by\s+([\s\S]+?))?\s*$/); if (!match) { throw uiSelectMinErr('iexp', "Expected expression in form of '_item_ in _collection_[ track by _id_]' but got '{0}'.", expression) }
            var source = match[5], filters = ''; if (match[3]) { source = match[5].replace(/(^\()|(\)$)/g, ''); var filterMatch = match[5].match(/^\s*(?:[\s\S]+?)(?:[^\|]|\|\|)+([\s\S]*)\s*$/); if (filterMatch && filterMatch[1].trim()) { filters = filterMatch[1]; source = source.replace(filters, '') } }
            return {
                itemName: match[4] || match[2], keyName: match[3], source: $parse(source), filters: filters, trackByExp: match[6], modelMapper: $parse(match[1] || match[4] || match[2]), repeatExpression: function (grouped) {
                    var expression = this.itemName + ' in ' + (grouped ? '$group.items' : '$select.items'); if (this.trackByExp) { expression += ' track by ' + this.trackByExp }
                    return expression
                }
            }
        }; self.getGroupNgRepeatExpression = function () { return '$group in $select.groups track by $group.name' }
    }])
}()); angular.module("ui.select").run(["$templateCache", function ($templateCache) { $templateCache.put("bootstrap/choices.tpl.html", "<ul class=\"ui-select-choices ui-select-choices-content ui-select-dropdown dropdown-menu\" ng-show=\"$select.open && $select.items.length > 0\"><li class=\"ui-select-choices-group\" id=\"ui-select-choices-{{ $select.generatedId }}\"><div class=\"divider\" ng-show=\"$select.isGrouped && $index > 0\"></div><div ng-show=\"$select.isGrouped\" class=\"ui-select-choices-group-label dropdown-header\" ng-bind=\"$group.name\"></div><div ng-attr-id=\"ui-select-choices-row-{{ $select.generatedId }}-{{$index}}\" class=\"ui-select-choices-row\" ng-class=\"{active: $select.isActive(this), disabled: $select.isDisabled(this)}\" role=\"option\"><span class=\"ui-select-choices-row-inner\"></span></div></li></ul>"); $templateCache.put("bootstrap/match-multiple.tpl.html", "<span class=\"ui-select-match\"><span ng-repeat=\"$item in $select.selected track by $index\"><span class=\"ui-select-match-item btn btn-default btn-xs\" tabindex=\"-1\" type=\"button\" ng-disabled=\"$select.disabled\" ng-click=\"$selectMultiple.activeMatchIndex = $index;\" ng-class=\"{\'btn-primary\':$selectMultiple.activeMatchIndex === $index, \'select-locked\':$select.isLocked(this, $index)}\" ui-select-sort=\"$select.selected\"><span class=\"close ui-select-match-close\" ng-hide=\"$select.disabled\" ng-click=\"$selectMultiple.removeChoice($index)\">&nbsp;&times;</span> <span uis-transclude-append=\"\"></span></span></span></span>"); $templateCache.put("bootstrap/match.tpl.html", "<div class=\"ui-select-match\" ng-hide=\"$select.open && $select.searchEnabled\" ng-disabled=\"$select.disabled\" ng-class=\"{\'btn-default-focus\':$select.focus}\"><span tabindex=\"-1\" class=\"btn btn-default form-control ui-select-toggle\" aria-label=\"{{ $select.baseTitle }} activate\" ng-disabled=\"$select.disabled\" ng-click=\"$select.activate()\" style=\"outline: 0;\"><span ng-show=\"$select.isEmpty()\" class=\"ui-select-placeholder text-muted\">{{$select.placeholder}}</span> <span ng-hide=\"$select.isEmpty()\" class=\"ui-select-match-text pull-left\" ng-class=\"{\'ui-select-allow-clear\': $select.allowClear && !$select.isEmpty()}\" ng-transclude=\"\"></span> <i class=\"caret pull-right\" ng-click=\"$select.toggle($event)\"></i> <a ng-show=\"$select.allowClear && !$select.isEmpty() && ($select.disabled !== true)\" aria-label=\"{{ $select.baseTitle }} clear\" style=\"margin-right: 10px\" ng-click=\"$select.clear($event)\" class=\"btn btn-xs btn-link pull-right\"><i class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></i></a></span></div>"); $templateCache.put("bootstrap/no-choice.tpl.html", "<ul class=\"ui-select-no-choice dropdown-menu\" ng-show=\"$select.items.length == 0\"><li ng-transclude=\"\"></li></ul>"); $templateCache.put("bootstrap/select-multiple.tpl.html", "<div class=\"ui-select-container ui-select-multiple ui-select-bootstrap dropdown form-control\" ng-class=\"{open: $select.open}\"><div><div class=\"ui-select-match\"></div><input type=\"search\" autocomplete=\"off\" autocorrect=\"off\" autocapitalize=\"off\" spellcheck=\"false\" class=\"ui-select-search input-xs\" placeholder=\"{{$selectMultiple.getPlaceholder()}}\" ng-disabled=\"$select.disabled\" ng-click=\"$select.activate()\" ng-model=\"$select.search\" role=\"combobox\" aria-expanded=\"{{$select.open}}\" aria-label=\"{{$select.baseTitle}}\" ng-class=\"{\'spinner\': $select.refreshing}\" ondrop=\"return false;\"></div><div class=\"ui-select-choices\"></div><div class=\"ui-select-no-choice\"></div></div>"); $templateCache.put("bootstrap/select.tpl.html", "<div class=\"ui-select-container ui-select-bootstrap dropdown\" ng-class=\"{open: $select.open}\"><div class=\"ui-select-match\"></div><span ng-show=\"$select.open && $select.refreshing && $select.spinnerEnabled\" class=\"ui-select-refreshing {{$select.spinnerClass}}\"></span> <input type=\"search\" autocomplete=\"off\" tabindex=\"-1\" aria-expanded=\"true\" aria-label=\"{{ $select.baseTitle }}\" aria-owns=\"ui-select-choices-{{ $select.generatedId }}\" class=\"form-control ui-select-search\" ng-class=\"{ \'ui-select-search-hidden\' : !$select.searchEnabled }\" placeholder=\"{{$select.placeholder}}\" ng-model=\"$select.search\" ng-show=\"$select.open\"><div class=\"ui-select-choices\"></div><div class=\"ui-select-no-choice\"></div></div>"); $templateCache.put("select2/choices.tpl.html", "<ul tabindex=\"-1\" class=\"ui-select-choices ui-select-choices-content select2-results\"><li class=\"ui-select-choices-group\" ng-class=\"{\'select2-result-with-children\': $select.choiceGrouped($group) }\"><div ng-show=\"$select.choiceGrouped($group)\" class=\"ui-select-choices-group-label select2-result-label\" ng-bind=\"$group.name\"></div><ul id=\"ui-select-choices-{{ $select.generatedId }}\" ng-class=\"{\'select2-result-sub\': $select.choiceGrouped($group), \'select2-result-single\': !$select.choiceGrouped($group) }\"><li role=\"option\" ng-attr-id=\"ui-select-choices-row-{{ $select.generatedId }}-{{$index}}\" class=\"ui-select-choices-row\" ng-class=\"{\'select2-highlighted\': $select.isActive(this), \'select2-disabled\': $select.isDisabled(this)}\"><div class=\"select2-result-label ui-select-choices-row-inner\"></div></li></ul></li></ul>"); $templateCache.put("select2/match-multiple.tpl.html", "<span class=\"ui-select-match\"><li class=\"ui-select-match-item select2-search-choice\" ng-repeat=\"$item in $select.selected track by $index\" ng-class=\"{\'select2-search-choice-focus\':$selectMultiple.activeMatchIndex === $index, \'select2-locked\':$select.isLocked(this, $index)}\" ui-select-sort=\"$select.selected\"><span uis-transclude-append=\"\"></span> <a href=\"javascript:;\" class=\"ui-select-match-close select2-search-choice-close\" ng-click=\"$selectMultiple.removeChoice($index)\" tabindex=\"-1\"></a></li></span>"); $templateCache.put("select2/match.tpl.html", "<a class=\"select2-choice ui-select-match\" ng-class=\"{\'select2-default\': $select.isEmpty()}\" ng-click=\"$select.toggle($event)\" aria-label=\"{{ $select.baseTitle }} select\"><span ng-show=\"$select.isEmpty()\" class=\"select2-chosen\">{{$select.placeholder}}</span> <span ng-hide=\"$select.isEmpty()\" class=\"select2-chosen\" ng-transclude=\"\"></span> <abbr ng-if=\"$select.allowClear && !$select.isEmpty()\" class=\"select2-search-choice-close\" ng-click=\"$select.clear($event)\"></abbr> <span class=\"select2-arrow ui-select-toggle\"><b></b></span></a>"); $templateCache.put("select2/no-choice.tpl.html", "<div class=\"ui-select-no-choice dropdown\" ng-show=\"$select.items.length == 0\"><div class=\"dropdown-content\"><div data-selectable=\"\" ng-transclude=\"\"></div></div></div>"); $templateCache.put("select2/select-multiple.tpl.html", "<div class=\"ui-select-container ui-select-multiple select2 select2-container select2-container-multi\" ng-class=\"{\'select2-container-active select2-dropdown-open open\': $select.open, \'select2-container-disabled\': $select.disabled}\"><ul class=\"select2-choices\"><span class=\"ui-select-match\"></span><li class=\"select2-search-field\"><input type=\"search\" autocomplete=\"off\" autocorrect=\"off\" autocapitalize=\"off\" spellcheck=\"false\" role=\"combobox\" aria-expanded=\"true\" aria-owns=\"ui-select-choices-{{ $select.generatedId }}\" aria-label=\"{{ $select.baseTitle }}\" aria-activedescendant=\"ui-select-choices-row-{{ $select.generatedId }}-{{ $select.activeIndex }}\" class=\"select2-input ui-select-search\" placeholder=\"{{$selectMultiple.getPlaceholder()}}\" ng-disabled=\"$select.disabled\" ng-hide=\"$select.disabled\" ng-model=\"$select.search\" ng-click=\"$select.activate()\" style=\"width: 34px;\" ondrop=\"return false;\"></li></ul><div class=\"ui-select-dropdown select2-drop select2-with-searchbox select2-drop-active\" ng-class=\"{\'select2-display-none\': !$select.open || $select.items.length === 0}\"><div class=\"ui-select-choices\"></div></div></div>"); $templateCache.put("select2/select.tpl.html", "<div class=\"ui-select-container select2 select2-container\" ng-class=\"{\'select2-container-active select2-dropdown-open open\': $select.open, \'select2-container-disabled\': $select.disabled, \'select2-container-active\': $select.focus, \'select2-allowclear\': $select.allowClear && !$select.isEmpty()}\"><div class=\"ui-select-match\"></div><div class=\"ui-select-dropdown select2-drop select2-with-searchbox select2-drop-active\" ng-class=\"{\'select2-display-none\': !$select.open}\"><div class=\"search-container\" ng-class=\"{\'ui-select-search-hidden\':!$select.searchEnabled, \'select2-search\':$select.searchEnabled}\"><input type=\"search\" autocomplete=\"off\" autocorrect=\"off\" autocapitalize=\"off\" spellcheck=\"false\" ng-class=\"{\'select2-active\': $select.refreshing}\" role=\"combobox\" aria-expanded=\"true\" aria-owns=\"ui-select-choices-{{ $select.generatedId }}\" aria-label=\"{{ $select.baseTitle }}\" class=\"ui-select-search select2-input\" ng-model=\"$select.search\"></div><div class=\"ui-select-choices\"></div><div class=\"ui-select-no-choice\"></div></div></div>"); $templateCache.put("selectize/choices.tpl.html", "<div ng-show=\"$select.open\" class=\"ui-select-choices ui-select-dropdown selectize-dropdown\" ng-class=\"{\'single\': !$select.multiple, \'multi\': $select.multiple}\"><div class=\"ui-select-choices-content selectize-dropdown-content\"><div class=\"ui-select-choices-group optgroup\"><div ng-show=\"$select.isGrouped\" class=\"ui-select-choices-group-label optgroup-header\" ng-bind=\"$group.name\"></div><div role=\"option\" class=\"ui-select-choices-row\" ng-class=\"{active: $select.isActive(this), disabled: $select.isDisabled(this)}\"><div class=\"option ui-select-choices-row-inner\" data-selectable=\"\"></div></div></div></div></div>"); $templateCache.put("selectize/match-multiple.tpl.html", "<div class=\"ui-select-match\" data-value=\"\" ng-repeat=\"$item in $select.selected track by $index\" ng-click=\"$selectMultiple.activeMatchIndex = $index;\" ng-class=\"{\'active\':$selectMultiple.activeMatchIndex === $index}\" ui-select-sort=\"$select.selected\"><span class=\"ui-select-match-item\" ng-class=\"{\'select-locked\':$select.isLocked(this, $index)}\"><span uis-transclude-append=\"\"></span> <span class=\"remove ui-select-match-close\" ng-hide=\"$select.disabled\" ng-click=\"$selectMultiple.removeChoice($index)\">&times;</span></span></div>"); $templateCache.put("selectize/match.tpl.html", "<div ng-hide=\"$select.searchEnabled && ($select.open || $select.isEmpty())\" class=\"ui-select-match\"><span ng-show=\"!$select.searchEnabled && ($select.isEmpty() || $select.open)\" class=\"ui-select-placeholder text-muted\">{{$select.placeholder}}</span> <span ng-hide=\"$select.isEmpty() || $select.open\" ng-transclude=\"\"></span></div>"); $templateCache.put("selectize/no-choice.tpl.html", "<div class=\"ui-select-no-choice selectize-dropdown\" ng-show=\"$select.items.length == 0\"><div class=\"selectize-dropdown-content\"><div data-selectable=\"\" ng-transclude=\"\"></div></div></div>"); $templateCache.put("selectize/select-multiple.tpl.html", "<div class=\"ui-select-container selectize-control multi plugin-remove_button\" ng-class=\"{\'open\': $select.open}\"><div class=\"selectize-input\" ng-class=\"{\'focus\': $select.open, \'disabled\': $select.disabled, \'selectize-focus\' : $select.focus}\" ng-click=\"$select.open && !$select.searchEnabled ? $select.toggle($event) : $select.activate()\"><div class=\"ui-select-match\"></div><input type=\"search\" autocomplete=\"off\" tabindex=\"-1\" class=\"ui-select-search\" ng-class=\"{\'ui-select-search-hidden\':!$select.searchEnabled}\" placeholder=\"{{$selectMultiple.getPlaceholder()}}\" ng-model=\"$select.search\" ng-disabled=\"$select.disabled\" aria-expanded=\"{{$select.open}}\" aria-label=\"{{ $select.baseTitle }}\" ondrop=\"return false;\"></div><div class=\"ui-select-choices\"></div><div class=\"ui-select-no-choice\"></div></div>"); $templateCache.put("selectize/select.tpl.html", "<div class=\"ui-select-container selectize-control single\" ng-class=\"{\'open\': $select.open}\"><div class=\"selectize-input\" ng-class=\"{\'focus\': $select.open, \'disabled\': $select.disabled, \'selectize-focus\' : $select.focus}\" ng-click=\"$select.open && !$select.searchEnabled ? $select.toggle($event) : $select.activate()\"><div class=\"ui-select-match\"></div><input type=\"search\" autocomplete=\"off\" tabindex=\"-1\" class=\"ui-select-search ui-select-toggle\" ng-class=\"{\'ui-select-search-hidden\':!$select.searchEnabled}\" ng-click=\"$select.toggle($event)\" placeholder=\"{{$select.placeholder}}\" ng-model=\"$select.search\" ng-hide=\"!$select.isEmpty() && !$select.open\" ng-disabled=\"$select.disabled\" aria-label=\"{{ $select.baseTitle }}\"></div><div class=\"ui-select-choices\"></div><div class=\"ui-select-no-choice\"></div></div>") }])