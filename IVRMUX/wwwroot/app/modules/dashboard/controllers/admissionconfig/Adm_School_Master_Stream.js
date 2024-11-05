
(function () {
    'use strict';
    angular
        .module('app')
        .controller('Adm_School_Master_Stream', Adm_School_Master_Stream)

    Adm_School_Master_Stream.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function Adm_School_Master_Stream($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {         
        
        $scope.itemsPerPage = 5;
        $scope.itemsPerPage2 = 5;
        $scope.currentPage = 1;
        $scope.currentPage2 = 1;
        $scope.searchValue = "";
        $scope.searchValue2 = "";
        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("Adm_School_Master_Stream/getdata", pageid).then(function (promise) {
          
                //$scope.categorylist = promise.concession;
                //$scope.categorylist3 = promise.concession3;
                $scope.editedit = false;
                $scope.streamlist = promise.streamlist;
                $scope.classlist = promise.classlist;
                $scope.sectionlist = promise.sectionlist;


                if (promise != null) {
                    $scope.mastervehicle = promise.mastervehicle;
                    $scope.totcountfirst = $scope.mastervehicle.length;

                    $scope.mastervehicle2 = promise.mastervehicle2;
                   $scope.totcountfirst2 = $scope.mastervehicle2.length;


                //    $scope.mastervehicle3 = promise.savedata33;
                //    $scope.totcountfirst3 = $scope.mastervehicle3.length;


                //    $scope.grouplist = promise.group;
                }
                else {
                    swal("No Records Found");
                }
            })

        }

        // checkbox
        $scope.searchchkbx = "";
        $scope.all_check = function () {

            var checkStatus = $scope.usercheck;
            angular.forEach($scope.sectionlist, function (itm) {
                itm.select = checkStatus;
            });
        }
        $scope.filterchkbx = function (obj) {
            return (angular.lowercase(obj.asmC_SectionName)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }
        $scope.togchkbx = function () {

            $scope.usercheck = $scope.sectionlist.every(function (options) {
                return options.select;
            });
        }
        $scope.isOptionsRequired = function () {
            return !$scope.sectionlist.some(function (options) {
                return options.select;
            });
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
                  
                    "ASMST_Id": $scope.asmsT_Id,
                    "ASMST_StreamName": $scope.ASMST_StreamName,
                    "ASMST_StreamCode": $scope.ASMST_StreamCode
                }
                apiService.create("Adm_School_Master_Stream/savedata", data).then(function (promise) {
                
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

        $scope.editedit = false;
        $scope.submitted2 = false;
        $scope.submitted3 = false;
        $scope.savedata2 = function () {
          
            $scope.selectedsectionlist = [];
            if ($scope.myform2.$valid) {
               
                angular.forEach($scope.sectionlist, function (year) {
                    if (year.select == true) {
                        $scope.selectedsectionlist.push({ asmS_Id: year.asmS_Id });
                    }
                });
                var data = {                    
                    "editedit": $scope.editedit,
                    "ASMST_Id": $scope.ASMST_Id,
                    "ASSTCL_Id": $scope.ASSTCL_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    selectedsectionlist: $scope.selectedsectionlist,
                }
                apiService.create("Adm_School_Master_Stream/savedata2", data).then(function (promise) {
                

                    if (promise.message == "Add") {
                        swal("Data Saved Successfully...!!!");
                   

                    }
                    else if (promise.message == "notAdd") {
                        swal("Data Not Saved Successfully...!!!");
                  
                    }
                    else if (promise.message == "Duplicate") {


                        swal("Data Already Exist...!!!");
                    
                    }
                    else if (promise.message == "Update2") {
                        swal("Data Updated Successfully...!!!");
                     
                    }
                    else if (promise.message == "notUpdate2") {
                        swal("Data Not Updated Successfully...!!!");
                   
                    }
                   
                    $scope.ASMST_Id = '';                 
                    $scope.ASSTCL_Id = 0;
                    $scope.ASMCL_Id = '';
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
            return (angular.lowercase(obj.asmsT_StreamName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.asmsT_StreamCode)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        $scope.filterValue2 = function (obj) {
            return (angular.lowercase(obj.asmsT_StreamName)).indexOf(angular.lowercase($scope.searchValue2)) >= 0
                ||
            (angular.lowercase(obj.asmcL_ClassName)).indexOf(angular.lowercase($scope.searchValue2)) >= 0
                ||
            (angular.lowercase(obj.asmC_SectionName)).indexOf(angular.lowercase($scope.searchValue2)) >= 0
                
        }
        //---Edit Data--//
        $scope.edit = function (user) {
            var data = {
                "ASMST_Id": user.asmsT_Id
            }

            apiService.create("Adm_School_Master_Stream/editdata", data).then(function (Promise) {
             
                if (Promise != null) {
                    $scope.asmsT_Id = Promise.editdata[0].asmsT_Id;
                    $scope.ASMST_StreamName = Promise.editdata[0].asmsT_StreamName;
                    $scope.ASMST_StreamCode = Promise.editdata[0].asmsT_StreamCode;
                    
                }
            })
        }

        $scope.edit2 = function (user1) {
            $scope.ASMCL_Id = user1.asmcL_Id;
            $scope.ASMST_Id = user1.asmsT_Id;
            var data = {
                "ASMCL_Id": user1.asmcL_Id,
                "ASMST_Id": user1.asmsT_Id,
            }
            apiService.create("Adm_School_Master_Stream/edit2", data).then(function (Promise) {
                if (Promise != null) {

                    $scope.editedit = true;
                    //$scope.ASMST_Id = Promise.editdata2[0].asmsT_Id;
                    //$scope.ASSTCL_Id = Promise.editdata2[0].asstcL_Id;
                    //$scope.asmsT_StreamName = Promise.editdata2[0].asmsT_StreamName;
                    //$scope.asmcL_ClassName = Promise.editdata2[0].asmcL_ClassName;
                    //$scope.asmC_SectionName = Promise.editdata2[0].asmC_SectionName;
                    angular.forEach($scope.sectionlist, function (yy) {
                        angular.forEach(Promise.sectionlistedit, function (uu) {
                            if (yy.asmS_Id == uu.asmS_Id) {
                                yy.select = true;
                            }
                        })
                    })

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
            if (user.asmsT_ActiveFlag === true) {
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
                        apiService.create("Adm_School_Master_Stream/activedeactive/", user).
                            then(function (promise) {
                              
                                if (promise.message != null) {
                                    swal("You Can't De-activate This Record,Record Already Mapped...!!!");
                                }
                                else {
                                    if (promise.returnval == true) {
                                        swal("Record"+" "+confirmmgs + " " + "Successfully");
                                        $state.reload();
                                    }
                                    else {
                                        swal("Record"+" "+confirmmgs + " " + " Successfully");
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

        $scope.get_MappedStudent = function (user) {
           
           
            apiService.create("Adm_School_Master_Stream/getdetails", user).then(function (promise) {
                $scope.mappedstudentlist = promise.streamdetails;
            })
        }

        $scope.deactive_student = function (user, SweetAlert) {
          
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.asstcL_ActiveFlag === true) {
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
                        apiService.create("Adm_School_Master_Stream/deactive2/", user).
                            then(function (promise) {
                              
                                if (promise.message != null) {
                                    swal(promise.message);
                                }
                                else {
                                    if (promise.returnval == true) {
                                        swal(confirmmgs + " " + "Successfully");
                                      
                                       

                                        $scope.ASMST_Id = '';                                      
                                        $scope.mappedstudentlist = promise.streamdetails;
                                       // $scope.get_MappedStudent();
                                        $scope.BindData();
                                    }
                                    else {
                                        swal(confirmmgs + " " + " Successfully");
                                        $scope.ASMST_Id = ''; 
                                        $scope.mappedstudentlist = promise.streamdetails;
                                       // $scope.get_MappedStudent();
                                        $scope.BindData();
                                       
                                        
                                    }
                                }
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                    $scope.ASMST_Id = '';                 
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
          
            $scope.BindData();
        };
        
    };
})();