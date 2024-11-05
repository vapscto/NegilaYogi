

(function () {
    'use strict';
    angular
.module('app')
.controller('FeeClassCategoryController', feeclassController)

    feeclassController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$stateParams']
    function feeclassController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache,$stateParams) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.sortKey = "fmcC_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.sortKey1 = "fyccC_Id";   //set the sortKey to the param passed
        $scope.reverse1 = true; //if true make it false and vice versa
        $scope.search = "";
        $scope.search1 = "";
        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {

            return angular.lowercase(obj.asmcL_ClassName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }
        $scope.savedisable = true;

        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.disabled = true;
                if (privlgs[i].ivrmirP_AddFlag == true) {
                    $scope.saveflg = true;
                    $scope.savebtn = true;

                }
                else {
                    $scope.saveflg = false;
                    $scope.savebtn = false;
                }
                if (privlgs[i].ivrmirP_UpdateFlag == true) {
                    $scope.editflag = true;
                    $scope.editbtn = true;

                }
                else {
                    $scope.editflag = false;
                    $scope.editbtn = false;
                }
                if (privlgs[i].ivrmirP_DeleteFlag == true) {
                    $scope.deactiveflag = true;
                    $scope.deletebtn = true;
                }
                else {
                    $scope.deactiveflag = false;
                    $scope.deletebtn = false;
                }


            }
        }
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = paginationformasters;
            $scope.page1 = "page1";
            $scope.page2 = "page2";
          

            $scope.reg = {};

            var pageid = 2;
            apiService.getURI("FeeClassCategory/getalldetails", pageid).
        then(function (promise) {

            //$scope.arrlist6 = academicyrlst;
            //$scope.reg.ASMAY_Id = academicyrlst[0].asmaY_Id;
            $scope.arrlist6 = promise.academicdrp;

            $scope.students = promise.claSSCategoryArray;

            $scope.arrlist7 = promise.classcategorydrp;
            $scope.arrlistchk = promise.admclas;
            $scope.pages = promise.clsYearData;

            $scope.totcountfirst = $scope.students.length;
            $scope.totcountsecond = $scope.pages.length;

            //$scope.user = {
            //    arrlistchk: [$scope.arrlistchk[1]]
            //};
            //$scope.checkAll = function () {
            //    $scope.user.arrlistchk = angular.copy($scope.arrlistchk);
            //};
            //$scope.uncheckAll = function () {
            //    $scope.user.arrlistchk = [];
            //};
            //$scope.checkFirst = function () {
            //    $scope.user.arrlistchk.splice(0, $scope.user.arrlistchk.length);
            //    $scope.user.arrlistchk.push($scope.arrlistchk[0]);
            //};
        })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

            $scope.sort1 = function (keyname) {
                $scope.sortKey1 = keyname;   //set the sortKey to the param passed
                $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
            }
        }
        $scope.submitted = false;
        $scope.saveGroupdata = function () {
            if ($scope.myform1.$valid) {
                var data = {
                    "fmcC_Id":$scope.fmcC_Id,
                    "MI_Id": 2,
                    "FMCC_ClassCategoryName": $scope.FMCC_ClassCategoryName,
                    "FMCC_ClassCategoryCode": $scope.FMCC_ClassCategoryCode,
                    "FMCC_ActiveFlag": true,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("FeeClassCategory/", data).
                then(function (promise) {

                    $scope.students = promise.claSSCategoryArray;

                    $scope.arrlist7 = promise.claSSCategoryArray;

                    if (promise.returnduplicatestatus === 'Duplicate' && promise.returnval === false) {
                        swal('Record Already Exist');
                    }
                    else if (promise.returnval === true) {

                        if (promise.message != null) {
                            swal('Record Updated Successfully', 'success');
                        }
                        else {
                            swal('Record Saved Successfully', 'success');
                        }
                    }
                    else {

                        if (promise.message != null) {
                            swal('Record Not Updated', 'success');
                        }
                        else {
                            swal('Record Not Saved', 'success');
                        }
                    }
                    $scope.cance();
                //    $scope.loaddata();
                })
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1 || field.$dirty;
        };
        $scope.cance = function () {
            $scope.fmcC_Id = 0;
            $scope.FMCC_ClassCategoryName = "";
            $scope.FMCC_ClassCategoryCode = "";
            $scope.submitted = false;
            $scope.myform1.$setPristine();
            $scope.myform1.$setUntouched();
          
            $scope.search = "";
        }
        $scope.searchsource = function () {
            var entereddata = $scope.search;
            var data = {
                "FMCC_ClassCategoryName": $scope.search,
                "FMCC_ClassCategoryCode": $scope.type
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeClassCategory/1", data).
        then(function (promise) {
            $scope.students = promise.claSSCategoryArray;
            swal("searched Successfully");
        })
        }
        $scope.editEmployee = {}
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.fmcC_Id;
            var pageid = $scope.editEmployee;
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
                    apiService.getURI("FeeClassCategory/deletepages", pageid).
                    then(function (promise) {
                        $scope.students = promise.claSSCategoryArray;
                        if (promise.retvalue === true && promise.returnresult === false) {
                            swal('Class Category Already Mapped in yearly class/Amount ');
                        }
                        else if (promise.returnval === true) {
                            swal('Record Deleted Successfully');
                        }
                        else {
                            swal('Record Not Deleted Successfully');
                        }
                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.fmcC_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("FeeClassCategory/getdetails", pageid).
            then(function (promise) {
                $scope.fmcC_Id = promise.claSSCategoryArray[0].fmcC_Id;
                $scope.FMCC_ClassCategoryName = promise.claSSCategoryArray[0].fmcC_ClassCategoryName;
                $scope.FMCC_ClassCategoryCode = promise.claSSCategoryArray[0].fmcC_ClassCategoryCode;
            })
        }
        $scope.deactive = function (claSSCategoryArray) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            var mgs = "";
            var msgs = "";
            var confirmmgs = "";
            if (claSSCategoryArray.fmcC_ActiveFlag ==true) {
                mgs = "Deactivate";
                msgs = "Deactivation";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Activate";
                msgs = "Activation";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + mgs + " Record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
           function (isConfirm) {
               if (isConfirm) {

                   var config = {
                       headers: {
                           'Content-Type': 'application/json;'
                       }
                   }

                   apiService.create("FeeClassCategory/deactivate", claSSCategoryArray).
                       then(function (promise) {
                           $scope.loaddata();

                           $scope.arrlist7 = promise.claSSCategoryArray;
                           $scope.students = promise.classcategorydrp;

                           if (promise.returnval === true) {
                               swal(confirmmgs +' Successfully');
                           }
                           else if (promise.message == "used") {
                               swal("Record cannot Deactivate.It is already used");
                           }
                           else {
                               swal('Record Not  Activated/Deactivated');
                           }

                           $scope.cance();
                       })
               }
               else {
                   swal("Record " + msgs + " Cancelled");
               }
           });
        }


        //for secon tab

        var name = "";
        var roleflag = "";
        $scope.submitted1 = false;
        $scope.saveYearlydata = function (selected, arrlistchk) {
            
            if ($scope.myform2.$valid) {

                $scope.albumNameArray = [];
                //for (var j = 0; j < selected.length; j++) {
                //    for (var i = 0; i < arrlistchk.length; i++) {
                //        if (selected[j] == arrlistchk[i].asmcL_Id) {
                //            $scope.albumNameArray.push(arrlistchk[i]);
                //        }
                //    }
                //}
                $scope.albumNameArray = [];
                angular.forEach($scope.arrlistchk, function (role) {
                    if (!!role.selected) $scope.albumNameArray.push(role);
                })
                if ($scope.albumNameArray.length > 0) {
                    var data = {
                       // "MI_Id": 2,
                        "ASMAY_Id": $scope.reg.ASMAY_Id,
                        "FMCC_Id": $scope.reg.fmcC_Id,
                        "FYCC_ActiveFlag": true,
                        "TempararyArrayList": $scope.albumNameArray,
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("FeeClassCategory/savedetailY", data).
                    then(function (promise) {
                        
                        $scope.students = promise.claSSCategoryArray;
                        if (promise.returnduplicatestatus === 'Duplicate')
                        {
                            swal('Record Already Exist');
                        }
                        else 
                        {
                            if (promise.message != null) {
                                swal('Record Updated Successfully', 'success');
                            }
                            else {
                                swal('Record Saved Successfully', 'success');
                            }
                            //$state.reload();
                          $scope.cance1($scope.arrlistchk);
                           // $scope.loaddata();
                            $scope.pages = promise.clsYearData;
                            console.log($scope.pages)
                            $scope.arrlistchk = [];
                        }
                        
                    })
                  
                }
                else {
                    swal('Select Class Atleast One');
                }
            }
            else {
                $scope.submitted1 = true;
            }
        };
        $scope.cance1 = function (arrlistchk) {
            $scope.reg.ASMAY_Id = "";
            $scope.reg.fmcC_Id = "";   
            for (var i = 0; i < $scope.arrlistchk.length; i++) {
                name = $scope.arrlistchk[i].selected
                if (name === true) {
                    $scope.arrlistchk[i].selected = false;
                }
            }
            $scope.submitted1 = false;
            $scope.myform2.$setPristine();
            $scope.myform2.$setUntouched();
            $scope.search1 = "";
        }


        $scope.searchsourceY = function () {
            var entereddata = $scope.search;
            var data = {
                "ASMAY_Id": $scope.search,
                "FMCC_Id": $scope.type
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeClassCategory/1", data).
        then(function (promise) {
            $scope.loaddata();
            $scope.students = promise.claSSCategoryArray;
            swal("searched Successfully");
        })
        }
        $scope.editEmployeeY = {}
        $scope.deletedataY = function (employee, SweetAlert) {
            $scope.editEmployeeY = employee.fycC_Id;
            var pageid = $scope.editEmployeeY;
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
                    apiService.getURI("FeeClassCategory/deletepagesY", pageid).
                    then(function (promise) {
                        $scope.loaddata();
                        $scope.students = promise.claSSCategoryArray;
                         if (promise.returnval === true) {
                            swal('Record Deleted Successfully');
                        }
                        else {
                            swal('Record Not Deleted Successfully');
                        }
                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }
        $scope.getorgvalueY = function (employee, arrlistchk) {
            $scope.editEmployeeY = employee.fycC_Id;
            var pageid = $scope.editEmployeeY;
            apiService.getURI("FeeClassCategory/getdetailsY", pageid).
            then(function (promise) {
                $scope.fycC_Id = promise.clsYearData[0].fycC_Id;
                $scope.ASMCL_ClassName = promise.clsYearData[0].asmcL_ID;
                $scope.arrlist6.selected = promise.clsYearData[0].asmaY_Id;
                $scope.arrlist7.selected = promise.clsYearData[0].fmcC_Id;
            })
        }
        $scope.deactiveY = function (claSSCategoryArray) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeClassCategory/deactivateY", claSSCategoryArray).
            then(function (promise) {
                $scope.reg.ASMAY_Id = "";
                $scope.fmcC_Id = "";
                $scope.loaddata();
                $scope.students = promise.claSSCategoryArray;
                if (promise.returnval === true) {
                    swal('Record Activated/Deactivated Successfully');
                }
                else {
                    swal('Record Not  Activated/Deactivated');
                }
            })
        };

        $scope.category = function () {
            
            var id = $scope.reg.fmcC_Id;

            var data = {
                "ASMAY_Id": $scope.reg.ASMAY_Id,
                "FMCC_Id": $scope.reg.fmcC_Id
            }


            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeClassCategory/Loaddata", data).
        then(function (promise) {

            if (promise.clsYearData != null && promise.clsYearData !== "") {
                

                $scope.arrlistchk = promise.clsYearData;

                //for (var i = 0; i < $scope.arrlistchk.length; i++) {
                //    $scope.arrlistchk[i].selected = false;
                //}
                //for (var i = 0; i < $scope.arrlistchk.length; i++) {
                //    $scope.iddd = $scope.arrlistchk[i].asmcL_ID;
                //    for (var j = 0; j < promise.clsYearData.length; j++) {
                //        if ($scope.iddd == promise.clsYearData[j].asmcL_ID) {
                //            $scope.arrlistchk[i].selected = true;
                //        }
                //    }
                //}


            }
        })
        };
        $scope.isOptionsRequired_1 = function () {

            return !$scope.arrlistchk.some(function (options) {
                return options.selected;
            });
        }
        function getIndependentDrpDwnss() {
            apiService.getURI("FeeClassCategory/getdpforyear", 2).then(function (promise) {

                $scope.arrlist6 = promise.academicdrp;
                $scope.arrlist7 = promise.classcategorydrp;
                // for pagination 
                $scope.currentPage = 1;
                $scope.itemsPerPage = 5;
                $scope.pages = promise.academicdrp;
                $scope.pages = promise.classcategorydrp;
                // for pagination
            })
            
        }
    
        $scope.OnAcdchange = function () {
            $scope.reg.fmcC_Id = "";
        }
     
    }
})();