
(function () {
    'use strict';
    angular
.module('app')
.controller('SportMasterHouseCommitteController', SportMasterHouseCommitteController)

    SportMasterHouseCommitteController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function SportMasterHouseCommitteController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.sortKey = 'SPCCMHC_Id';
        $scope.sortReverse = true;

        //var paginationformasters;
        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        //}

        $scope.ddate = {};
        $scope.ddate = new Date();

        //$scope.usrname = localStorage.getItem('username');
        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        //if (admfigsettings.length > 0) {
        //    var logopath = admfigsettings[0].asC_Logo_Path;
        //}

        //$scope.imgname = logopath;

        //var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));



        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("SportMasterHouseCommitte/Getdetails").
       then(function (promise) {
           
           $scope.currentPage = 1;
           $scope.itemsPerPage = 10;

               $scope.yearlt = promise.yearlist;
               $scope.DesignationList = promise.designationList;
               //$scope.HouseList = promise.houseList;
               $scope.ClassList = promise.classList;
      
           if (promise.count > 0) {
               $scope.gridviewDetails = promise.gridviewDetails;
           }
           
           $scope.cancel();

           //$scope.Categaries = promise.SportMasterHouseCommittename;
       })
        };

        $scope.get_section = function (asmcL_Id) {
            
            $scope.amsT_Id = "";
            $scope.asmS_Id = "";
            $scope.studentDropdown = "";
            var data = { 
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            }
            apiService.create("SportMasterHouseCommitte/get_section", data)
                .then(function (promise) {
                    $scope.sectionDropdown = promise.sectionList;


                })
        }
        $scope.get_student = function (asmS_Id) {
            

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            }
            apiService.create("SportMasterHouseCommitte/get_student", data)
                .then(function (promise) {
                    $scope.studentDropdown = promise.studentList;


                })

        }

        // to Edit Data
        $scope.EditSportMasterHouseCommittedata = function (EditRecord) {
            debugger;
            var data = {
                "SPCCMHC_Id": EditRecord.spccmhC_Id,
                "ASMAY_Id": EditRecord.asmaY_Id,
                
            }
           
            apiService.create("SportMasterHouseCommitte/GetSelectedRowDetails/", data).
            then(function (promise) {
                
             
                debugger;
                $scope.HouseList = promise.houseList;
                $scope.DesignationList = promise.designationList;

                $scope.ContactNo = promise.studentList[0].spccmhC_ContactNo;
                $scope.EmailId = promise.studentList[0].spccmhC_EmailId;
                $scope.SPCCMHC_Id = promise.studentList[0].spccmhC_Id;
                $scope.SPCCMHD_Id = promise.studentList[0].spccmhD_Id;
                
                $scope.AMST_Id = promise.studentList[0].amsT_Id;
                $scope.asmaY_Id = promise.studentList[0].asmaY_Id;

                $scope.get_House($scope.asmaY_Id);
                $scope.SPCCMH_Id = promise.studentList[0].spccmH_Id; 
                $scope.onhousechage($scope.SPCCMH_Id);

             
                $scope.AMST_Id = promise.studentList[0].amsT_Id;
                

            })
        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveSportMasterHouseCommittedata = function () {
            
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                if ($scope.ContactNo == "") {
                    $scope.ContactNo = 0;
                }


                var data = {
                    "SPCCMHC_Id": $scope.SPCCMHC_Id,
                    "SPCCMHD_Id": $scope.SPCCMHD_Id,
                    "SPCCMHC_ContactNo": $scope.ContactNo,
                    "SPCCMHC_EmailId": $scope.EmailId,
                    "SPCCMH_Id":$scope.SPCCMH_Id,
                    //"ASMS_Id": $scope.ASMS_Id,
                    //"ASMCL_Id": $scope.ASMCL_Id,
                    "AMST_Id": $scope.AMST_Id
                }
                apiService.create("SportMasterHouseCommitte/", data).
                    then(function (promise) {
                        
                        if (promise.msg != "" && promise.msg != null) {
                            swal(promise.msg);
                        }
                        else if (promise.returnVal == true) {
                            swal("Record Saved Successfully");
                        }
                        else if (promise.returnVal_update == true) {
                            swal("Record Updated Successfully");
                        }
                        else if (promise.duplicate_caste_name_bool == true) {
                            swal("House Committe Contact Already Exists");
                        }
                        else if (promise.returnVal == false) {
                            swal("Failed to Save");
                        }
                        else if (promise.returnVal_update == false) {
                            swal("Failed to Update");
                        }
                        $scope.BindData();
                    })
            }
        };

        $scope.deactive = function (newuser1, SweetAlertt) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            
            var mgs = "";
            if (newuser1.spccmhD_ActiveFlag == false) {

                mgs = "Activate";

            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the House Committee?",
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
                     apiService.create("SportMasterHouseCommitte/deactivate", newuser1).
                     then(function (promise) {
                         
                         if (promise.returnVal == true) {
                             if (promise.msg != null) {
                                 swal(promise.msg);
                                 $scope.BindData();
                             }
                         }
                         else {
                             swal('Failed to Activate/Deactivate the Record');
                         }
                     })
                 } else {
                     swal("Cancelled");
                 }
             })
        }


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

        $scope.cancel = function () {
            
            $scope.SPCCMHC_Id = 0;
            $scope.SPCCMHD_Id = "";
            $scope.ContactNo = "";
            $scope.EmailId = "";
            $scope.SPCCMH_Id = "";
            $scope.ASMS_Id = "";
            $scope.ASMCL_Id = "";
            $scope.AMST_Id = "";
            $scope.ASMAY_Id = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.asmaY_Id = "";
        }

        $scope.searchValue = "";
        //$scope.filterValue = function (obj) {
        //    return (angular.lowercase(obj.spccmhD_DesignationDescription)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (angular.lowercase(obj.spccmhD_DesignationName)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        //}



        $scope.onhousechage = function () {
            debugger;
            var data = {
                "SPCCMH_Id": $scope.SPCCMH_Id,
                "ASMAY_Id": $scope.asmaY_Id,
            }
            apiService.create("SportMasterHouseCommitte/onhousechage/", data).then(function (promise) {

                $scope.studentDropdown = promise.studentList;

            })
        }

        //============================Get House List
        $scope.get_House = function () {
            debugger;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
            }
            apiService.create("SportMasterHouseCommitte/get_House", data).
                then(function (promise) {

                    $scope.HouseList = promise.houseList;

                });
        }






    }

})();