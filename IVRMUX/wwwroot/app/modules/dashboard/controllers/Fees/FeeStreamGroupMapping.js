(function () {
    'use strict';
    angular
.module('app')
.controller('FeeStreamGroupMappingController', FeeStreamGroupMappingController)

    FeeStreamGroupMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$stateParams', 'superCache']
    function FeeStreamGroupMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $stateParams, superCache) {
      
        $scope.sortKey = 'fmsgM_ID';
        $scope.sortReverse = true;

      
        $scope.searchValue = "";
     
        $scope.columnSort = false;
        $scope.isOptionsRequired = function () {
            return !$scope.classDrpDwn.some(function (options) {
                return options.Selected;
            });
        }

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0 && ivrmcofigsettings!=null) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 5;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;


        $scope.loadDrpDwn = function () {
            var pageid = 0;         

            apiService.getURI("FeeStreamGroupMapping/getData/",pageid).
   then(function (promise) {
                $scope.acdYear = promise.yearlist;
                $scope.classDrpDwn = promise.classdetails;
                $scope.groupDrpDwn = promise.groupdetails;
                $scope.streamDrpDwn = promise.stream;
                $scope.loaddetails = promise.getdeatils;
                if (promise.count == 0) {
                    swal('No Records Found');
                }
                else {
                 
                    $scope.presentCountgrid = $scope.loaddetails.length;
                }
            })
          
        }

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.selectedClassList = [];
        $scope.selectedSectionList = [];
        $scope.submitted = false;

        $scope.save = function () {
            
            if ($scope.myForm.$valid) {

                $scope.albumNameArray = [];
                angular.forEach($scope.groupDrpDwn, function (cls) {
                    if (!!cls.Selected) $scope.albumNameArray.push(cls);
                })
                $scope.albumNameArray1 = [];
                angular.forEach($scope.streamDrpDwn, function (sec) {
                    if (!!sec.Selected) $scope.albumNameArray1.push(sec);
                })
        
                
                var data = {
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMAY_Id": $scope.asmaY_Id1,
                    "TempararyArrayList": $scope.albumNameArray,
                    "TempararyArrayList1": $scope.albumNameArray1
                 
                }
                apiService.create("FeeStreamGroupMapping/saveData", data).
                then(function (promise) {
                    if (promise.message != "" && promise.message != null) {
                        alert(promise.message);
                        $scope.clearid();
                    }

                    if (promise.returnVal == true) {
                        if (promise.messagesaveupdate == "Save") {
                            swal('Record Saved Successfully');
                        } else if (promise.messagesaveupdate == "Update") {
                            swal('Record Updated Successfully');
                        }

                        $scope.classcategoryList = promise.classcategoryList;
                        $scope.presentCountgrid = $scope.classcategoryList.length;
                        $scope.clearid();
                    }
                    else if (promise.returnVal == false) {
                        if (promise.messagesaveupdate == "Save") {
                            swal('Record Failed To Save');
                        } else if (promise.messagesaveupdate == "Update") {
                            swal('Record Failed To Update');
                        }

                        $scope.classcategoryList = promise.classcategoryList;
                        $scope.presentCountgrid = $scope.classcategoryList.length;
                        $scope.clearid();
                    }

                })
            }
            else {
                $scope.submitted = true;
            }

        }
        $scope.clearid = function () {
            $state.reload();
            //$scope.ASMAY_Id = "";
            //$scope.AMC_Id = "";
            //$scope.cls.Selected = false;
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.searchByColumn = function (search, columnName) {
            
            if (search != null && search != "" && search != undefined && columnName != null && columnName != "" && columnName != undefined) {
                var data = {
                    "Input": search,
                    "SearchColumn": columnName
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("FeeStreamGroupMapping/searchByColumn", data).
               then(function (promise) {
                   if (promise.count == 0) {
                       swal('No Records Found');
                       $scope.searchValue = "";
                   }
                   else {
                       $scope.searchValue = "";
                       $scope.classcategoryList = promise.classcategoryList;
                       $scope.presentCountgrid = $scope.classcategoryList.length;
                   }
               })
            }
            else {
                swal('Please Enter Data In search here... Text Box And Select Column Name From Dropdown. Then Click On Search Icon');
            }
        }

        //$scope.edit = function (editdata) {
        //    
        //    $scope.edit_flag = true;
        //    var ids = {
        //        "ASMAY_Id": editdata.asmaY_Id,
        //        "ASMCL_Id": editdata.asmcL_Id,
        //        "FMG_Id": editdata.fmG_Id,
        //        "PASL_ID": editdata.pasL_ID
        //    }
        //    apiService.create("FeeStreamGroupMapping/getdetails", ids).
        //    then(function (promise) {
        //                      
        //        $scope.ASMAY_Id = promise.classcategoryList[0].asmaY_Id;
        //        $scope.ASMCL_Id = promise.classcategoryList[0].asmcL_Id;
        //       // $scope.ASMCC_Id = promise.classcategoryList[0].asmcC_Id;
        //        //  $scope.ASMCL_Id = promise.classcategoryList[0].asmcL_Id;
        //        //  $scope.ASMS_Id = promise.classcategoryList[0].asmS_Id;

        //        for (var i = 0; i < $scope.classDrpDwn.length; i++) {
        //            if ($scope.classDrpDwn[i].asmcL_Id == promise.classcategoryList[0].asmcL_Id) {
        //                $scope.classDrpDwn[i].Selected = true;
        //            }
        //            else {
        //                $scope.classDrpDwn[i].Selected = false;
        //            }
        //        }
        //        for (var i = 0; i < $scope.sectionDrpDwn.length; i++) {
        //            if ($scope.sectionDrpDwn[i].asmS_Id == promise.classcategoryList[0].asmS_Id) {
        //                $scope.sectionDrpDwn[i].Selected1 = true;
        //            }
        //            else {
        //                $scope.sectionDrpDwn[i].Selected1 = false;
        //            }
        //        }
        //        //$("input:checkbox").on('click', function () {
        //        //    // in the handler, 'this' refers to the box clicked on
        //        //    var $box = $(this);
        //        //    if ($box.is(":checked")) {
        //        //        // the name of the box is retrieved using the .attr() method
        //        //        // as it is assumed and expected to be immutable
        //        //        var group = "input:checkbox[name='" + $box.attr("name") + "']";
        //        //        // the checked state of the group/box on the other hand will change
        //        //        // and the current value is retrieved using .prop() method
        //        //        $(group).prop("checked", false);
        //        //        $box.prop("checked", true);
        //        //    } else {
        //        //        $box.prop("checked", false);
        //        //    }
        //        //});
        //    })
        //}


        $scope.getorgvalue = function (employee) {
            
            $scope.editEmployee = employee.fmsgM_Id;

            var pageid = $scope.editEmployee;
            apiService.getURI("FeeStreamGroupMapping/Editdetails", pageid).
            then(function (promise) {

         

                $scope.asmcL_Id = promise.groupData[0].asmcL_ID;
                $scope.asmaY_Id1 = promise.groupData[0].asmaY_Id;

                angular.forEach($scope.groupDrpDwn, function (cls) {
                    if (cls.fmG_Id==promise.groupData[0].fmG_Id) 
                    {
                        cls.Selected = true;
                    }
                })
               
                angular.forEach($scope.streamDrpDwn, function (cls) {
                    if (cls.pasL_ID == promise.groupData[0].pasL_ID) {
                        cls.Selected = true;
                    }
                })
                
      

                $scope.FMG_CompulsoryFlag = promise.groupData[0].fmG_CompulsoryFlag;
                var activeflag = promise.groupData[0].fmG_CompulsoryFlag;

                if (promise.groupData[0].fmG_CompulsoryFlag == "1") {
                    $scope.FMG_CompulsoryFlag = true;

                }
                else {
                    $scope.FMG_CompulsoryFlag = false;
                }
        
            })
        }

        $scope.isOptionsRequired = function () {

            return !$scope.groupDrpDwn.some(function (cls) {
                return cls.Selected;
            });
        }

        $scope.isOptionsRequired1 = function () {

            return !$scope.streamDrpDwn.some(function (sec) {
                return sec.Selected;
            });
        }

        $scope.deleteRecord = function (asmcC_Id, SweetAlert) {
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("FeeStreamGroupMapping/deletedetails", asmcC_Id).
                    then(function (promise) {
                        if (promise.message != "" && promise.message != null) {
                            swal(promise.message);
                            $state.reload();
                        }
                        if (promise.returnVal == true) {
                            $scope.classcategoryList = promise.classcategoryList;
                            $scope.presentCountgrid = $scope.classcategoryList.length;
                            swal('Record Deleted Successfully');
                            $state.reload();
                        }
                        else {
                            swal('Failed To Delete Record');
                            //$state.reload();
                        }
                    })
                }
                else {
                    swal("Cancelled");
                    //$state.reload();
                }
            });
        }

        $scope.deactive = function (clscatgry, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            
            var mgs = "";
            if (clscatgry.fmsgM_Active == false) {

                mgs = "Activate";

            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To  " + mgs + " Stream?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("FeeStreamGroupMapping/deactivate", clscatgry).
                        then(function (promise) {
                            $scope.classcategoryList = promise.classcategoryList;
                            $scope.presentCountgrid = $scope.classcategoryList.length;
                            if (promise.msgdeactive == "Deactive") {
                                swal("You Can Not Activate/Deactivate This Record. It Is Already Mapped With Student");
                            }
                            else {
                                if (promise.returnVal == true) {
                                    swal('Stream' + " " + mgs + 'd Successfully');
                                    $state.reload();
                                }
                                else {
                                    swal('Failed to Activate/Deactivate Stream');
                                }
                            }
                        })
                    } else {
                        swal("Cancelled");
                    }
                    $state.reload();
                });
        }

        $scope.filterValue = function (obj) {
           
            return (angular.lowercase(obj.asmaY_Year)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.asmcL_ClassName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.fmG_GroupName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.pasL_Name)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
    }
})();
