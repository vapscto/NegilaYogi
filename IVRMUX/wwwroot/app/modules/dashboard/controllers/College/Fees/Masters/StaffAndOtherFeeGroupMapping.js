(function () {
    'use strict';
    angular
        .module('app')
        .controller('StaffAndOtherFeeGroupMappingController', StaffAndOtherFeeGroupMappingController)

    StaffAndOtherFeeGroupMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function StaffAndOtherFeeGroupMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

      

        $scope.eeditstudentsdata = [];

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));



        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.sortKey1 = "fmsG_Id";  
        $scope.reverse1 = true; 
        $scope.totcountsearch = 0;
        $scope.totcountsearch_s = 0;
        $scope.totcountsearch_o = 0;

        $scope.firstfncedit = function (vlobjedit) {

            if (vlobjedit.checkedgrplstedit == true) {
                angular.forEach($scope.grouplstedit, function (objedit) {
                    if (vlobjedit.fmG_Id == objedit.fmG_Id) {
                        angular.forEach($scope.headlstedit, function (objedit1) {
                            if (objedit1.fmG_Id == objedit.fmG_Id) {
                                objedit1.checkedheadlstedit = true;
                                angular.forEach($scope.installlstedit, function (objedit2) {
                                    if (objedit.fmG_Id == objedit2.fmG_Id && objedit1.fmH_Id == objedit2.fmH_Id) {
                                        objedit2.checkedinstallmentlstedit = true;
                                    }
                                });
                            }
                        });
                    }
                });
            } else {
                angular.forEach($scope.grouplstedit, function (objedit) {
                    if (vlobjedit.fmG_Id == objedit.fmG_Id) {
                        angular.forEach($scope.headlstedit, function (objedit1) {
                            if (objedit1.fmG_Id == objedit.fmG_Id) {
                                objedit1.checkedheadlstedit = false;
                                angular.forEach($scope.installlstedit, function (objedit2) {
                                    if (objedit.fmG_Id == objedit2.fmG_Id && objedit1.fmH_Id == objedit2.fmH_Id) {
                                        objedit2.checkedinstallmentlstedit = false;
                                    }
                                });
                            }
                        });
                    }
                });
            }
        }
        $scope.secfncedit = function (vlobjedit1) {

            if (vlobjedit1.checkedheadlstedit == true) {
    
                angular.forEach($scope.headlstedit, function (valedit) {
                    if (vlobjedit1.fmG_Id == valedit.fmG_Id && vlobjedit1.fmH_Id == valedit.fmH_Id) {
                        angular.forEach($scope.installlstedit, function (valedit1) {

                            if (valedit1.fmH_Id == valedit.fmH_Id && valedit1.fmG_Id == valedit.fmG_Id) {
                                valedit1.checkedinstallmentlstedit = true;
                            }

                        });
                    }
                });
            } else {
                angular.forEach($scope.headlstedit, function (valedit) {
                    if (vlobjedit1.fmG_Id == valedit.fmG_Id && vlobjedit1.fmH_Id == valedit.fmH_Id) {
                        angular.forEach($scope.installlstedit, function (valedit1) {
                            if (valedit1.fmH_Id == valedit.fmH_Id && valedit1.fmG_Id == valedit.fmG_Id) {
                                valedit1.checkedinstallmentlstedit = false;
                            }
                        });
                    }
                });
            }

            for (var s = 0; s < $scope.grouplstedit.length; s++) {
                if (vlobjedit1.fmG_Id == $scope.grouplstedit[s].fmG_Id) {
                    for (var t = 0; t < $scope.headlstedit.length; t++) {
                        if (vlobjedit1.fmG_Id == $scope.headlstedit[t].fmG_Id) {
                            if ($scope.headlstedit[t].checkedheadlstedit == false) {
                                $scope.grouplstedit[s].checkedgrplstedit = false;
                            }
                            else {
                                $scope.grouplstedit[s].checkedgrplstedit = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        $scope.trdfncedit = function (vlobjedit2) {

            for (var u = 0; u < $scope.headlstedit.length; u++) {
                if (vlobjedit2.fmG_Id == $scope.headlstedit[u].fmG_Id && vlobjedit2.fmH_Id == $scope.headlstedit[u].fmH_Id) {
                    for (var v = 0; v < $scope.installlstedit.length; v++) {
                        if ($scope.installlstedit[v].fmH_Id == $scope.headlstedit[u].fmH_Id && $scope.installlstedit[v].fmG_Id == $scope.headlstedit[u].fmG_Id) {
                            if ($scope.installlstedit[v].checkedinstallmentlstedit == false) {
                                $scope.headlstedit[u].checkedheadlstedit = false;
                            }
                            else {
                                $scope.headlstedit[u].checkedheadlstedit = true;
                                break;
                            }
                        }
                    }

                    for (var w = 0; w < $scope.grouplstedit.length; w++) {
                        if (vlobjedit2.fmG_Id == $scope.grouplstedit[w].fmG_Id) {
                            for (var x = 0; x < $scope.headlstedit.length; x++) {
                                if (vlobjedit2.fmG_Id == $scope.headlstedit[x].fmG_Id) {
                                    if ($scope.headlstedit[x].checkedheadlstedit == false) {
                                        $scope.grouplstedit[w].checkedgrplstedit = false;
                                    }
                                    else {
                                        $scope.grouplstedit[w].checkedgrplstedit = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }



        $scope.firstfnc = function (vlobj) {

            if (vlobj.checkedgrplst == true) {
                angular.forEach($scope.grouplst, function (obj) {
                    if (vlobj.fmG_Id == obj.fmG_Id) {
                        angular.forEach($scope.headlst, function (obj1) {
                            if (obj1.fmG_Id == obj.fmG_Id) {
                                obj1.checkedheadlst = true;
                                angular.forEach($scope.installlst, function (obj2) {
                                    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
                                        obj2.checkedinstallmentlst = true;
                                    }
                                });
                            }
                        });
                    }
                });
            } else {
                angular.forEach($scope.grouplst, function (obj) {
                    if (vlobj.fmG_Id == obj.fmG_Id) {
                        angular.forEach($scope.headlst, function (obj1) {
                            if (obj1.fmG_Id == obj.fmG_Id) {
                                obj1.checkedheadlst = false;
                                angular.forEach($scope.installlst, function (obj2) {
                                    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
                                        obj2.checkedinstallmentlst = false;
                                    }
                                });
                            }
                        });
                    }
                });
            }
        }
        $scope.firstfnc_s = function (vlobj_s) {

            if (vlobj_s.checkedgrplst_s == true) {
                angular.forEach($scope.grouplst_s, function (obj) {
                    if (vlobj_s.fmG_Id == obj.fmG_Id) {
                        angular.forEach($scope.headlst_s, function (obj1) {
                            if (obj1.fmG_Id == obj.fmG_Id) {
                                obj1.checkedheadlst_s = true;
                                angular.forEach($scope.installlst_s, function (obj2) {
                                    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
                                        obj2.checkedinstallmentlst_s = true;
                                    }
                                });
                            }
                        });
                    }
                });
            } else {
                angular.forEach($scope.grouplst_s, function (obj) {
                    if (vlobj_s.fmG_Id == obj.fmG_Id) {
                        angular.forEach($scope.headlst_s, function (obj1) {
                            if (obj1.fmG_Id == obj.fmG_Id) {
                                obj1.checkedheadlst_s = false;
                                angular.forEach($scope.installlst_s, function (obj2) {
                                    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
                                        obj2.checkedinstallmentlst_s = false;
                                    }
                                });
                            }
                        });
                    }
                });
            }
        }

        $scope.firstfnc_o = function (vlobj_o) {

            if (vlobj_o.checkedgrplst_o == true) {
                angular.forEach($scope.grouplst_o, function (obj) {
                    if (vlobj_o.fmG_Id == obj.fmG_Id) {
                        angular.forEach($scope.headlst_o, function (obj1) {
                            if (obj1.fmG_Id == obj.fmG_Id) {
                                obj1.checkedheadlst_o = true;
                                angular.forEach($scope.installlst_o, function (obj2) {
                                    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
                                        obj2.checkedinstallmentlst_o = true;
                                    }
                                });
                            }
                        });
                    }
                });
            } else {
                angular.forEach($scope.grouplst_o, function (obj) {
                    if (vlobj_o.fmG_Id == obj.fmG_Id) {
                        angular.forEach($scope.headlst_o, function (obj1) {
                            if (obj1.fmG_Id == obj.fmG_Id) {
                                obj1.checkedheadlst_o = false;
                                angular.forEach($scope.installlst_o, function (obj2) {
                                    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
                                        obj2.checkedinstallmentlst_o = false;
                                    }
                                });
                            }
                        });
                    }
                });
            }
        }

        $scope.secfnc = function (vlobj1) {

            if (vlobj1.checkedheadlst == true) {
                angular.forEach($scope.headlst, function (val) {
                    if (vlobj1.fmG_Id == val.fmG_Id && vlobj1.fmH_Id == val.fmH_Id) {
                        angular.forEach($scope.installlst, function (val1) {
                            if (val1.fmH_Id == val.fmH_Id && val1.fmG_Id == val.fmG_Id) {
                                val1.checkedinstallmentlst = true;
                            }
                        });
                    }
                });
            } else {

                angular.forEach($scope.headlst, function (val) {
                    if (vlobj1.fmG_Id == val.fmG_Id && vlobj1.fmH_Id == val.fmH_Id) {
                        angular.forEach($scope.installlst, function (val1) {
                            if (val1.fmH_Id == val.fmH_Id && val1.fmG_Id == val.fmG_Id) {
                                val1.checkedinstallmentlst = false;
                            }
                        });
                    }
                });
            }

            for (var s = 0; s < $scope.grouplst.length; s++) {
                if (vlobj1.fmG_Id == $scope.grouplst[s].fmG_Id) {
                    for (var t = 0; t < $scope.headlst.length; t++) {
                        if (vlobj1.fmG_Id == $scope.headlst[t].fmG_Id) {
                            if ($scope.headlst[t].checkedheadlst == false) {
                                $scope.grouplst[s].checkedgrplst = false;
                            }
                            else {
                                $scope.grouplst[s].checkedgrplst = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
        $scope.secfnc_s = function (vlobj1_s) {

            if (vlobj1_s.checkedheadlst_s == true) {
                angular.forEach($scope.headlst_s, function (val) {
                    if (vlobj1_s.fmG_Id == val.fmG_Id && vlobj1_s.fmH_Id == val.fmH_Id) {
                        angular.forEach($scope.installlst_s, function (val1) {
                            if (val1.fmH_Id == val.fmH_Id && val1.fmG_Id == val.fmG_Id) {
                                val1.checkedinstallmentlst_s = true;
                            }
                        });
                    }
                });
            } else {

                angular.forEach($scope.headlst_s, function (val) {
                    if (vlobj1_s.fmG_Id == val.fmG_Id && vlobj1_s.fmH_Id == val.fmH_Id) {
                        angular.forEach($scope.installlst_s, function (val1) {
                            if (val1.fmH_Id == val.fmH_Id && val1.fmG_Id == val.fmG_Id) {
                                val1.checkedinstallmentlst_s = false;
                            }
                        });
                    }
                });
            }

            for (var s = 0; s < $scope.grouplst_s.length; s++) {
                if (vlobj1_s.fmG_Id == $scope.grouplst_s[s].fmG_Id) {
                    for (var t = 0; t < $scope.headlst_s.length; t++) {
                        if (vlobj1_s.fmG_Id == $scope.headlst_s[t].fmG_Id) {
                            if ($scope.headlst_s[t].checkedheadlst_s == false) {
                                $scope.grouplst_s[s].checkedgrplst_s = false;
                            }
                            else {
                                $scope.grouplst_s[s].checkedgrplst_s = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        $scope.secfnc_o = function (vlobj1_o) {

            if (vlobj1_o.checkedheadlst_o == true) {
                angular.forEach($scope.headlst_o, function (val) {
                    if (vlobj1_o.fmG_Id == val.fmG_Id && vlobj1_o.fmH_Id == val.fmH_Id) {
                        angular.forEach($scope.installlst_o, function (val1) {
                            if (val1.fmH_Id == val.fmH_Id && val1.fmG_Id == val.fmG_Id) {
                                val1.checkedinstallmentlst_o = true;
                            }
                        });
                    }
                });
            } else {

                angular.forEach($scope.headlst_o, function (val) {
                    if (vlobj1_o.fmG_Id == val.fmG_Id && vlobj1_o.fmH_Id == val.fmH_Id) {
                        angular.forEach($scope.installlst_o, function (val1) {
                            if (val1.fmH_Id == val.fmH_Id && val1.fmG_Id == val.fmG_Id) {
                                val1.checkedinstallmentlst_o = false;
                            }
                        });
                    }
                });
            }

            for (var s = 0; s < $scope.grouplst_o.length; s++) {
                if (vlobj1_o.fmG_Id == $scope.grouplst_o[s].fmG_Id) {
                    for (var t = 0; t < $scope.headlst_o.length; t++) {
                        if (vlobj1_o.fmG_Id == $scope.headlst_o[t].fmG_Id) {
                            if ($scope.headlst_o[t].checkedheadlst_o == false) {
                                $scope.grouplst_o[s].checkedgrplst_o = false;
                            }
                            else {
                                $scope.grouplst_o[s].checkedgrplst_o = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        $scope.trdfnc = function (vlobj2, oobjj) {

            for (var u = 0; u < $scope.headlst.length; u++) {
                if (vlobj2.fmG_Id == $scope.headlst[u].fmG_Id && vlobj2.fmH_Id == $scope.headlst[u].fmH_Id) {
                    for (var v = 0; v < $scope.installlst.length; v++) {
                        if ($scope.installlst[v].fmH_Id == $scope.headlst[u].fmH_Id && $scope.installlst[v].fmG_Id == $scope.headlst[u].fmG_Id) {
                            if ($scope.installlst[v].checkedinstallmentlst == false) {
                                $scope.headlst[u].checkedheadlst = false;
                            }
                            else {
                                $scope.headlst[u].checkedheadlst = true;
                                break;
                            }
                        }
                    }

                    for (var w = 0; w < $scope.grouplst.length; w++) {
                        if (vlobj2.fmG_Id == $scope.grouplst[w].fmG_Id) {
                            for (var x = 0; x < $scope.headlst.length; x++) {
                                if (vlobj2.fmG_Id == $scope.headlst[x].fmG_Id) {
                                    if ($scope.headlst[x].checkedheadlst == false) {
                                        $scope.grouplst[w].checkedgrplst = false;
                                    }
                                    else {
                                        $scope.grouplst[w].checkedgrplst = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        $scope.trdfnc_s = function (vlobj2_s, oobjj_s) {

            for (var u = 0; u < $scope.headlst_s.length; u++) {
                if (vlobj2_s.fmG_Id == $scope.headlst_s[u].fmG_Id && vlobj2_s.fmH_Id == $scope.headlst_s[u].fmH_Id) {
                    for (var v = 0; v < $scope.installlst_s.length; v++) {
                        if ($scope.installlst_s[v].fmH_Id == $scope.headlst_s[u].fmH_Id && $scope.installlst_s[v].fmG_Id == $scope.headlst_s[u].fmG_Id) {
                            if ($scope.installlst_s[v].checkedinstallmentlst_s == false) {
                                $scope.headlst_s[u].checkedheadlst_s = false;
                            }
                            else {
                                $scope.headlst_s[u].checkedheadlst_s = true;
                                break;
                            }
                        }
                    }

                    for (var w = 0; w < $scope.grouplst_s.length; w++) {
                        if (vlobj2_s.fmG_Id == $scope.grouplst_s[w].fmG_Id) {
                            for (var x = 0; x < $scope.headlst_s.length; x++) {
                                if (vlobj2_s.fmG_Id == $scope.headlst_s[x].fmG_Id) {
                                    if ($scope.headlst_s[x].checkedheadlst_s == false) {
                                        $scope.grouplst_s[w].checkedgrplst_s = false;
                                    }
                                    else {
                                        $scope.grouplst_s[w].checkedgrplst_s = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        $scope.trdfnc_o = function (vlobj2_o, oobjj_o) {

            for (var u = 0; u < $scope.headlst_o.length; u++) {
                if (vlobj2_o.fmG_Id == $scope.headlst_o[u].fmG_Id && vlobj2_o.fmH_Id == $scope.headlst_o[u].fmH_Id) {
                    for (var v = 0; v < $scope.installlst_o.length; v++) {
                        if ($scope.installlst_o[v].fmH_Id == $scope.headlst_o[u].fmH_Id && $scope.installlst_o[v].fmG_Id == $scope.headlst_o[u].fmG_Id) {
                            if ($scope.installlst_o[v].checkedinstallmentlst_o == false) {
                                $scope.headlst_o[u].checkedheadlst_o = false;
                            }
                            else {
                                $scope.headlst_o[u].checkedheadlst_o = true;
                                break;
                            }
                        }
                    }

                    for (var w = 0; w < $scope.grouplst_o.length; w++) {
                        if (vlobj2_o.fmG_Id == $scope.grouplst_o[w].fmG_Id) {
                            for (var x = 0; x < $scope.headlst_o.length; x++) {
                                if (vlobj2_o.fmG_Id == $scope.headlst_o[x].fmG_Id) {
                                    if ($scope.headlst_o[x].checkedheadlst_o == false) {
                                        $scope.grouplst_o[w].checkedgrplst_o = false;
                                    }
                                    else {
                                        $scope.grouplst_o[w].checkedgrplst_o = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }



        $scope.toggleAll = function (allchkdata) {
            var toggleStatus = $scope.selectedAll;
            angular.forEach($scope.studentsdata, function (itm) {
                itm.studchecked = toggleStatus;
            });
        }
        $scope.toggleAll_s = function (allchkdata) {
            var toggleStatus = $scope.selectedAll_s;
            angular.forEach($scope.staff_list, function (itm) {
                itm.studchecked_s = toggleStatus;
            });
        }
        $scope.toggleAll_o = function (allchkdata) {
            var toggleStatus = $scope.selectedAll_o;
            angular.forEach($scope.student_list, function (itm) {
                itm.studchecked_o = toggleStatus;
            });
        }


        $scope.searchvalue1 = '';
        $scope.filtervalue1 = function (obj) {
            return angular.lowercase(obj.fmG_GroupName).indexOf(angular.lowercase($scope.searchvalue1)) >= 0 || angular.lowercase(obj.asmcL_ClassName).indexOf(angular.lowercase($scope.searchvalue1)) >= 0 || angular.lowercase(obj.amsT_FirstName + ' ' + obj.amsT_MiddleName + ' ' + obj.amsT_LastName).indexOf(angular.lowercase($scope.searchvalue1)) >= 0;
        }

        $scope.isOptionsRequired = function () {
            if ($scope.checkboxval != 'Staff' && $scope.checkboxval != 'Others') {
                return !$scope.studentsdata.some(function (options) {
                    return options.studchecked;
                });
            }
            else if ($scope.checkboxval == 'Staff' || $scope.checkboxval == 'Others') {
                return false;
            }
        }
        $scope.isOptionsRequired_s = function () {
            if ($scope.checkboxval == 'Staff') {
                return !$scope.staff_list.some(function (options) {
                    return options.studchecked_s;
                });
            }
            else if ($scope.checkboxval != 'Staff') {
                return false;
            }
        }
        $scope.isOptionsRequired_o = function () {
            if ($scope.checkboxval == 'Others') {
                return !$scope.student_list.some(function (options) {
                    return options.studchecked_o;
                });
            }
            else if ($scope.checkboxval != 'Others') {
                return false;
            }
        }
        $scope.isOptionsRequired1 = function () {
            if ($scope.checkboxval != 'Staff' && $scope.checkboxval != 'Others') {
                return !$scope.grouplst.some(function (options) {
                    return options.checkedgrplst;
                });
            }
            else if ($scope.checkboxval == 'Staff' || $scope.checkboxval == 'Others') {
                return false;
            }

        }
        $scope.isOptionsRequired1_s = function () {
            if ($scope.checkboxval == 'Staff') {
                return !$scope.grouplst_s.some(function (options) {
                    return options.checkedgrplst_s;
                });
            }
            else if ($scope.checkboxval != 'Staff') {
                return false;
            }
        }
        $scope.isOptionsRequired1_o = function () {
            if ($scope.checkboxval == 'Others') {
                return !$scope.grouplst_o.some(function (options) {
                    return options.checkedgrplst_o;
                });
            }
            else if ($scope.checkboxval != 'Others') {
                return false;
            }
        }

        $scope.Clearid = function () {
            $state.reload();
        }

        $scope.editcheckboxtreeview = function (usr) {

            var data = {
                "AMST_Id": usr.amsT_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("StaffAndOtherFeeGroupMapping/geteditdata", data).

                then(function (promise) {

                    $scope.eeditstudentsdata = promise.alldatathird;
                    if ($scope.eeditstudentsdata.length > 0) {
                        $scope.AMST_idedit = promise.amsT_Id;
                        angular.forEach($scope.grouplstedit, function (objedit) {
                            objedit.disablegrp = false;
                        });
                        angular.forEach($scope.headlstedit, function (objedit1) {
                            objedit1.disablehead = false;
                        });
                        angular.forEach($scope.installlstedit, function (objedit2) {
                            objedit2.disableins = false;
                        });

                        angular.forEach($scope.eeditstudentsdata, function (grpeditt) {

                            angular.forEach($scope.grouplstedit, function (objedit) {
                                if (grpeditt.fmG_Id == objedit.fmG_Id) {
                                    objedit.checkedgrplstedit = true;
                                    
                                    angular.forEach($scope.headlstedit, function (objedit1) {
                                        if (grpeditt.fmG_Id == objedit1.fmG_Id && objedit1.fmH_Id == grpeditt.fmH_Id) {
                                            objedit1.checkedheadlstedit = true;
                                            
                                            angular.forEach($scope.installlstedit, function (objedit2) {
                                                if (grpeditt.fmG_Id == objedit2.fmG_Id && grpeditt.fmH_Id == objedit2.fmH_Id && grpeditt.ftI_Id == objedit2.ftI_Id) {
                                                    objedit2.checkedinstallmentlstedit = true;
                                                    if (grpeditt.fsS_PaidAmount > 0) {
                                                        objedit2.disableins = true;
                                                        objedit1.disablehead = true;
                                                        objedit.disablegrp = true;
                                                    }
                                                }
                                            });
                                        }

                                    });
                                }

                            });
                        });
                    }


                    else {
                        angular.forEach($scope.grouplstedit, function (objedit) {
                            objedit.checkedgrplstedit = false;
                            objedit.disablegrp = false;
                        });
                        angular.forEach($scope.headlstedit, function (objedit1) {
                            objedit1.checkedheadlstedit = false;
                            objedit1.disablehead = false;
                        });
                        angular.forEach($scope.installlstedit, function (objedit2) {
                            objedit2.checkedinstallmentlstedit = false;
                            objedit2.disableins = false;
                        });
                        swal("Student has not mapped with any of the group!")
                        $('#editmodal').modal('hide');
                    }

                })
        }

        $scope.page1 = "page1";
        $scope.page2 = "page2";
        $scope.page_s = "page_s";
        $scope.page2_s = "page2_s";

        $scope.reverse1 = true;
        $scope.reverse2 = true;
        

        $scope.cllose = function () {

            angular.forEach($scope.grouplstedit, function (objedit) {
                objedit.checkedgrplstedit = false;
            });
            angular.forEach($scope.headlstedit, function (objedit1) {
                objedit1.checkedheadlstedit = false;
            });
            angular.forEach($scope.installlstedit, function (objedit2) {
                objedit2.checkedinstallmentlstedit = false;
            });
            $('#editmodal').modal('hide');
            $('#editmodal_s').modal('hide');
            $('#editmodal_o').modal('hide');
        }

       
        var temp_staff_list = [];
        var temp_grid_staff_list = [];
        var temp_oth_student_list = [];
        var temp_grid_oth_student_list = [];
        $scope.formload = function () {
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = paginationformasters

            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = paginationformasters

            $scope.currentPage_s = 1;
            $scope.itemsPerPage_s = paginationformasters

            $scope.currentPage_o = 1;
            $scope.itemsPerPage_o = paginationformasters

            $scope.currentPage2_s = 1;
            $scope.itemsPerPage2_s = paginationformasters

            $scope.currentPage2_o = 1;
            $scope.itemsPerPage2_o = paginationformasters

            var pageid = 1;

            apiService.getURI("StaffAndOtherFeeGroupMapping/getalldetails", pageid).
                then(function (promise) {

                    $scope.yearlst = promise.academicdrp;

                    $scope.ASMAY_Id = academicyrlst[0].asmaY_Id;

                  //  $scope.studentsdata = promise.alldata;
                   // templistdata = $scope.studentsdata;
                    $scope.grouplst = promise.fillmastergroup;
                    $scope.headlst = promise.fillmasterhead;
                    $scope.installlst = promise.fillinstallment;
                    $scope.grouplstedit = promise.fillmastergroup;
                    $scope.headlstedit = promise.fillmasterhead;
                    $scope.installlstedit = promise.fillinstallment;

                    $scope.thirdgrid = promise.alldatathird;

                  

                    $scope.classdrp = true;
                    $scope.areadrp = true;


                   

                    if (promise.configsetting.length > 0) {
                        $scope.FMC_EableStaffTrans = promise.configsetting[0].fmC_EableStaffTrans;
                        $scope.FMC_EableOtherStudentTrans = promise.configsetting[0].fmC_EableOtherStudentTrans;
                    }
                    else {
                        $scope.FMC_EableStaffTrans = 0;
                        $scope.FMC_EableOtherStudentTrans = 0;
                    }


                    $scope.onclickloaddata();


                })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;  
                $scope.reverse = !$scope.reverse; 
            }

            $scope.sort1 = function (keyname1) {
                $scope.sortKey1 = keyname1;   
                $scope.reverse1 = !$scope.reverse1; 
            }
        }

        $scope.DeletRecord = function (studentmappingid, studentid, groupid) {

            var studmapid = studentmappingid;
            var studentid = studentid;

            var data = {
                "FMSG_Id": studmapid,
                "AMST_Id": studentid,
                "FMG_Id": groupid,
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }


            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("StaffAndOtherFeeGroupMapping/Deletedetails", data).
                            then(function (promise) {

                          
                                if (promise.returnval == "true") {

                             

                                    swal('Record Deleted Successfully');
                                }
                                else {
                                    swal('Record cannot be Deleted.Transaction has already been done for this group');
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        }
        $scope.DeletRecord_s = function (staffmappedid, staffid, groupid) {

            var stafmapid = staffmappedid;
            var stafid = staffid;

            var data = {
                "FMSTGH_Id": stafmapid,
                "HRME_Id": stafid,
                "FMG_Id": groupid,
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }


            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("StaffAndOtherFeeGroupMapping/Deletedetails_s", data).
                            then(function (promise) {

                          

                                if (promise.returnval == "true") {

                         

                                    swal('Record Deleted Successfully');
                                }
                                else {
                                    swal('Record cannot be Deleted.Transaction has already been done for this group');
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        }

        $scope.DeletRecord_o = function (oth_studentmappedid, oth_studentid, groupid) {

            var studentmapid = oth_studentmappedid;
            var oth_stuid = oth_studentid;

            var data = {
                "FMOSTGH_Id": studentmapid,
                "FMOST_Id": oth_stuid,
                "FMG_Id": groupid,
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }


            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("StaffAndOtherFeeGroupMapping/Deletedetails_o", data).
                            then(function (promise) {

                            

                                if (promise.returnval == "true") {


                                    swal('Record Deleted Successfully');
                                }
                                else {
                                    swal('Record cannot be Deleted.Transaction has already been done for this group');
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        }


        $scope.fillstudentsroute = function () {
            debugger;
            if ($scope.checkboxvalAreawise == true) {
                var data = {

                    "radioval": $scope.checkboxval,
                    "classwisecheckboxvalue": $scope.checkboxvalAreawise,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "TRMA_Id": $scope.trmA_Id
                }
            }
            else {
                var data = {
                    "TRMR_Id": $scope.trmR_Id,
                    "radioval": $scope.checkboxval,
                    "ASMAY_Id": $scope.ASMAY_Id
                }
            }


            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("StaffAndOtherFeeGroupMapping/fillstudentsroute", data).
                then(function (promise) {

                    if (promise.returnval == "Yes") {
                        $scope.studentsdata = promise.alldata;
                        templistdata = $scope.studentsdata;
                    }
                    else { $scope.studentsdata = {}; swal("No Records Found!"); }
                })
        };


        $scope.fillstudents = function () {
            var classid = $scope.ASMCL_Id;
            var secid = $scope.sectiondrp;
            var radiobtnvalue = $scope.checkboxval;
            var classcheckedvalue = $scope.checkboxvalClasswise;

            var data = {
                "ASMCL_Id": classid,
                "ASMS_Id": secid,
                "radioval": $scope.checkboxval,
                "classwisecheckboxvalue": classcheckedvalue,
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("StaffAndOtherFeeGroupMapping/getgroupmappedheads", data).
                then(function (promise) {

                    if (promise.returnval == "Yes") {
                        $scope.studentsdata = promise.alldata;
                        templistdata = $scope.studentsdata;

                        $scope.searchstud = "";
                        $scope.searchtxtstud = "";
                    }
                    else {
                        $scope.studentsdata = {}; swal("No Records Found!");
                        $scope.searchstud = "";
                        $scope.searchtxtstud = "";
                    }
                })
        };

        $scope.feeclasscatdrp = true;
        $scope.admissionclscatdrp = true;
        $scope.busroutedrp = true;
        $scope.classdrp = true;
        $scope.areadrp = true;

        $scope.clearsearch1 = function () {
            debugger;
            $scope.studentsdata = templistdata;
     
            $scope.searchstud = '';
            $scope.search_flag = false;
        }

        $scope.onclickloaddata = function () {

            if ($scope.checkboxval == "Staff") {
                $scope.feeclasscatdrp = true;
                $scope.admissionclscatdrp = true;
                $scope.busroutedrp = true;
                $scope.classdrp = true;
                $scope.areadrp = true;
                $scope.checkboxvalClasswise = false;
                $scope.checkboxvalAreawise = false;

            }
            else if ($scope.checkboxval == "Others") {
                $scope.feeclasscatdrp = true;
                $scope.admissionclscatdrp = true;
                $scope.busroutedrp = true;
                $scope.classdrp = true;
                $scope.areadrp = true;
                $scope.checkboxvalClasswise = false;
                $scope.checkboxvalAreawise = false;

            }

            


            var data = {
                "radioval": $scope.checkboxval,
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("StaffAndOtherFeeGroupMapping/radiobtndata", data).
                then(function (promise) {
                    
                 if (promise.radioval == "Staff") {
                        $scope.grouplst_s = promise.fillmastergroup;
                        $scope.headlst_s = promise.fillmasterhead;
                        $scope.installlst_s = promise.fillinstallment;

                        angular.forEach(promise.stafflist, function (stf) {
                            stf.HRME_SaveFlg = false;
                        })
                        if (promise.saved_stafflist.length > 0) {
                            angular.forEach(promise.saved_stafflist, function (s_stf) {
                                angular.forEach(promise.stafflist, function (stf) {
                                    if (stf.hrmE_Id == s_stf) {
                                        stf.HRME_SaveFlg = true;
                                    }
                                })
                            })
                        }

                        temp_staff_list = promise.stafflist;
                        temp_grid_staff_list = promise.grid_stafflist;
                        $scope.staff_list = promise.stafflist;
                        $scope.thirdgrid_s = promise.grid_stafflist;

                        $scope.totcountfirst_s = promise.grid_stafflist.length;

                    }
                    else if (promise.radioval == "Others") {
                        $scope.grouplst_o = promise.fillmastergroup;
                        $scope.headlst_o = promise.fillmasterhead;
                        $scope.installlst_o = promise.fillinstallment


                        angular.forEach(promise.oth_studentlist, function (stf) {
                            stf.FMOST_SaveFlg = false;
                        })
                        if (promise.saved_oth_studentlist.length > 0) {
                            angular.forEach(promise.saved_oth_studentlist, function (o_stu) {
                                angular.forEach(promise.oth_studentlist, function (stu) {
                                    if (stu.fmosT_Id == o_stu) {
                                        stu.FMOST_SaveFlg = true;
                                    }
                                })
                            })
                        }

                        temp_oth_student_list = promise.oth_studentlist;
                        temp_grid_oth_student_list = promise.grid_oth_studentlist;
                        $scope.student_list = promise.oth_studentlist;
                        $scope.thirdgrid_o = promise.grid_oth_studentlist;

                        $scope.totcountfirst_o = promise.grid_oth_studentlist.length;

                    }
                    
                    $scope.search_flag = false;
                    $scope.search_flag1 = false;
                    $scope.search_flag1_s = false;
                    $scope.thirdgrid = promise.alldatathird;
                    $scope.studentsdata = promise.alldata;

                    templistdata = $scope.studentsdata;
                    
                })

        };

        $scope.arr = [];
        var amstid;
        $scope.onstudentclick = function (amstid) {
            $scope.selectedAll = $scope.studentsdata.every(function (itm) { return itm.studchecked; });
            var data = {
                "AMST_Id": amstid,
                "ASMAY_Id": $scope.ASMAY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("StaffAndOtherFeeGroupMapping/studentsavedgroup", data).
                then(function (promise) {
                    
                })
        }

        $scope.onstudentclick_s = function (hrmE_Id) {
            $scope.selectedAll_s = $scope.staff_list.every(function (itm) { return itm.studchecked_s; });

        }
        $scope.onstudentclick_o = function (fmosT_Id) {
            $scope.selectedAll_o = $scope.student_list.every(function (itm) { return itm.studchecked_o; });

        }

        $scope.onadmcatclick = function (catid) {

            $scope.angularData1 = {
                'nameList1': []
            };

            $scope.vals1 = [];

            var admcat = "AC";
        
            var data = {
                "AMC_Id": catid,
                "radioval": admcat,
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("StaffAndOtherFeeGroupMapping/getclassoncatselect", data).
                then(function (promise) {
                    if (promise.fillfeeclasscategory.length > 0) {
                        
                        $scope.studentsdata = promise.fillfeeclasscategory;
                        templistdata = $scope.studentsdata;
                        

                    }
                    else {
                        $scope.studentsdata = [];
                    }
                })
        }

        $scope.onfeecatclick = function (feecatcount, feecatid) {

            $scope.angularData1 = {
                'nameList1': []
            };

            $scope.vals1 = [];

            var feecat = "FCC";
            var data = {
                "FMCC_Id": $scope.FMCC_Id,
                "radioval": feecat,
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("StaffAndOtherFeeGroupMapping/getclassoncatselect", data).
                then(function (promise) {
                    if (promise.fillfeeclasscategory.length > 0) {
                       
                        $scope.studentsdata = promise.fillfeeclasscategory;
                        templistdata = $scope.studentsdata;
                        


                    }
                    else {
                        $scope.studentsdata = [];
                    }
                })
        }

        $scope.cleardata = function () {
            $scope.FMG_Id = "";
            $scope.Cmp_Code = "";
            $scope.install = "";
            $scope.details = "";
            $scope.totalgrid = [];
        }

        $scope.EditMasterSectvalue = function (employee) {
            $scope.editEmployee = employee.fyghM_Id;
            var orgid = $scope.editEmployee;
     
            apiService.getURI("StaffAndOtherFeeGroupMapping/Editdetails", orgid).
                then(function (promise) {
                    $scope.addnewbtn = false;
                    $scope.FMG_Id = promise.alldata[0].fmG_Id;
                    $scope.totalgrid.headcount.selected.FMH_Id = promise.alldata[0].fmH_Id;
                    $scope.totalgrid.installmentcount.FMI_Id = promise.alldata[0].fmI_Id;

                    if (promise.alldata[0].fyghM_FineApplicableFlag = 0) {

                        $scope.totalgrid.FYGHM_FineApplicableFlag = true;
                    }
                    if (promise.alldata[0].fyghM_Common_AmountFlag = 0) {

                        $scope.totalgrid.FYGHM_Common_AmountFlag = true;
                    }
                    if (promise.alldata[0].fyghM_ActiveFlag = 0) {

                        $scope.totalgrid.FYGHM_ActiveFlag = true;
                    }
                })
        }

        $scope.filterby = function () {
            var entereddata = $scope.search;

            var data = {
                "FMG_GroupName": $scope.searchthird,
                "FMH_FeeName": $scope.typethird,
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("StaffAndOtherFeeGroupMapping/1", data).
                then(function (promise) {
                    $scope.thirdgrid = promise.alldata;
                    swal("searched Successfully");
                })
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

       
        $scope.saveeditdata = function (AMST_idedit, grouplstedit, headlstedit, installlstedit) {


            var data = {
               
                "AMST_Id": AMST_idedit,
               
                savegrplst: grouplstedit,
                saveheadlst: headlstedit,
                saveftilst: installlstedit,
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("StaffAndOtherFeeGroupMapping/saveeditdata", data).
                then(function (promise) {


                    if (promise.returnval == "false") {
                        swal("You are missing Amount entry/Due Date/Fine Slab Settings/Category Mapping.")
                    }
                    else if (promise.returnval == "true") {
                        swal('Record Updated Successfully');
                    }
                    else if (promise.returnval == "Error") {
                        swal('Kindly contact Administrator ');
                    }

                    
                    $scope.formload();
                    $scope.cllose();
                    

                });

        }
        $scope.submitted = false;
        $scope.valsgroup = [];
        $scope.valshead = [];
        $scope.valsinstallment = [];
        $scope.valstudentlst = [];
        $scope.savedata = function (studentsdata, grouplst, headlst, installlst) {

            if ($scope.myForm.$valid) {

                for (var t = 0; t < grouplst.length; t++) {
                    if (grouplst[t].checkedgrplst == true) {
                        $scope.valsgroup.push(grouplst[t]);
                    }
                }

                for (var u = 0; u < headlst.length; u++) {
                    if (headlst[u].checkedheadlst == true) {
                        $scope.valshead.push(headlst[u]);
                    }
                }

                for (var v = 0; v < installlst.length; v++) {
                    if (installlst[v].checkedinstallmentlst == true) {
                        $scope.valsinstallment.push(installlst[v]);
                    }
                }


                for (var w = 0; w < studentsdata.length; w++) {
                    if (studentsdata[w].studchecked == true) {
                        $scope.valstudentlst.push(studentsdata[w]);
                    }
                }

                studentsdata = $scope.valstudentlst;
                grouplst = $scope.valsgroup;
                headlst = $scope.valshead;
                installlst = $scope.valsinstallment;

                var data = {
                    "FMG_Id": $scope.FMG_Id,
                    studentdata: studentsdata,
                    savegrplst: grouplst,
                    saveheadlst: headlst,
                    saveftilst: installlst,
                    "ASMAY_Id": $scope.ASMAY_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("StaffAndOtherFeeGroupMapping/", data).
                    then(function (promise) {

                        if (promise.returnval == "false") {
                            swal("You are missing Amount entry/Due Date/Fine Slab Settings/Category Mapping.")
                        }
                        else if (promise.returnval == "duplicate") {
                            swal('Group is already saved for the student');
                        }
                        else if (promise.returnval == "true") {
                            swal('Record Saved Successfully');
                        }
                        else if (promise.returnval == "Error") {
                            swal('Kindly contact Administrator ');
                        }

                        $scope.addnewbtn = true;
                       
                        $state.reload();
                    })
            }
            else {
                $scope.submitted = true;
            }

        };
        $scope.valsgroup_s = [];
        $scope.valshead_s = [];
        $scope.valsinstallment_s = [];
        $scope.valstudentlst_s = [];
        $scope.savedata_s = function (staff_list, grouplst, headlst, installlst) {
            var stf_cnt = 0;
            var grp_cnt = 0;
            angular.forEach(staff_list, function (stf) {
                if (stf.studchecked_s) {
                    stf_cnt += 1;
                }
            })
            angular.forEach(grouplst, function (grp) {
                if (grp.checkedgrplst_s) {
                    grp_cnt += 1;
                }
            })
            if (stf_cnt > 0 && grp_cnt > 0) {
                if ($scope.myForm.$valid) {
                    

                    for (var t = 0; t < grouplst.length; t++) {
                        if (grouplst[t].checkedgrplst_s == true) {
                            $scope.valsgroup_s.push(grouplst[t]);
                        }
                    }

                    for (var u = 0; u < headlst.length; u++) {
                        if (headlst[u].checkedheadlst_s == true) {
                            $scope.valshead_s.push(headlst[u]);
                        }
                    }

                    for (var v = 0; v < installlst.length; v++) {
                        if (installlst[v].checkedinstallmentlst_s == true) {
                            $scope.valsinstallment_s.push(installlst[v]);
                        }
                    }


                    for (var w = 0; w < staff_list.length; w++) {
                        if (staff_list[w].studchecked_s == true) {
                            $scope.valstudentlst_s.push(staff_list[w]);
                        }
                    }

                    staff_list = $scope.valstudentlst_s;
                    grouplst = $scope.valsgroup_s;
                    headlst = $scope.valshead_s;
                    installlst = $scope.valsinstallment_s;

                    var data = {
                        "FMG_Id": $scope.FMG_Id,
                        "FMSTGH_Id": $scope.FMSTGH_Id,
                        staff_list: staff_list,
                        savegrplst: grouplst,
                        saveheadlst: headlst,
                        saveftilst: installlst,
                        "ASMAY_Id": $scope.ASMAY_Id
                    }

                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("StaffAndOtherFeeGroupMapping/savedata_s", data).
                        then(function (promise) {

                            if (promise.returnval == "Cancel") {
                                swal("You are missing Amount entry/Due Date/Fine Slab Settings.")
                            }
                            else if (promise.returnval == "Duplicate") {
                                swal('Group is already saved for the student');
                            }
                            else if (promise.returnval == "Save") {
                                swal('Record Saved Successfully');
                            }
                            else if (promise.returnval == "Error") {
                                swal('Kindly contact Administrator ');
                            }

                            
                            $state.reload();
                          
                        })
                }
                else {
                    $scope.submitted = true;
                }
            }
            else {
                swal("Select Atleast One Staff And Group For Proceed");
            }



        };

        $scope.valsgroup_o = [];
        $scope.valshead_o = [];
        $scope.valsinstallment_o = [];
        $scope.valstudentlst_o = [];
        $scope.savedata_o = function (student_list, grouplst, headlst, installlst) {
            var stu_cnt = 0;
            var grp_cnt = 0;
            angular.forEach(student_list, function (stf) {
                if (stf.studchecked_o) {
                    stu_cnt += 1;
                }
            })
            angular.forEach(grouplst, function (grp) {
                if (grp.checkedgrplst_o) {
                    grp_cnt += 1;
                }
            })
            if (stu_cnt > 0 && grp_cnt > 0) {
                if ($scope.myForm.$valid) {
                

                    for (var t = 0; t < grouplst.length; t++) {
                        if (grouplst[t].checkedgrplst_o == true) {
                            $scope.valsgroup_o.push(grouplst[t]);
                        }
                    }

                    for (var u = 0; u < headlst.length; u++) {
                        if (headlst[u].checkedheadlst_o == true) {
                            $scope.valshead_o.push(headlst[u]);
                        }
                    }

                    for (var v = 0; v < installlst.length; v++) {
                        if (installlst[v].checkedinstallmentlst_o == true) {
                            $scope.valsinstallment_o.push(installlst[v]);
                        }
                    }


                    for (var w = 0; w < student_list.length; w++) {
                        if (student_list[w].studchecked_o == true) {
                            $scope.valstudentlst_o.push(student_list[w]);
                        }
                    }

                    student_list = $scope.valstudentlst_o;
                    grouplst = $scope.valsgroup_o;
                    headlst = $scope.valshead_o;
                    installlst = $scope.valsinstallment_o;

                    var data = {
                        "FMG_Id": $scope.FMG_Id,
                        "FMOSTGH_Id": $scope.FMOSTGH_Id,
                        student_list: student_list,
                        savegrplst: grouplst,
                        saveheadlst: headlst,
                        saveftilst: installlst,
                        "ASMAY_Id": $scope.ASMAY_Id
                    }

                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("StaffAndOtherFeeGroupMapping/savedata_o", data).
                        then(function (promise) {

                            if (promise.returnval == "Cancel") {
                                swal("You are missing Amount entry/Due Date/Fine Slab Settings.")
                            }
                            else if (promise.returnval == "Duplicate") {
                                swal('Group is already saved for the student');
                            }
                            else if (promise.returnval == "Save") {
                                swal('Record Saved Successfully');
                            }
                            else if (promise.returnval == "Error") {
                                swal('Kindly contact Administrator ');
                            }

                   
                            $state.reload();
                 
                        })
                }
                else {
                    $scope.submitted = true;
                }
            }
            else {
                swal("Select Atleast One Other Student And Group For Proceed");
            }



        };


        $scope.search_flag = false;
        $scope.search123 = "";
        $scope.search123_s = "";
        var search_s = "";
        var search_ss = "";
        var search_oo = "";
        var list_s = [];
        $scope.onselectsearch = function () {
            search_s = $scope.search123;
       
            if (search_s == "" || search_s == undefined) {
                swal("Select Any Field For Search");
                $scope.search_flag1 = false;
            }
            else {
                $scope.search_flag1 = true;
                if ($scope.search123 == "0" || $scope.search123 == "1" || $scope.search123 == "2" || $scope.search123 == "3") {
                    $scope.txt = true;
                }
                $scope.searchtxt = "";
                $scope.searchdat = "";
                $scope.searchnumbr = "";

            }
        }
        $scope.onselectsearch_s = function () {
            search_ss = $scope.search123_s;
          
            if (search_ss == "" || search_ss == undefined) {
                swal("Select Any Field For Search");
                $scope.search_flag1_s = false;
            }
            else {
                $scope.search_flag1_s = true;
                if ($scope.search123_s == "0" || $scope.search123_s == "1" || $scope.search123_s == "2" || $scope.search123_s == "3" || $scope.search123_s == "4") {
                    $scope.txt_ss = true;
                }
                $scope.searchtxt_ss = "";


            }
        }

        $scope.onselectsearch_o = function () {
            search_oo = $scope.search123_o;
           
            if (search_oo == "" || search_oo == undefined) {
                swal("Select Any Field For Search");
                $scope.search_flag1_o = false;
            }
            else {
                $scope.search_flag1_o = true;
                if ($scope.search123_o == "0" || $scope.search123_o == "1" || $scope.search123_o == "2" || $scope.search123_o == "3") {
                    $scope.txt_oo = true;
                }
                $scope.searchtxt_oo = "";


            }
        }

        var templistdata = [];

        $scope.onselectsearchstudent = function () {
            search_s = $scope.searchstud;
          
            if (search_s == "" || search_s == undefined) {
                swal("Select Any Field For Search");
                $scope.search_flag = false;
            }
            else {
                $scope.search_flag = true;
                if ($scope.searchstud == "0" || $scope.searchstud == "1" || $scope.searchstud == "2") {
                    

                    $scope.txt = true;
                }
                $scope.searchtxtstud = "";
                $scope.searchdat = "";
                $scope.searchnumbr = "";

            }
        }
        $scope.onselectsearchstaff = function () {
            search_s = $scope.searchstaff;
    
            if (search_s == "" || search_s == undefined) {
                swal("Select Any Field For Search");
                $scope.search_flag_s = false;
            }
            else {
                $scope.search_flag_s = true;
                if ($scope.searchstaff == "0" || $scope.searchstaff == "1" || $scope.searchstaff == "2" || $scope.searchstaff == "3") {
                    $scope.txt_s = true;
                }
                $scope.searchtxtstaff = "";
                $scope.searchdat = "";
                $scope.searchnumbr = "";

            }
        }
        $scope.onselectsearchothers = function () {
            search_s = $scope.searchothers;
           
            if (search_s == "" || search_s == undefined) {
                swal("Select Any Field For Search");
                $scope.search_flag_o = false;
            }
            else {
                $scope.search_flag_o = true;
                if ($scope.searchothers == "0" || $scope.searchothers == "1" || $scope.searchothers == "2") {
                    $scope.txt_o = true;
                }
                $scope.searchtxtothers = "";
            

            }
        }

        $scope.clearsearch_s = function () {
            $scope.searchtxtstaff = "";
            $scope.searchstaff = "";
            $scope.search_flag_s = false;
            $scope.staff_list = temp_staff_list;

        }
        $scope.clearsearch_o = function () {
            $scope.searchtxtothers = "";
            $scope.searchothers = "";
            $scope.search_flag_o = false;
            $scope.student_list = temp_oth_student_list;

        }

        $scope.clearsearch_ss = function () {
            $scope.searchtxt_ss = "";
            $scope.search123_s = "";
            $scope.search_flag1_s = false;
            $scope.thirdgrid_s = temp_grid_staff_list;
           
        }
        $scope.clearsearch_oo = function () {
            $scope.searchtxt_oo = "";
            $scope.search123_o = "";
            $scope.search_flag1_o = false;
            $scope.thirdgrid_o = temp_grid_oth_student_list;
        
        }


        $scope.clearsearch = function () {
       
            $state.reload();
        }

        $scope.ShowSearch_Report = function () {

            if ($scope.searchtxt != "" || $scope.searchnumbr != "" || $scope.searchdat != "") {

                var data = {
                    "searchType": $scope.search123,
                    "searchtext": $scope.searchtxt,
                    "ASMAY_Id": $scope.ASMAY_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("StaffAndOtherFeeGroupMapping/searching", data).
                    then(function (promise) {
                        $scope.thirdgrid = promise.alldatathird;
                        $scope.totcountsearch = promise.alldatathird.length;
                        if (promise.alldatathird == null || promise.alldatathird == "") {
                            swal("Record Does Not Exist For Searched Data !!!!!!")
                        }
                    })
            }
            else {
                swal("Data Is Needed For Search ");
            }
        }
        $scope.ShowSearch_Report_s = function () {

            if ($scope.searchtxt_ss != "") {

                var data = {
                    "searchType": $scope.search123_s,
                    "searchtext": $scope.searchtxt_ss,
                    "ASMAY_Id": $scope.ASMAY_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("StaffAndOtherFeeGroupMapping/searching_s", data).
                    then(function (promise) {
                        $scope.thirdgrid_s = promise.grid_stafflist;
                        $scope.totcountsearch_s = promise.grid_stafflist.length;
                        if (promise.grid_stafflist == null || promise.grid_stafflist == "") {
                            swal("Record Does Not Exist For Searched Data !!!!!!")
                        }
                    })
            }
            else {
                swal("Data Is Needed For Search ");
            }
        }

        $scope.ShowSearch_Report_o = function () {

            if ($scope.searchtxt_oo != "") {

                var data = {
                    "searchType": $scope.search123_o,
                    "searchtext": $scope.searchtxt_o,
                    "ASMAY_Id": $scope.ASMAY_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("StaffAndOtherFeeGroupMapping/searching_o", data).
                    then(function (promise) {
                        $scope.thirdgrid_o = promise.grid_oth_studentlist;
                        $scope.totcountsearch_o = promise.grid_oth_studentlist.length;
                        if (promise.grid_oth_studentlist == null || promise.grid_oth_studentlist == "") {
                            swal("Record Does Not Exist For Searched Data !!!!!!")
                        }
                    })
            }
            else {
                swal("Data Is Needed For Search ");
            }
        }


        $scope.ShowSearchstudent_Report = function () {

            $scope.angularData1 = {
                'nameList1': []
            };

            $scope.vals1 = [];



            if ($scope.searchtxtstud != "" || $scope.searchnumbr != "" || $scope.searchdat != "") {

                if (($scope.checkboxval == "alldata" || $scope.checkboxval == "Regular" || $scope.checkboxval == "NewStude" || $scope.checkboxval == "BR") && ($scope.checkboxvalClasswise == undefined || $scope.checkboxvalClasswise == false)) {

                    if ($scope.trmR_Id != undefined || $scope.trmR_Id != null) {
                        var data = {
                            "searchType": $scope.searchstud,
                            "searchtext": $scope.searchtxtstud,
                            "ASMAY_Id": $scope.ASMAY_Id,
                            "ASMCL_Id": 0,
                            "ASMS_Id": 0,
                            "radioval": $scope.checkboxval,
                            "TRMR_Id": $scope.trmR_Id,
                        }
                    }

                    else {

                        var data = {
                            "searchType": $scope.searchstud,
                            "searchtext": $scope.searchtxtstud,
                            "ASMAY_Id": $scope.ASMAY_Id,
                            "ASMCL_Id": 0,
                            "ASMS_Id": 0,
                            "radioval": $scope.checkboxval
                        }
                    }


                }

                if (($scope.checkboxval == "alldata" || $scope.checkboxval == "Regular" || $scope.checkboxval == "NewStude") && $scope.checkboxvalClasswise == true) {
                    var data = {
                        "searchType": $scope.searchstud,
                        "searchtext": $scope.searchtxtstud,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "ASMS_Id": $scope.sectiondrp,
                        "radioval": $scope.checkboxval

                    }

                    if ($scope.FCC == true) {
                        var data = {
                            "searchType": $scope.searchstud,
                            "searchtext": $scope.searchtxtstud,
                            "ASMAY_Id": $scope.ASMAY_Id,
                            "ASMCL_Id": 0,
                            "ASMS_Id": 0,
                            "radioval": $scope.checkboxval
                        }
                    }
                    if ($scope.AC == true) {
                        var data = {
                            "searchType": $scope.searchstud,
                            "searchtext": $scope.searchtxtstud,
                            "ASMAY_Id": $scope.ASMAY_Id,
                            "ASMCL_Id": 0,
                            "ASMS_Id": 0,
                            "radioval": $scope.checkboxval
                        }
                    }
                }


                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("StaffAndOtherFeeGroupMapping/searchingstudent", data).
                    then(function (promise) {

                        $scope.studentsdata = promise.alldata;
                        

                        if (promise.alldata == null || promise.alldata == "") {
                            swal("Record Does Not Exist For Searched Data !!!!!!")
                        }

                    })
            }
            else {
                swal("Data Is Needed For Search ");
            }
        }
        $scope.ShowSearchstaff_Report = function () {


            if ($scope.searchtxtstaff != "") {

                var data = {
                    "searchType": $scope.searchstaff,
                    "searchtext": $scope.searchtxtstaff,
                    "ASMAY_Id": $scope.ASMAY_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("StaffAndOtherFeeGroupMapping/searchingstaff", data).
                    then(function (promise) {

                        angular.forEach(promise.stafflist, function (stf) {
                            stf.HRME_SaveFlg = false;
                        })
                        // }
                        if (promise.saved_stafflist.length > 0) {
                            angular.forEach(promise.saved_stafflist, function (s_stf) {
                                angular.forEach(promise.stafflist, function (stf) {
                                    if (stf.hrmE_Id == s_stf) {
                                        stf.HRME_SaveFlg = true;
                                    }
                                })
                            })
                        }
                        $scope.staff_list = promise.stafflist;

                        if (promise.stafflist == null || promise.stafflist == "") {
                            swal("Record Does Not Exist For Searched Data !!!!!!")
                        }

                    })
            }
            else {
                swal("Data Is Needed For Search ");
            }
        }

        $scope.ShowSearchothers_Report = function () {

            if ($scope.searchtxtothers != "") {

                var data = {
                    "searchType": $scope.searchothers,
                    "searchtext": $scope.searchtxtothers,
                    "ASMAY_Id": $scope.ASMAY_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("StaffAndOtherFeeGroupMapping/searchingothers", data).
                    then(function (promise) {
                        angular.forEach(promise.oth_studentlist, function (stf) {
                            stf.FMOST_SaveFlg = false;
                        })
                        if (promise.saved_oth_studentlist.length > 0) {
                            angular.forEach(promise.saved_oth_studentlist, function (o_stu) {
                                angular.forEach(promise.oth_studentlist, function (stu) {
                                    if (stu.fmosT_Id == o_stu) {
                                        stu.FMOST_SaveFlg = true;
                                    }
                                })
                            })
                        }
                        $scope.student_list = promise.oth_studentlist;

                        if (promise.oth_studentlist == null || promise.oth_studentlist == "") {
                            swal("Record Does Not Exist For Searched Data !!!!!!")
                        }

                    })
            }
            else {
                swal("Data Is Needed For Search ");
            }
        }


        $scope.selectacademicyear = function (yearlst) {
            $scope.trmA_Id = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

       
            apiService.create("StaffAndOtherFeeGroupMapping/getacademicyear", data).
                then(function (promise) {

                    $scope.studentsdata = promise.alldata;
                    templistdata = $scope.studentsdata;
                    $scope.grouplst = promise.fillmastergroup;
                    $scope.headlst = promise.fillmasterhead;
                    $scope.installlst = promise.fillinstallment;
                    $scope.grouplstedit = promise.fillmastergroup;
                    $scope.headlstedit = promise.fillmasterhead;
                    $scope.installlstedit = promise.fillinstallment;

                    $scope.thirdgrid = promise.alldatathird;
                    $scope.fillbusroutedet = promise.fillbusroutedet;



                    $scope.classdrp = true;
                    $scope.areadrp = true;

                    if ($scope.checkboxval == "BR") {
                        $scope.classdrp = true;
                        $scope.areadrp = true;
                    }
                    if ($scope.checkboxvalAreawise == true) {
                        $scope.classdrp = true;
                        $scope.areadrp = false;
                    }

                })
        };



        $scope.editcheckboxtreeview_s = function (usr) {

            if ($scope.checkboxval == "Staff") {
                var data = {
                    "HRME_Id": usr.hrmE_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "radioval": $scope.checkboxval
                }
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("StaffAndOtherFeeGroupMapping/geteditdatastaffothers", data).

                then(function (promise) {

                    $scope.eeditstudentsdata = promise.alldatathird;

                    if ($scope.eeditstudentsdata.length > 0) {
                        $scope.hrme_idedit = promise.hrmE_Id;
                        angular.forEach($scope.grouplstedit, function (objedit) {
                            objedit.disablegrp = false;
                        });
                        angular.forEach($scope.headlstedit, function (objedit1) {
                            objedit1.disablehead = false;
                        });
                        angular.forEach($scope.installlstedit, function (objedit2) {
                            objedit2.disableins = false;
                        });

                        angular.forEach($scope.eeditstudentsdata, function (grpeditt) {

                            angular.forEach($scope.grouplstedit, function (objedit) {
                                if (grpeditt.fmG_Id == objedit.fmG_Id) {
                                    objedit.checkedgrplstedit = true;
                              
                                    angular.forEach($scope.headlstedit, function (objedit1) {
                                        if (grpeditt.fmG_Id == objedit1.fmG_Id && objedit1.fmH_Id == grpeditt.fmH_Id) {
                                            objedit1.checkedheadlstedit = true;
                                         

                                            angular.forEach($scope.installlstedit, function (objedit2) {
                                                if (grpeditt.fmG_Id == objedit2.fmG_Id && grpeditt.fmH_Id == objedit2.fmH_Id && grpeditt.ftI_Id == objedit2.ftI_Id) {
                                                    objedit2.checkedinstallmentlstedit = true;
                                                    if (grpeditt.fsS_PaidAmount > 0) {
                                                        objedit2.disableins = true;
                                                        objedit1.disablehead = true;
                                                        objedit.disablegrp = true;
                                                    }
                                                }
                                            });
                                        }

                                    });
                                }

                            });
                        });
                    }

                    else {
                        angular.forEach($scope.grouplstedit, function (objedit) {
                            objedit.checkedgrplstedit = false;
                            objedit.disablegrp = false;
                        });
                        angular.forEach($scope.headlstedit, function (objedit1) {
                            objedit1.checkedheadlstedit = false;
                            objedit1.disablehead = false;
                        });
                        angular.forEach($scope.installlstedit, function (objedit2) {
                            objedit2.checkedinstallmentlstedit = false;
                            objedit2.disableins = false;
                        });
                        swal("Student has not mapped with any of the group!")
                        $('#editmodal').modal('hide');
                    }

                })
        }


        $scope.editcheckboxtreeview_o = function (usr) {

            if ($scope.checkboxval == "Others") {
                var data = {
                    "FMOST_Id": usr.fmosT_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "radioval": $scope.checkboxval
                }
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("StaffAndOtherFeeGroupMapping/geteditdatastaffothers", data).

                then(function (promise) {

                    $scope.eeditstudentsdata = promise.alldatathird;

                    if ($scope.eeditstudentsdata.length > 0) {
                        $scope.fmost_idedit = promise.fmosT_Id;
                        angular.forEach($scope.grouplstedit, function (objedit) {
                            objedit.disablegrp = false;
                        });
                        angular.forEach($scope.headlstedit, function (objedit1) {
                            objedit1.disablehead = false;
                        });
                        angular.forEach($scope.installlstedit, function (objedit2) {
                            objedit2.disableins = false;
                        });

                        angular.forEach($scope.eeditstudentsdata, function (grpeditt) {

                            angular.forEach($scope.grouplstedit, function (objedit) {
                                if (grpeditt.fmG_Id == objedit.fmG_Id) {
                                    objedit.checkedgrplstedit = true;
                              
                                    angular.forEach($scope.headlstedit, function (objedit1) {
                                        if (grpeditt.fmG_Id == objedit1.fmG_Id && objedit1.fmH_Id == grpeditt.fmH_Id) {
                                            objedit1.checkedheadlstedit = true;
                                      

                                            angular.forEach($scope.installlstedit, function (objedit2) {
                                                if (grpeditt.fmG_Id == objedit2.fmG_Id && grpeditt.fmH_Id == objedit2.fmH_Id && grpeditt.ftI_Id == objedit2.ftI_Id) {
                                                    objedit2.checkedinstallmentlstedit = true;
                                                    if (grpeditt.fsS_PaidAmount > 0) {
                                                        objedit2.disableins = true;
                                                        objedit1.disablehead = true;
                                                        objedit.disablegrp = true;
                                                    }
                                                }
                                            });
                                        }

                                    });
                                }

                            });
                        });
                    }


                    else {
                        angular.forEach($scope.grouplstedit, function (objedit) {
                            objedit.checkedgrplstedit = false;
                            objedit.disablegrp = false;
                        });
                        angular.forEach($scope.headlstedit, function (objedit1) {
                            objedit1.checkedheadlstedit = false;
                            objedit1.disablehead = false;
                        });
                        angular.forEach($scope.installlstedit, function (objedit2) {
                            objedit2.checkedinstallmentlstedit = false;
                            objedit2.disableins = false;
                        });
                        swal("Student has not mapped with any of the group!")
                        $('#editmodal').modal('hide');
                    }



                })
        }



        $scope.saveeditdatastaff = function (hrme_idedit, grouplstedit, headlstedit, installlstedit) {

            var data = {
                
                "HRME_Id": hrme_idedit,
                
                savegrplst: grouplstedit,
                saveheadlst: headlstedit,
                saveftilst: installlstedit,
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("StaffAndOtherFeeGroupMapping/saveeditdatastaff", data).
                then(function (promise) {


                    if (promise.returnval == "false") {
                        swal("You are missing Amount entry/Due Date/Fine Slab Settings/Category Mapping.")
                    }
                    else if (promise.returnval == "true") {
                        swal('Record Updated Successfully');
                    }
                    else if (promise.returnval == "Error") {
                        swal('Kindly contact Administrator ');
                    }

                    //$scope.formload();
                    $scope.cllose();
                    $state.reload();

                });

        }



        $scope.saveeditdataothers = function (fmost_idedit, grouplstedit, headlstedit, installlstedit) {

            var data = {
              
                "FMOST_Id": fmost_idedit,
           
                savegrplst: grouplstedit,
                saveheadlst: headlstedit,
                saveftilst: installlstedit,
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("StaffAndOtherFeeGroupMapping/saveeditdataothers", data).
                then(function (promise) {


                    if (promise.returnval == "false") {
                        swal("You are missing Amount entry/Due Date/Fine Slab Settings/Category Mapping.")
                    }
                    else if (promise.returnval == "true") {
                        swal('Record Updated Successfully');
                    }
                    else if (promise.returnval == "Error") {
                        swal('Kindly contact Administrator ');
                    }

                  
                    $scope.cllose();
                    $state.reload();

                });

        }



    }

})();