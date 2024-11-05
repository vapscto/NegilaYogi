
(function () {
    'use strict';
    angular
        .module('app')
        .controller('Adm_School_Master_CE', Adm_School_Master_CE)

    Adm_School_Master_CE.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function Adm_School_Master_CE($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

        $scope.itemsPerPage = 5;
        $scope.itemsPerPage2 = 5;
        $scope.currentPage = 1;
        $scope.currentPage2 = 1;
        $scope.searchValue = "";
        $scope.searchValue2 = "";
        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("Adm_School_Master_CE/getdata", pageid).then(function (promise) {               

                $scope.streamlist = promise.streamlist;
                $scope.classlist = promise.classlist;
                $scope.cexamlist = promise.cexamlist;
                if (promise != null) {
                    $scope.mastervehicle = promise.mastervehicle;
                    $scope.totcountfirst = $scope.mastervehicle.length;

                    $scope.mastervehicle2 = promise.mastervehicle2;
                    $scope.totcountfirst2 = $scope.mastervehicle2.length;
                }
                else {
                    swal("No Records Found");
                }
            })

        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };


        //---Save--//
        $scope.savedata = function () {
            if ($scope.myform1.$valid) {
               
                var data = {
                    "ASMCE_Id": $scope.ASMCE_Id,
                    "ASMCE_CEName": $scope.ASMCE_CEName,
                    "ASMCE_CECode": $scope.ASMCE_CECode
                }
                apiService.create("Adm_School_Master_CE/savedata", data).then(function (promise) {
                  
                    if (promise.message == "saved") {
                        swal("Data Saved Successfully...!!!");
                        $state.reload();

                    }
                    else if (promise.message == "notsaved") {
                        swal("Data Not Saved Successfully...!!!");
                        $state.reload();
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Data Already Exist...!!!");
                        $state.reload();
                    }
                    else if (promise.message == "updated") {
                        swal("Data Updated Successfully...!!!");
                        $state.reload();
                    }
                    else if (promise.message == "notupdated") {
                        swal("Data Not Updated Successfully...!!!");
                        $state.reload();
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }


        $scope.submitted2 = false;
        $scope.submitted3 = false;
        $scope.savedata2 = function () {
         
            $scope.selectedsectionlist = [];
            if ($scope.myform2.$valid) {
              
                if ($scope.ASSTCLCE_CompulsoryFlg == '1') {
                    $scope.ASSTCLCE_CompulsoryFlg = true;
                }
                else if ($scope.ASSTCLCE_CompulsoryFlg == '0') {
                    $scope.ASSTCLCE_CompulsoryFlg = false;
                }

                var data = {
                    "ASMST_Id": $scope.ASMST_Id,
                    "ASSTCLCE_Id": $scope.ASSTCLCE_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMCE_Id": $scope.ASMCE_Id,
                   
                    "ASSTCLCE_CompulsoryFlg": $scope.ASSTCLCE_CompulsoryFlg
                   
                }
                apiService.create("Adm_School_Master_CE/savedata2", data).then(function (promise) {
                   

                    if (promise.message == "saved") {
                        swal("Data Saved Successfully...!!!");
                    }
                    else if (promise.message == "notsaved") {
                        swal("Data Not Saved Successfully...!!!");
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Data Already Exist...!!!");
                    }
                    else if (promise.message == "updated") {
                        swal("Data Updated Successfully...!!!");
                    }
                    else if (promise.message == "notupdated") {
                        swal("Data Not Updated Successfully...!!!");
                    }

                    $scope.ASMST_Id = '';
                    $scope.ASSTCLCE_Id = 0;
                    $scope.ASMCL_Id = '';
                    $scope.ASMCE_Id = '';
                    $scope.ASSTCLCE_CompulsoryFlg = '0';
                    $scope.BindData();
                })
            }
            else {

                $scope.submitted2 = true;
            }
        }



        //--Sorting--//     
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        //--Sorting--//     
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.sort2 = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.sort2 = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }


        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.asmcE_CEName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.asmcE_CECode)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        $scope.filterValue2 = function (obj) {
            return (angular.lowercase(obj.asmsT_StreamName)).indexOf(angular.lowercase($scope.searchValue2)) >= 0
                ||
                (angular.lowercase(obj.asmcL_ClassName)).indexOf(angular.lowercase($scope.searchValue2)) >= 0
                ||
                (angular.lowercase(obj.asmcE_CEName)).indexOf(angular.lowercase($scope.searchValue2)) >= 0

        }
        //---Edit Data--//
        $scope.edit = function (user) {
       
            var data = {
                "ASMCE_Id": user.asmcE_Id
            }

            apiService.create("Adm_School_Master_CE/editdata", data).then(function (Promise) {
               
                if (Promise != null) {
                    $scope.ASMCE_Id = Promise.editdata[0].asmcE_Id;
                    $scope.ASMCE_CEName = Promise.editdata[0].asmcE_CEName;
                    $scope.ASMCE_CECode = Promise.editdata[0].asmcE_CECode;
                }
            })
        }

        $scope.edit2 = function (user1) {
       
           // $scope.ASMCL_Id = user1.asmcL_Id;
           // $scope.ASMST_Id = user1.asmsT_Id;
            var data = {
                "ASSTCLCE_Id": user1.asstclcE_Id,               
            }
            apiService.create("Adm_School_Master_CE/edit2", data).then(function (Promise) {
                if (Promise != null) {                   
                    $scope.ASSTCLCE_Id = Promise.editdata2[0].asstclcE_Id;
                    $scope.ASMST_Id = Promise.editdata2[0].asmsT_Id;
                    $scope.ASMCL_Id = Promise.editdata2[0].asmcL_Id;
                    $scope.ASMCE_Id = Promise.editdata2[0].asmcE_Id;
                    $scope.asmsT_StreamName = Promise.editdata2[0].asmsT_StreamName;
                    $scope.ASSTCLCE_CompulsoryFlg = Promise.editdata2[0].asstclcE_CompulsoryFlg;

                    $scope.asmcL_ClassName = Promise.editdata2[0].asmcL_ClassName;
                    $scope.asmcE_CEName = Promise.editdata2[0].asmcE_CEName;
                  

                }
            })
        }

        //--Active Deactive--//
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.asmcE_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Adm_School_Master_CE/activedeactive/", user).
                            then(function (promise) {
                                if (promise.message != null) {
                                    swal("You Can't De-activate This Record,Record Already Mapped...!!!");
                                }
                                else {
                                    if (promise.returnval == true) {
                                        swal("Record" + " " + confirmmgs + " " + "Successfully");
                                        $state.reload();
                                    }
                                    else {
                                        swal("Record" + " " + confirmmgs + " " + " Successfully");
                                        $state.reload();
                                    }
                                }
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                    $state.reload();
                });
        }

        //$scope.get_MappedStudent = function (user) {


        //    apiService.create("Adm_School_Master_Stream/getdetails", user).then(function (promise) {
        //        $scope.mappedstudentlist = promise.streamdetails;
        //    })
        //}

        $scope.deactive2 = function (user, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.asstclcE_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Adm_School_Master_CE/deactive2/", user).
                            then(function (promise) {

                                if (promise.message != null) {
                                    swal(promise.message);
                                }
                                else {
                                    if (promise.returnval == true) {
                                        swal(confirmmgs + " " + "Successfully");

                                       

                                        $scope.ASMCE_Id = '';
                                        $scope.BindData();
                                        $state.reload();
                                    }
                                    else {
                                        swal(confirmmgs + " " + " Successfully");
                                        $scope.ASMCE_Id = '';
                                        $scope.BindData();
                                 
                                    }
                                }
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                    $scope.ASMCE_Id = '';
                    $scope.BindData();
                });
        }

        //-Clear-//
        $scope.clearid = function () {
            $state.reload();
        };

        $scope.clearid2 = function () {
            $scope.ASMST_Id = '';
            $scope.ASMCL_Id = '';
            $scope.ASMCE_Id = '';
            $scope.ASSTCLCE_CompulsoryFlg ='0';
          
            $scope.BindData();
        };

    };
})();