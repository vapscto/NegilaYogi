(function () {
    'use strict';
    angular
.module('app')
.controller('StaffFeeGroupMapping', StaffFeeGroupMapping)

    StaffFeeGroupMapping.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$stateParams']
    function StaffFeeGroupMapping($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache,$stateParams) {

        $scope.busroutedisable = true;
        $scope.areadisable = true;

        $scope.eeditstudentsdata = [];

        $scope.angularData1 = {
            'nameList1': []
        };

        $scope.angularData = {
            'nameList': []
        };

        $scope.vals1 = [];

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.userPrivileges = [];
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.disabled = true;
                if (privlgs[i].ivrmirP_AddFlag == true) {
                    $scope.save = true;
                    $scope.savebtn = true;
                    $scope.savedisable = true;
                }
                else {
                    $scope.save = false;
                    $scope.savebtn = false;

                }
                if (privlgs[i].ivrmirP_UpdateFlag == true) {
                    $scope.editflag = true;
                    $scope.editbtn = true;
                    $scope.editdisable = true;

                }
                else {
                    $scope.editflag = false;
                    $scope.editbtn = false;

                }
                if (privlgs[i].ivrmirP_DeleteFlag == true) {
                    $scope.deactiveflag = true;
                    $scope.deletebtn = true;
                    $scope.deletedisable = true;
                }
                else {
                    $scope.deactiveflag = false;
                    $scope.deletebtn = false;

                }


            }
        }

        $scope.sortKey1 = "fmsG_Id";   //set the sortKey to the param passed
        $scope.reverse1 = true; //if true make it false and vice versa
        $scope.totcountsearch = 0;

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
                //angular.forEach($scope.grouplst, function (val) {
                //    if (vlobj1.fmG_Id == val.fmG_Id) {
                //        angular.forEach($scope.headlst, function (val1) {
                //            if (val1.fmG_Id == val.fmG_Id) {
                //                val1.checkedheadlst = true;
                //                angular.forEach($scope.installlst, function (val2) {
                //                    if (val.fmG_Id == val2.fmG_Id && val1.fmH_Id == val2.fmH_Id) {
                //                        val2.checkedinstallmentlst = true;
                //                    }
                //                });
                //            }
                //        });
                //    }
                //});

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




        //$scope.firstfnc = function (vlobj) {
        //    
        //    if (vlobj.checkedgrplst == true) {
        //        angular.forEach($scope.grouplst, function (obj) {
        //            if (vlobj.fmG_Id == obj.fmG_Id) {
        //                angular.forEach($scope.headlst, function (obj1) {
        //                    if (obj1.fmG_Id == obj.fmG_Id) {
        //                        obj1.checkedheadlst = true;
        //                        angular.forEach($scope.installlst, function (obj2) {
        //                            if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
        //                                obj2.checkedinstallmentlst = true;
        //                            }
        //                        });
        //                    }
        //                });
        //            }
        //        });
        //    } else {
        //        angular.forEach($scope.grouplst, function (obj) {
        //            if (vlobj.fmG_Id == obj.fmG_Id) {
        //                angular.forEach($scope.headlst, function (obj1) {
        //                    if (obj1.fmG_Id == obj.fmG_Id) {
        //                        obj1.checkedheadlst = false;
        //                        angular.forEach($scope.installlst, function (obj2) {
        //                            if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
        //                                obj2.checkedinstallmentlst = false;
        //                            }
        //                        });
        //                    }
        //                });
        //            }
        //        });
        //    }
        //}
        //$scope.secfnc = function (vlobj1) {
        //    
        //    if (vlobj1.checkedheadlst == true) {
             

        //        angular.forEach($scope.headlst, function (val) {
        //            if (vlobj1.fmG_Id == val.fmG_Id && vlobj1.fmH_Id == val.fmH_Id) {
        //                angular.forEach($scope.installlst, function (val1) {

        //                    if (val1.fmH_Id == val.fmH_Id && val1.fmG_Id == val.fmG_Id) {
        //                        val1.checkedinstallmentlst = true;
        //                    }

        //                });
        //            }
        //        });
        //    } else {
        //        angular.forEach($scope.headlst, function (val) {
        //            if (vlobj1.fmG_Id == val.fmG_Id && vlobj1.fmH_Id == val.fmH_Id) {
        //                angular.forEach($scope.installlst, function (val1) {
        //                    if (val1.fmH_Id == val.fmH_Id && val1.fmG_Id == val.fmG_Id) {
        //                        val1.checkedinstallmentlst = false;
        //                    }
        //                });
        //            }
        //        });
        //    }
        //}
        //$scope.trdfnc = function (vlobj2) {
        //    
        //    if (vlobj2.checkedinstallmentlst == true) {

        //    }
        //    else {
        //        angular.forEach($scope.installlst, function (subobj) {
        //            if (vlobj2.fmG_Id == subobj.fmG_Id) {
        //                obj2.checkedinstallmentlst = true;
        //            }
        //        });
        //    }
        //}

        //checkbox

        //$(function () {

        //    $('input[type="checkbox"]').change(checkboxChanged);

        //    function checkboxChanged() {
        //        var $this = $(this),
        //            checked = $this.prop("checked"),
        //            container = $this.parent(),
        //            siblings = container.siblings();

        //        container.find('input[type="checkbox"]')
        //        .prop({
        //            indeterminate: false,
        //            checked: checked
        //        })
        //        .siblings('label')
        //        .removeClass('custom-checked custom-unchecked custom-indeterminate')
        //        .addClass(checked ? 'custom-checked' : 'custom-unchecked');

        //        checkSiblings(container, checked);
        //    }

        //    function checkSiblings($el, checked) {
        //        var parent = $el.parent().parent(),
        //            all = true,
        //            indeterminate = false;

        //        $el.siblings().each(function () {
        //            return all = ($(this).children('input[type="checkbox"]').prop("checked") === checked);
        //        });

        //        if (all && checked) {
        //            parent.children('input[type="checkbox"]')
        //            .prop({
        //                indeterminate: false,
        //                checked: checked
        //            })
        //            .siblings('label')
        //            .removeClass('custom-checked custom-unchecked custom-indeterminate')
        //            .addClass(checked ? 'custom-checked' : 'custom-unchecked');

        //            checkSiblings(parent, checked);
        //        }
        //        else if (all && !checked) {
        //            indeterminate = parent.find('input[type="checkbox"]:checked').length > 0;

        //            parent.children('input[type="checkbox"]')
        //            .prop("checked", checked)
        //            .prop("indeterminate", indeterminate)
        //            .siblings('label')
        //            .removeClass('custom-checked custom-unchecked custom-indeterminate')
        //            .addClass(indeterminate ? 'custom-indeterminate' : (checked ? 'custom-checked' : 'custom-unchecked'));

        //            checkSiblings(parent, checked);
        //        }
        //        else {
        //            $el.parents("li").children('input[type="checkbox"]')
        //            .prop({
        //                indeterminate: true,
        //                checked: false
        //            })
        //            .siblings('label')
        //            .removeClass('custom-checked custom-unchecked custom-indeterminate')
        //            .addClass('custom-indeterminate');
        //        }
        //    }
        //});

        //checkbox
        $scope.toggleAll = function (allchkdata) {
            var toggleStatus = $scope.selectedAll;
            angular.forEach($scope.studentsdata, function (itm) {
                itm.studchecked = toggleStatus;
            });
        }


        $scope.searchvalue1 = '';
        $scope.filtervalue1 = function (obj) {
            return angular.lowercase(obj.fmG_GroupName).indexOf(angular.lowercase($scope.searchvalue1)) >= 0 || angular.lowercase(obj.asmcL_ClassName).indexOf(angular.lowercase($scope.searchvalue1)) >= 0 || angular.lowercase(obj.amsT_FirstName + ' ' + obj.amsT_MiddleName + ' ' + obj.amsT_LastName).indexOf(angular.lowercase($scope.searchvalue1)) >= 0;
        }

        //$scope.searchvalue = '';

        //$scope.filtervalue = function (obj) {
        //    return (angular.lowercase(obj.amsT_RegistrationNo)).indexOf(angular.lowercase($scope.searchvalue)) >= 0 || (obj.amsT_AdmNo).indexOf($scope.searchvalue) >= 0 || JSON.stringify(obj.amaY_RollNo).indexOf($scope.searchvalue) >= 0 || angular.lowercase(obj.amsT_FirstName + ' ' + obj.amsT_MiddleName + ' ' + obj.amsT_LastName).indexOf(angular.lowercase($scope.searchvalue)) >= 0;
        //}


        //$scope.filtervalue = function (obj) {
        //    return (angular.lowercase(obj.amsT_RegistrationNo)).indexOf(angular.lowercase($scope.searchvalue)) >= 0 || (obj.amsT_AdmNo).indexOf(angular.lowercase($scope.searchvalue)) >= 0 || JSON.stringify(obj.amaY_RollNo).indexOf($scope.searchvalue) >= 0 || angular.lowercase(obj.namee).indexOf(angular.lowercase($scope.searchvalue)) >= 0;
        //}

        $scope.isOptionsRequired = function () {
            return !$scope.studentsdata.some(function (options) {
                return options.studchecked;
            });
        }
        $scope.isOptionsRequired1 = function () {
            return !$scope.grouplst.some(function (options) {
                return options.checkedgrplst;
            });
        }

        $scope.Clearid = function () {
            $state.reload();
        }

        $scope.editcheckboxtreeview = function (usr) {

            var data = {
                "AMST_Id": usr.amsT_Id,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("StaffFeeGroupMapping/geteditdata", data).

            then(function (promise) {
                
                $scope.eeditstudentsdata = promise.alldatathird;
                if ($scope.eeditstudentsdata.length > 0) {
                    $scope.AMST_idedit = promise.amsT_Id;
                    angular.forEach($scope.eeditstudentsdata, function (grpeditt) {
                        angular.forEach($scope.grouplstedit, function (objedit) {
                            if (grpeditt.fmG_Id == objedit.fmG_Id) {
                                objedit.checkedgrplstedit = true;
                                if (grpeditt.fsS_PaidAmount > 0) {
                                    objedit.disablegrp = true;
                                }

                                //objedit.disablegrp = true;
                                angular.forEach($scope.headlstedit, function (objedit1) {
                                    if (grpeditt.fmG_Id == objedit1.fmG_Id && objedit1.fmH_Id == grpeditt.fmH_Id) {
                                        objedit1.checkedheadlstedit = true;
                                        if (grpeditt.fsS_PaidAmount > 0) {
                                            objedit1.disablehead = true;
                                        }
                                        angular.forEach($scope.installlstedit, function (objedit2) {
                                            if (grpeditt.fmG_Id == objedit2.fmG_Id && grpeditt.fmH_Id == objedit2.fmH_Id && grpeditt.ftI_Id == objedit2.ftI_Id) {
                                                objedit2.checkedinstallmentlstedit = true;
                                                if (grpeditt.fsS_PaidAmount > 0) {
                                                    objedit2.disableins = true;
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
                    });
                    angular.forEach($scope.headlstedit, function (objedit1) {
                        objedit1.checkedheadlstedit = false;
                    });
                    angular.forEach($scope.installlstedit, function (objedit2) {
                        objedit2.checkedinstallmentlstedit = false;
                    });
                    swal("Student has not mapped with any of the group!")
                    $('#editmodal').modal('hide');
                }

            })
        }

        $scope.page1 = "page1";
        $scope.page2 = "page2";

        $scope.reverse1 = true;
        $scope.reverse2 = true;
        $scope.angularData = {
            'nameList': []
        };

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
            }

        $scope.vals = [];
        $scope.formload = function () {
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = paginationformasters

            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = paginationformasters

            var pageid = 1;

            apiService.getURI("StaffFeeGroupMapping/getalldetails", pageid).
        then(function (promise) {

            $scope.staffdata = promise.alldata;

            for (var i = 0; i < promise.alldata.length; i++) {
                var name = promise.alldata[i].hrmE_EmployeeFirstName;
                if (promise.alldata[i].hrmE_EmployeeMiddleName !== null) {
                    name += " " + promise.alldata[i].hrmE_EmployeeMiddleName;
                }
                if (promise.alldata[i].hrmE_EmployeeLastName != null) {
                    name += " " + promise.alldata[i].hrmE_EmployeeLastName;
                }
                $scope.vals.push(name);
            }
            angular.forEach($scope.vals, function (v, k) {
                $scope.angularData.nameList.push({
                    'fullname': v
                });
            });

            var j = 0;
            angular.forEach($scope.staffdata, function (obj) {
                //Using bracket notation
                obj["fullname"] = $scope.angularData.nameList[j].fullname;
                j++;
            });
            angular.forEach($scope.staffdata, function (objectt) {
                if (objectt.fullname.length > 0) {
                    var string = objectt.fullname;
                    objectt.namee = string.replace(/  +/g, ' ');
                }
            })

            $scope.grouplst = promise.fillmastergroup;
            $scope.headlst = promise.fillmasterhead;
            $scope.installlst = promise.fillinstallment;
            $scope.grouplstedit = promise.fillmastergroup;
            $scope.headlstedit = promise.fillmasterhead;
            $scope.installlstedit = promise.fillinstallment;

            $scope.thirdgrid = promise.alldatathird;

            $scope.totcountfirst = promise.alldatathirdall.length;

            $scope.classdrp = true;
            $scope.areadrp = true;
        })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

            $scope.sort1 = function (keyname1) {
                $scope.sortKey1 = keyname1;   //set the sortKey to the param passed
                $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
            }
        }

        $scope.DeletRecord = function (studentmappingid, studentid, groupid) {

            var studmapid = studentmappingid;
            var studentid = studentid;

            var data = {
                "FMSG_Id": studmapid,
                "AMST_Id": studentid,
                "FMG_Id": groupid
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
                   apiService.create("StaffFeeGroupMapping/Deletedetails", data).
                   then(function (promise) {

                       // $scope.thirdgrid = promise.alldata;

                       if (promise.returnval == "true") {

                           //$scope.masterse = promise.masterSectionData;

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

        $scope.fillstudents = function (classcount) {

            $scope.angularData1 = {
                'nameList1': []
            };

            $scope.vals1 = [];

            var classid = $scope.ASMCL_Id;
            var radiobtnvalue = $scope.checkboxval;
            var classcheckedvalue = $scope.checkboxvalClasswise;

            var data = {
                "ASMCL_Id":classid,
                "radioval": $scope.checkboxval,
                "classwisecheckboxvalue": classcheckedvalue
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("StaffFeeGroupMapping/getgroupmappedheads", data).
       then(function (promise) {
           if (promise.returnval == "Yes") {
               $scope.studentsdata = promise.alldata;

               for (var i = 0; i < promise.alldata.length; i++) {
                var name = promise.alldata[i].amsT_FirstName;
                if (promise.alldata[i].amsT_MiddleName !== null) {
                    name += " " +promise.alldata[i].amsT_MiddleName;
                    }
                if (promise.alldata[i].amsT_LastName != null) {
                    name += " " + promise.alldata[i].amsT_LastName;
                }
                $scope.vals1.push(name);
                }
            angular.forEach($scope.vals1, function (v, k) {
                $scope.angularData1.nameList1.push({
                    'fullname' : v
                });
            });

            var j = 0;
            angular.forEach($scope.studentsdata, function (obj) {
                //Using bracket notation
                obj["fullname"]= $scope.angularData1.nameList1[j].fullname;
                j++;
                });
            angular.forEach($scope.studentsdata, function (objectt) {
                if (objectt.fullname.length > 0) {
                    var string = objectt.fullname;
                    objectt.namee = string.replace(/  +/g, ' ');
                }
            })
       }
            else
           {
               $scope.studentsdata = {};
                swal("No Records Found!");
           }
          // $scope.thirdgrid = promise.alldatathird;
       })
        };

        $scope.feeclasscatdrp = true;
        $scope.admissionclscatdrp = true;
        $scope.busroutedrp = true;
        $scope.classdrp = true;
        $scope.areadrp = true;

        $scope.onclickloaddata = function () {

            if ($scope.checkboxval == "alldata") {
                $scope.feeclasscatdrp = true;
                $scope.admissionclscatdrp = true;
                $scope.busroutedrp = true;
                $scope.classdrp = true;
                $scope.areadrp = true;
            }
            else if ($scope.checkboxval == "FCC") {
                $scope.feeclasscatdrp = false;
                $scope.admissionclscatdrp = true;
                $scope.busroutedrp = true;
                $scope.classdrp = true;
                $scope.areadrp = true;
            }
            else if ($scope.checkboxval == "AC") {
                $scope.admissionclscatdrp = false;
                $scope.feeclasscatdrp = true;
                $scope.busroutedrp = true;
                $scope.classdrp = true;
                $scope.areadrp = true;
            }
            else if ($scope.checkboxval == "BR") {
                $scope.busroutedrp = false;
                $scope.admissionclscatdrp = true;
                $scope.feeclasscatdrp = true;
                $scope.classdrp = false;
                $scope.areadrp = true;
            }
            else if ($scope.checkboxval == "Regular") {
                $scope.busroutedrp = true;
                $scope.admissionclscatdrp = true;
                $scope.feeclasscatdrp = true;
                $scope.classdrp = true;
                $scope.areadrp = true;
            }

            else if ($scope.checkboxval == "NewStude") {
                $scope.busroutedrp = true;
                $scope.admissionclscatdrp = true;
                $scope.feeclasscatdrp = true;
                $scope.classdrp = true;
                $scope.areadrp = true;
            }

            if ($scope.checkboxvalClasswise == true) {

                $scope.classdrp = false;
                $scope.busroutedrp = true;
                $scope.admissionclscatdrp = true;
                $scope.feeclasscatdrp = true;
                $scope.areadrp = true;


            }
            else if ($scope.checkboxvalAreawise == true) {
                $scope.areadrp = false;
                $scope.classdrp = true;
                $scope.busroutedrp = true;
                $scope.admissionclscatdrp = true;
                $scope.feeclasscatdrp = true;

                $scope.checkboxvalClasswise == false;
                $scope.checkboxvalRegular == false;
                $scope.checkboxvalNewStude == false;
            }
            

            var data = {
                "radioval": $scope.checkboxval
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("StaffFeeGroupMapping/radiobtndata", data).
        then(function (promise) {
            //$scope.FMG_Id = promise.alldata[0].fmG_Id;
          
                if (promise.radioval == "FCC") {
                    $scope.feecatcount = promise.fillfeeclasscategory;
                }
                else if (promise.radioval == "AC") {
                    $scope.admissioncatcount = promise.filladmissionclasscategory;
                }

                else if (promise.radioval == "Regular") {
                    $scope.classcount = promise.fillmasterclass;
                }

                else if (promise.radioval == "NewStude") {
                    $scope.classcount = promise.fillmasterclass;
                }
            
        })

        };

        $scope.arr = [];
        var amstid;
        $scope.onstudentclick = function (amstid) {
            $scope.selectedAll = $scope.studentsdata.every(function (itm)
            { return itm.studchecked; });
            var data = {
                "AMST_Id": amstid
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("StaffFeeGroupMapping/studentsavedgroup", data).
           then(function (promise) {
               //$scope.grouplst = promise.fillmappedgroupforstudents;
               //$scope.headlst = promise.fillmappedgroupforstudents; 
               //$scope.installlst = promise.fillmappedgroupforstudents;
           })
        }


        $scope.onadmcatclick = function (catid) {

            $scope.angularData1 = {
                'nameList1': []
            };

            $scope.vals1 = [];

            var admcat = "AC";
            // catid = $scope.amC_Id;
            var data = {
                "AMC_Id": catid,
                "radioval": admcat
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("StaffFeeGroupMapping/getclassoncatselect", data).
           then(function (promise) {
               if (promise.fillfeeclasscategory.length > 0)
               {
                   //$scope.classcount = promise.fillfeeclasscategory;
                   $scope.studentsdata = promise.fillfeeclasscategory;

                   for (var i = 0; i < promise.fillfeeclasscategory.length; i++) {
                var name = promise.fillfeeclasscategory[i].amsT_FirstName;
                if (promise.fillfeeclasscategory[i].amsT_MiddleName !== null) {
                    name += " " + promise.fillfeeclasscategory[i].amsT_MiddleName;
                }
                if (promise.fillfeeclasscategory[i].amsT_LastName != null) {
                    name += " " +promise.fillfeeclasscategory[i].amsT_LastName;
                    }
                    $scope.vals1.push(name);
                    }
            angular.forEach($scope.vals1, function(v, k) {
                $scope.angularData1.nameList1.push({
                    'fullname': v
                });
                });

            var j = 0;
            angular.forEach($scope.studentsdata, function (obj) {
                    //Using bracket notation
                obj["fullname"] = $scope.angularData1.nameList1[j].fullname;
                j++;
                });
            angular.forEach($scope.studentsdata, function (objectt) {
                if (objectt.fullname.length > 0) {
                    var string = objectt.fullname;
                    objectt.namee = string.replace(/  +/g, ' ');
                }
                })


               }
               else
               {
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
                "radioval": feecat
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("StaffFeeGroupMapping/getclassoncatselect", data).
                then(function (promise) {
                    if (promise.fillfeeclasscategory.length > 0)
                    {
                        //$scope.classcount = promise.fillfeeclasscategory;
                        $scope.studentsdata = promise.fillfeeclasscategory;

                        for (var i = 0; i < promise.fillfeeclasscategory.length; i++) {
                            var name = promise.fillfeeclasscategory[i].amsT_FirstName;
                            if (promise.fillfeeclasscategory[i].amsT_MiddleName !== null) {
                                name += " " +promise.fillfeeclasscategory[i].amsT_MiddleName;
                            }
                            if (promise.fillfeeclasscategory[i].amsT_LastName != null) {
                                name += " " +promise.fillfeeclasscategory[i].amsT_LastName;
                            }
                            $scope.vals1.push(name);
                        }
                        angular.forEach($scope.vals1, function (v, k) {
                            $scope.angularData1.nameList1.push({
                                'fullname': v
                            });
                        });

                        var j = 0;
                        angular.forEach($scope.studentsdata, function (obj) {
                            //Using bracket notation
                            obj["fullname"] = $scope.angularData1.nameList1[j].fullname;
                            j++;
                        });
                        angular.forEach($scope.studentsdata, function (objectt) {
                            if (objectt.fullname.length > 0) {
                                var string = objectt.fullname;
                                objectt.namee = string.replace(/  +/g, ' ');
                            }
                        })


                    }
                    else
                    {
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
            //orgid = 12;
            apiService.getURI("StaffFeeGroupMapping/Editdetails", orgid).
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
                "FMH_FeeName": $scope.typethird
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("StaffFeeGroupMapping/1", data).
        then(function (promise) {
            $scope.thirdgrid = promise.alldata;
            swal("searched Successfully");
        })
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //$scope.saveeditdata=function()
        //{
        //    var data = {
        //        "FMG_Id": $scope.FMG_Id,
        //        studentdata: studentsdata,
        //        savegrplst: grouplst,
        //        saveheadlst: headlst,
        //        saveftilst: installlst,
        //    }

        //    var config = {
        //        headers: {
        //            'Content-Type': 'application/json;'
        //        }
        //    }
        //}
        $scope.saveeditdata = function (AMST_idedit, grouplstedit, headlstedit, installlstedit) {
            

            var data = {
                // "FMG_Id": $scope.FMG_Id,
                "AMST_Id": AMST_idedit,
                // studentdata: studentsdata,
                savegrplst: grouplstedit,
                saveheadlst: headlstedit,
                saveftilst: installlstedit,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("StaffFeeGroupMapping/saveeditdata", data).
                then(function (promise) {
                    

                    if (promise.returnval == "false") {
                        swal("You are missing Amount entry/Due Date/Fine Slab Settings.")
                    }
                    else if (promise.returnval == "true") {
                        swal('Record Updated Successfully');
                    }
                    else if (promise.returnval == "Error") {
                        swal('Kindly contact Administrator ');
                    }

                    //$scope.addnewbtn = true;
                    //$scope.thirdgrid = promise.alldata;
                    $state.reload();
                });

        }
        $scope.submitted = false;
        $scope.valsgroup = [];
        $scope.valshead = [];
        $scope.valsinstallment = [];
        $scope.valstafflst = [];
        $scope.savedata = function (staffdata, grouplst, headlst, installlst) {
           
            if ($scope.myForm.$valid) {

                for (var t = 0; t < grouplst.length; t++){
                    if(grouplst[t].checkedgrplst==true){
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

                for (var w = 0; w < staffdata.length; w++) {
                    if (staffdata[w].staffchecked == true) {
                        $scope.valstafflst.push(staffdata[w]);
                    }
                }

                staffdata = $scope.valstafflst;
                grouplst = $scope.valsgroup;
                headlst = $scope.valshead;
                installlst = $scope.valsinstallment;

                var data = {
                    "FMG_Id": $scope.FMG_Id,
                    studentdata: staffdata,
                    savegrplst: grouplst,
                    saveheadlst: headlst,
                    saveftilst: installlst,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("StaffFeeGroupMapping/", data).
                then(function (promise) {

                    if (promise.returnval == "false") {
                        swal("You are missing Amount entry/Due Date/Fine Slab Settings.")
                    }
                    else if (promise.returnval == "duplicate") {
                        swal('Group is already saved for the student');
                    }
                    else if (promise.returnval == "true") {
                        swal('Record Saved/Updated Successfully');
                    }
                    else if (promise.returnval == "Error") {
                        swal('Kindly contact Administrator ');
                    }

                    $scope.addnewbtn = true;
                   // $scope.thirdgrid = promise.alldata;
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }

        };


        $scope.search_flag = false;
        $scope.search123 = "";
        var search_s = "";
        var list_s = [];
        $scope.onselectsearch = function () {
            search_s = $scope.search123;
            //list_s = $scope.receiptgrid;
            if (search_s == "" || search_s == undefined) {
                swal("Select Any Field For Search");
                $scope.search_flag = false;
            }
            else {
                $scope.search_flag = true;
                if ($scope.search123 == "0" || $scope.search123 == "1" || $scope.search123 == "2" || $scope.search123 == "3") {
                    $scope.txt = true;
                }
                $scope.searchtxt = "";
                $scope.searchdat = "";
                $scope.searchnumbr = "";

            }
        }


        $scope.onselectsearchstudent = function () {
            search_s = $scope.searchstud;
            //list_s = $scope.receiptgrid;
            if (search_s == "" || search_s == undefined) {
                swal("Select Any Field For Search");
                $scope.search_flag = false;
            }
            else {
                $scope.search_flag = true;
                if ($scope.searchstud == "0" || $scope.searchstud == "1" || $scope.searchstud == "2") {
                    $scope.txt = true;
                }
                $scope.searchtxt = "";
                $scope.searchdat = "";
                $scope.searchnumbr = "";

            }
        }


        $scope.clearsearch = function () {
            //$scope.search123 = "";
            //$scope.search_flag = false;
            //$scope.searchtxt = "";
            //$scope.searchstud="';"
            $state.reload();
        }

        $scope.ShowSearch_Report = function () {

            if ($scope.searchtxt != "" || $scope.searchnumbr != "" || $scope.searchdat != "") {
              
                    var data = {
                        "searchType": $scope.search123,
                        "searchtext": $scope.searchtxt,
                    }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("StaffFeeGroupMapping/searching", data).
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

       

        $scope.ShowSearchstudent_Report = function () {

            $scope.angularData1 = {
                'nameList1': []
            };

            $scope.vals1 = [];

            if ($scope.searchtxtstud != "" || $scope.searchnumbr != "" || $scope.searchdat != "") {

                var data = {
                    "searchType": $scope.searchstud,
                    "searchtext": $scope.searchtxtstud,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("StaffFeeGroupMapping/searchingstudent", data).
            then(function (promise) {

                $scope.staffdata = promise.alldata;

                for (var i = 0; i < promise.alldata.length; i++) {
                    var name = promise.alldata[i].hrmE_EmployeeFirstName;
                    if (promise.alldata[i].hrmE_EmployeeMiddleName !== null) {
                        name += " " + promise.alldata[i].hrmE_EmployeeMiddleName;
                    }
                    if (promise.alldata[i].hrmE_EmployeeLastName != null) {
                        name += " " + promise.alldata[i].hrmE_EmployeeLastName;
                    }
                    $scope.vals1.push(name);
                }
                angular.forEach($scope.vals1, function (v, k) {
                    $scope.angularData1.nameList1.push({
                        'fullname': v
                    });
                });

                var j = 0;
                angular.forEach($scope.staffdata, function (obj) {
                    //Using bracket notation
                    obj["fullname"] = $scope.angularData1.nameList1[j].fullname;
                    j++;
                });
                angular.forEach($scope.staffdata, function (objectt) {
                    if (objectt.fullname.length > 0) {
                        var string = objectt.fullname;
                        objectt.namee = string.replace(/  +/g, ' ');
                    }
                })

                if (promise.alldata == null || promise.alldata == "") {
                    swal("Record Does Not Exist For Searched Data !!!!!!")
                }

            })
            }
            else {
                swal("Data Is Needed For Search ");
            }
        }


    }

})();