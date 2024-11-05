(function () {
    'use strict';
    angular
.module('app')
.controller('InstitutionRoleModulePreviledgeController', InstitutionRoleModulePreviledgeController)
    InstitutionRoleModulePreviledgeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', 'superCache']
    function InstitutionRoleModulePreviledgeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, superCache) {

        var HostName = location.host;

        $scope.Previous = function () {
            $window.location.href = 'http://' + HostName + '/#/app/ConSetting/';
        };

        $scope.Next = function () {
            $window.location.href = 'http://' + HostName + '/#/app/MasterMainMenuINS/';
        };

        $scope.Finish = function () {
            $window.location.href = 'http://' + HostName + '/#/app/ConSettingWizardComplete/';
        };

        $scope.showdelete = false;
        $scope.secondgrid = [];
        $scope.page1 = "pag1";
        $scope.page2 = "pag2";
        $scope.page3 = "pag3";
        $scope.page4 = "pag4";
        $scope.page5 = "pag5";
        $scope.currentPage1 = 1;
        $scope.currentPage2 = 1;
        $scope.currentPage3 = 1;
        $scope.currentPage4 = 1;
        $scope.itemsPerPage1 = 10;
        $scope.itemsPerPage2 = 10;
        $scope.itemsPerPage3 = 10;
        $scope.itemsPerPage4 = 10;
        $scope.currentPage5 = 1;
        $scope.itemsPerPage5 = 10;

        $scope.onchangeInstitute = function () {
            var pageid = 2;
            apiService.getURI("MasterInstitutionRoleandModuleMapping/getModuleDropdownList", pageid).
        then(function (promise) {
            $scope.moduleDropDown = promise.moduleDropDown;
        })
        }



        $scope.details = function () {

            $scope.reverse1 = true;
            $scope.reverse2 = true;
            $scope.reverse3 = true;
            $scope.reverse4 = true;
            $scope.reverse5 = true;



            $scope.pagedata = true;

            var pageid = 2;
            apiService.getURI("MasterInstitutionRoleandModuleMapping/getalldetails", pageid).
        then(function (promise) {

            $scope.institutionDropDown = promise.institutionDropDown;
            // $scope.roleDropDown = promise.roleDropDown;
            // $scope.moduleDropDown = promise.moduleDropDown;
            $scope.pagelist = promise.pageDropDown; // All pages list

            $scope.institutionRolePrivilegesList = promise.institutionRolePrivilegesList;

            $scope.roleDropDown = promise.roleDropDown;
            $scope.moduleDropDown = promise.moduleDropDown;
        })
            // for pagelist
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

            // for institutionRolePrivilegesList
            $scope.sort1 = function (keyname) {
                $scope.sortKey1 = keyname;   //set the sortKey to the param passed
                $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
            }




        }

        $scope.clearForm = function () {
            $state.reload();
        }


        $scope.getpagesname = function (IVRMM_Id) {

            apiService.getURI("MasterInstitutionRoleandModuleMapping/getPagedetailsByModuleId", IVRMM_Id).
        then(function (promise) {
            $scope.roleDropDown = promise.roleList;
            $scope.pagelist = promise.thirdgriddata; //all pages based on module
        })
        }

        $scope.getPagedetailsRoleType = function (id) {

            $scope.SeletedPageDiv = false;
            $scope.secondgrid = [];

            var data = {
                "IVRMRT_Id": id,
                "IVRMM_Id": $scope.IVRMM_Id,
                "MI_Id": $scope.MI_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            $scope.pagedata = false;
            apiService.create("MasterInstitutionRoleandModuleMapping/getPagedetailsByRoleType", data).
        then(function (promise) {
            if (promise.privilegesList != null && promise.privilegesList.length > 0) {

                $scope.MasterPageDiv = true;
                $scope.gridview1 = true;
                $scope.privilegesList = promise.privilegesList;

                $scope.selectedAll = true;
                $scope.checkAll();
            }
            if (promise.institutionRolePrivilegesDTO.institutionRolePrivilegesList.length > 0 && promise.institutionRolePrivilegesDTO.institutionRolePrivilegesList != null) {
                $scope.SeletedPageDiv = true;
                $scope.secondgrid = promise.institutionRolePrivilegesDTO.institutionRolePrivilegesList;
            }
            else {
                $scope.MasterPageDiv = false;
            }
        })
        }

        $scope.Toggle_Addsave = function () {
            
            var toggleStatus2 = $scope.Addsaveall2;
            angular.forEach($scope.secondgrid, function (itm) {
                itm.ivrmrP_AddFlag = toggleStatus2;
            });
        }
        $scope.Toggle_addsavegrd = function () {
            
            $scope.Addsaveall2 = $scope.secondgrid.every(function (itm)

            { return itm.ivrmrP_AddFlag; });
        }

        $scope.Toggle_Deletesave = function () {
            
            var toggleStatus2 = $scope.Deletesaveall2;
            angular.forEach($scope.secondgrid, function (itm) {
                itm.ivrmrP_DeleteFlag = toggleStatus2;
            });
        }

        $scope.Toggle_Deletesavegrd = function () {
            
            $scope.Deletesaveall2 = $scope.secondgrid.every(function (itm)

            { return itm.ivrmrP_DeleteFlag; });
        }

        $scope.Toggle_Updatesave = function () {
            
            var toggleStatus2 = $scope.Updatesaveall2;
            angular.forEach($scope.secondgrid, function (itm) {
                itm.ivrmrP_UpdateFlag = toggleStatus2;
            });
        }

        $scope.Toggle_Updatesavegrd = function () {
            
            $scope.Updatesaveall2 = $scope.secondgrid.every(function (itm)

            { return itm.ivrmrP_UpdateFlag; });
        }

        $scope.Toggle_Processsave = function () {
            
            var toggleStatus2 = $scope.Processsaveall2;
            angular.forEach($scope.secondgrid, function (itm) {
                itm.ivrmrP_ProcessFlag = toggleStatus2;
            });
        }

        $scope.Toggle_Processsavegrd = function () {
            
            $scope.Processsaveall2 = $scope.secondgrid.every(function (itm)

            { return itm.ivrmrP_ProcessFlag; });
        }


        $scope.Toggle_Reportsave = function () {
            
            var toggleStatus2 = $scope.Reportsaveall2;
            angular.forEach($scope.secondgrid, function (itm) {
                itm.ivrmrP_ReportFlag = toggleStatus2;
            });
        }

        $scope.Toggle_Reportsavegrd = function () {
            
            $scope.Reportsaveall2 = $scope.secondgrid.every(function (itm)

            { return itm.ivrmrP_ReportFlag; });
        }


        $scope.Toggle_Searchsave = function () {
            
            var toggleStatus2 = $scope.Searchaveall2;
            angular.forEach($scope.secondgrid, function (itm) {
                itm.ivrmrP_SearchFlag = toggleStatus2;
            });
        }

        $scope.Toggle_Searchsavegrd = function () {
            
            $scope.Searchaveall2 = $scope.secondgrid.every(function (itm)

            { return itm.ivrmrP_SearchFlag; });
        }





        var selection = {};

        $scope.SelectedStudentRecord = {};
        $scope.deleteselectedrecord = {};

        $scope.addtocart = function (record, previouslysaveddata, index) {
            $scope.gridview2 = true;
            if ($scope.secondgrid.indexOf(record) === -1) {
                var valid;
                for (var i = 0; i < previouslysaveddata.length; i++) {
                    if (previouslysaveddata[i].ivrmP_Id == record.ivrmP_Id) {
                        swal("Already this page is saved with Current role and Module..Kindly select other pages")
                        valid = "committ";
                        record.checked = false;
                        // $scope.disablecheck[index] = true;
                    }
                }

                if (valid != "committ") {
                    if ($scope.secondgrid.indexOf(record) === -1) {
                        $scope.secondgrid.push(record);
                    }
                    else {
                        $scope.secondgrid.splice($scope.secondgrid.indexOf(record), 1);
                    }
                }
            }
            else {
                $scope.secondgrid.splice($scope.secondgrid.indexOf(record), 1);
            }

            for (var i = 0; i < $scope.secondgrid.length; i++) {
                
                if ($scope.secondgrid[i].ivrmrP_ReportFlag == true) {
                    $scope.Reportsaveall2 = true;
                }
                else {
                    $scope.Reportsaveall2 = false;
                    break;

                }
            }

            for (var i = 0; i < $scope.secondgrid.length; i++) {
                
                if ($scope.secondgrid[i].ivrmrP_AddFlag == true) {
                    $scope.Addsaveall2 = true;
                }
                else {
                    $scope.Addsaveall2 = false;
                    break;

                }
            }
            for (var i = 0; i < $scope.secondgrid.length; i++) {
                
                if ($scope.secondgrid[i].ivrmrP_DeleteFlag == true) {
                    $scope.Deletesaveall2 = true;
                }
                else {
                    $scope.Deletesaveall2 = false;
                    break;

                }
            }

            for (var i = 0; i < $scope.secondgrid.length; i++) {
                
                if ($scope.secondgrid[i].ivrmrP_UpdateFlag == true) {
                    $scope.Updatesaveall2 = true;
                }
                else {
                    $scope.Updatesaveall2 = false;
                    break;

                }
            }

            for (var i = 0; i < $scope.secondgrid.length; i++) {
                
                if ($scope.secondgrid[i].ivrmrP_ProcessFlag == true) {
                    $scope.Processsaveall2 = true;
                }
                else {
                    $scope.Processsaveall2 = false;
                    break;

                }
            }
            for (var i = 0; i < $scope.secondgrid.length; i++) {
                
                if ($scope.secondgrid[i].ivrmrP_SearchFlag == true) {
                    $scope.Searchaveall2 = true;
                }
                else {
                    $scope.Searchaveall2 = false;
                    break;

                }
            }

        };
        //$scope.addtocart = function (record, previousgridrecord, index) {

        //    $scope.gridview2 = true;
        //    if ($scope.secondgrid.indexOf(record) === -1) {
        //        $scope.secondgrid.push(record);

        //    }
        //    else {
        //        $scope.secondgrid.splice($scope.secondgrid.indexOf(record), 1);

        //    }
        //    if ($scope.secondgrid != null && $scope.secondgrid.length > 0)
        //    {
        //        $scope.SeletedPageDiv = true;
        //    }
        //    else {
        //        $scope.SeletedPageDiv = false;
        //    }
        //};


        $scope.currenrow = {};
        $scope.deletesecondgriddata = function (index, stuDelRecord, currenrow) {
            // $("#check-" + stuDelRecord.ivrmP_Id).attr('checked', false);


            swal({
                title: "Are you sure",
                text: "Do you want to Delete Record????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
    function (isConfirm) {
        if (isConfirm) {
            if (stuDelRecord.ivrmirP_Id > 0) {
                //$scope.deletesecondgriddatafromdb(stuDelRecord.ivrmirP_Id);

                apiService.DeleteURI("MasterInstitutionRoleandModuleMapping/deletedetails", stuDelRecord.ivrmirP_Id).
                  then(function (promise) {
                  })
            }

            stuDelRecord.checked = false;
            console.log($scope.secondgrid.indexOf(stuDelRecord));
            $scope.$apply(function () {
                $scope.secondgrid.splice($scope.secondgrid.indexOf(stuDelRecord), 1);

                for (var i = 0; i < $scope.secondgrid.length; i++) {
                    
                    if ($scope.secondgrid[i].ivrmrP_ReportFlag == true) {
                        $scope.Reportsaveall2 = true;
                    }
                    else {
                        $scope.Reportsaveall2 = false;
                        break;

                    }
                }

                for (var i = 0; i < $scope.secondgrid.length; i++) {
                    
                    if ($scope.secondgrid[i].ivrmrP_AddFlag == true) {
                        $scope.Addsaveall2 = true;
                    }
                    else {
                        $scope.Addsaveall2 = false;
                        break;

                    }
                }
                for (var i = 0; i < $scope.secondgrid.length; i++) {
                    
                    if ($scope.secondgrid[i].ivrmrP_DeleteFlag == true) {
                        $scope.Deletesaveall2 = true;
                    }
                    else {
                        $scope.Deletesaveall2 = false;
                        break;

                    }
                }

                for (var i = 0; i < $scope.secondgrid.length; i++) {
                    
                    if ($scope.secondgrid[i].ivrmrP_UpdateFlag == true) {
                        $scope.Updatesaveall2 = true;
                    }
                    else {
                        $scope.Updatesaveall2 = false;
                        break;

                    }
                }

                for (var i = 0; i < $scope.secondgrid.length; i++) {
                    
                    if ($scope.secondgrid[i].ivrmrP_ProcessFlag == true) {
                        $scope.Processsaveall2 = true;
                    }
                    else {
                        $scope.Processsaveall2 = false;
                        break;

                    }
                }
                for (var i = 0; i < $scope.secondgrid.length; i++) {
                    
                    if ($scope.secondgrid[i].ivrmrP_SearchFlag == true) {
                        $scope.Searchaveall2 = true;
                    }
                    else {
                        $scope.Searchaveall2 = false;
                        break;

                    }
                }
            });

            if ($scope.secondgrid != null && $scope.secondgrid.length > 0) {
                $scope.SeletedPageDiv = true;
            }
            else {
                $scope.SeletedPageDiv = false;
                if ($scope.privilegesList == null || $scope.privilegesList.length == 0) {
                    $scope.IVRMRT_Id = "";
                }
            }

            swal("Row as been removed from list Successfully");
            

        }
        else {
            swal("Cancelled Successfully");
        }
    });

        };
        $scope.order = function (predicate) {
            $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
            $scope.predicate = predicate;
        };

        $scope.chckedIndexs = [];





        $scope.checkAll = function () {
            if ($scope.selectedAll) {

                $scope.selectedAll = true;
            } else {
                $scope.selectedAll = false;
            }
            angular.forEach($scope.privilegesList, function (privilege) {
                privilege.Selected = $scope.selectedAll;

                if ($scope.chckedIndexs.indexOf(privilege) === -1) {
                    if (privilege.Selected) $scope.chckedIndexs.push(privilege);
                }
            });
        }



        $scope.toggleAllmaster = function () {
            $scope.deletegrid = [];
            if ($scope.searchList == '') {
                var toggleStatus = $scope.alll;
                angular.forEach($scope.institutionRolePrivilegesList, function (itm) {
                    itm.checked = toggleStatus;
                    if ($scope.alll == true) {
                        $scope.deletegrid.push(itm);
                    }
                    else {
                        $scope.deletegrid.splice(itm);
                    }
                });
            }

            if ($scope.searchList != '') {
                var toggleStatus = $scope.alll;
                angular.forEach($scope.filterValue1master, function (itm) {
                    itm.checked = toggleStatus;
                    if ($scope.alll == true) {
                        $scope.deletegrid.push(itm);
                    }
                    else {
                        $scope.deletegrid.splice(itm);
                    }
                });
            }
            if ($scope.deletegrid.length > 0) {
                $scope.showdelete = true;
              
            }
            else {
                $scope.showdelete = false;
               
            }

        }

        $scope.deletegrid = [];
        $scope.searchList = "";
        $scope.addtocartdelete = function (SelectedStudentRecord, index) {
            $scope.gridview2 = true;
            $scope.btns = true;
            if ($scope.searchList == '') {
                $scope.alll = $scope.institutionRolePrivilegesList.every(function (itm) { return itm.checked; });

                if (SelectedStudentRecord.checked == true) {
                    angular.forEach($scope.institutionRolePrivilegesList, function (qwe) {
                        if (qwe.ivrmirP_Id == SelectedStudentRecord.ivrmirP_Id) {

                            $scope.deletegrid.push(qwe);
                        }
                    })
                } else if (SelectedStudentRecord.checked == false) {
                    angular.forEach($scope.deletegrid, function (qwe) {
                        if (qwe.ivrmirP_Id == SelectedStudentRecord.ivrmirP_Id) {

                            $scope.deletegrid.splice($scope.deletegrid.indexOf(qwe), 1);
                        }
                    })

                }

            }

            if ($scope.searchList != '') {
                $scope.alll = $scope.filterValue1master.every(function (itm) { return itm.checked; });
                if (SelectedStudentRecord.checked == true) {
                    angular.forEach($scope.institutionRolePrivilegesList, function (qwe) {
                        if (qwe.ivrmirP_Id == SelectedStudentRecord.ivrmirP_Id) {

                            $scope.deletegrid.push(qwe);
                        }
                    })
                } else if (SelectedStudentRecord.checked == false) {
                    angular.forEach($scope.deletegrid, function (qwe) {
                        if (qwe.ivrmirP_Id == SelectedStudentRecord.ivrmirP_Id) {

                            //$scope.secondgrid.splice(qwe);
                            $scope.deletegrid.splice($scope.deletegrid.indexOf(qwe), 1);
                        }
                    })

                }
            }
            if ($scope.deletegrid.length > 0) {
                $scope.showdelete = true;
            }
            else {
                $scope.showdelete = false;
            }

        }



        $scope.test = function (data) {

            console.log(data.Selected);
            if ($scope.chckedIndexs.indexOf(data) === -1) {
                if (data.Selected) $scope.chckedIndexs.push(data);
            }
            else {
                $scope.chckedIndexs.splice($scope.chckedIndexs.indexOf(data), 1);
            }
        }
        $scope.submitted = false;
        $scope.savadata = function (pagesrecord) {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {

                var array = $.map(pagesrecord, function (value, index) {
                    return [value];
                });

                var plg = $scope.chckedIndexs;

                if ((plg != null && plg.length > 0) || (array != null && array.length > 0)) {
                    var data = {
                        "MI_Id": $scope.MI_Id,
                        "IVRMM_Id": $scope.IVRMM_Id,
                        "IVRMRT_Id": $scope.IVRMRT_Id,
                        privilagedata: plg,
                        savetmpdata: array
                    }

                    apiService.create("MasterInstitutionRoleandModuleMapping/", data).
                    then(function (promise) {
                        $scope.submitted = false;
                        if (promise.returnval == "Save" || promise.returnval == "Update") {
                            if (promise.returnval == "Save") {
                                swal('Record Saved Successfully', 'success');
                                $state.reload();
                            }
                            else if (promise.returnval == "Update") {
                                swal('Record Updated Successfully', 'success');
                                $state.reload();
                            }
                        }
                        else if (promise.returnval == "NotSave" || promise.returnval == "NotUpdate") {
                            if (promise.returnval == "NotSave") {
                                swal('Record Not Saved');
                                $state.reload();
                            }
                            else if (promise.returnval == "NotUpdate") {
                                swal('Record Not Updated');
                                $state.reload();

                            }
                        }
                        $state.reload();
                    })
                }
                else {
                    swal("Please select atleast one record to procced..!");
                    return;
                }
            }
        };

        $scope.delete = function (id, SweetAlert) {
            swal({
                title: "Are you sure?",
                text: "Do you want to Delete Record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("MasterInstitutionRoleandModuleMapping/deletedetails", id).
                    then(function (promise) {
                        swal('Record Deleted Successfully!', 'success');
                        $state.reload();
                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });


        }





        $scope.edit = function (data) {
            $scope.SeletedPageDiv = true;

            $scope.chckedIndexs = [];
            $scope.privilegesList = $scope.chckedIndexs;
            $scope.MI_Id = data.mI_Id;
            $scope.IVRMM_Id = data.ivrmM_Id;

            $scope.secondgrid[0] = data;

            $scope.IVRMRT_Id = data.ivrmrT_Id;

            $scope.secondgrid[0].ivrmrP_AddFlag = data.ivrmirP_AddFlag;
            $scope.secondgrid[0].ivrmrP_DeleteFlag = data.ivrmirP_DeleteFlag;
            $scope.secondgrid[0].ivrmrP_UpdateFlag = data.ivrmirP_AddFlag;

            $scope.secondgrid[0].ivrmrP_ProcessFlag = data.ivrmirP_ProcessFlag;
            $scope.secondgrid[0].ivrmrP_ReportFlag = data.ivrmirP_ReportFlag;
            $scope.secondgrid[0].ivrmrP_SearchFlag = data.ivrmirP_SearchFlag;

            console.log(data);
        }

        $scope.interacted = function (field) {
            // swal(field);

            return $scope.submitted || field.$dirty;
        };

        //
        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }

        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        }

        $scope.IsHidden3 = true;
        $scope.ShowHide3 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden3 = $scope.IsHidden3 ? false : true;
        }

        $scope.IsHidden4 = true;
        $scope.ShowHide4 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden4 = $scope.IsHidden4 ? false : true;
        }

        $scope.IsHidden5 = true;
        $scope.ShowHide5 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden5 = $scope.IsHidden5 ? false : true;
        }


        $scope.cleardata = function () {
            $state.reload();
            $scope.submitted = false;
        }


        $scope.sortPage = function (keyname) {
            $scope.reverse1 = !$scope.reverse1;
            $scope.sortKey = keyname;   //set the sortKey to the param passed
        }


        $scope.sortMasterPrevilege = function (keyname) {
            $scope.reverse2 = !$scope.reverse2;
            $scope.sortKey = keyname;   //set the sortKey to the param passed
        }
        $scope.sortSelectedRecord = function (keyname) {
            $scope.reverse3 = !$scope.reverse3;
            $scope.sortKey = keyname;   //set the sortKey to the param passed
        }
        $scope.sortList = function (keyname) {
            $scope.reverse4 = !$scope.reverse4;
            $scope.sortKey = keyname;   //set the sortKey to the param passed
        }



        $scope.submitted = false;
      
        $scope.deletedata = function (savedpagesrecord) {

            swal({
                title: "Are you sure?",
                text: "Do you want to Delete Record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        $scope.submitted = true;

                        if (savedpagesrecord != undefined) {
                            var savedarray = $.map(savedpagesrecord, function (value, index) {
                                return [value];
                            });
                        }

                        var data = {
                            "MI_Id": $scope.MI_Id,
                            privilagedata: savedarray
                        };

                        apiService.create("MasterInstitutionRoleandModuleMapping/deletedetails", data).
                            then(function (promise) {

                                if (promise.returnval == 'Data Deleted Successfully') {
                                    swal(promise.returnval);
                                    $state.reload();
                                }
                                else {
                                    swal(promise.returnval)
                                    $state.reload();
                                }

                            });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });

       
            // };
        }

    }

})();