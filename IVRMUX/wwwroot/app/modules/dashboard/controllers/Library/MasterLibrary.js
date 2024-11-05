(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterLibraryController', MasterLibraryController)

    MasterLibraryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterLibraryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;
        $scope.submitted1 = false;
        $scope.submitted2 = false;
        $scope.tabbcl = false;
        $scope.search3 = "";
        $scope.itemsPerPage3 = 10;
        $scope.currentPage3 = 1;


       
        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;
        $scope.search2 = "";

        $scope.MI_SchoolCollegeFlag = '';

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse2 = !$scope.reverse2; //if true make it false and vice versa
        }
      
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse3 = !$scope.reverse3; //if true make it false and vice versa
        }


        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        //=====================Load data..........
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            $scope.usercheckC = false;
          
            debugger;
            var pageid = 2;
            apiService.getURI("MasterLibrary/getdetails", pageid).then(function (promise) {

                $scope.MI_SchoolCollegeFlag = promise.mI_SchoolCollegeFlag;
                if ($scope.MI_SchoolCollegeFlag == 'C' || $scope.MI_SchoolCollegeFlag == 'U') {
                    $scope.tabbcl = true;
                }
                else {
                    $scope.tabbcl = false;
                }
                $scope.alldata = promise.alldata;

                $scope.mappedclass = promise.mappedclass;

                $scope.librylist = promise.librylist;
                $scope.liballdata = promise.liballdata;
                $scope.classlist = promise.classlist;
                $scope.role = promise.role;
                

                //$scope.stafflist = promise.stafflist;
            })
        }
        //=====================End-----Load-data----//



        //=====================saverecord....
        $scope.savedata = function () {
            debugger;
            if ($scope.myForm1.$valid) {
                var data = {
                    "LMAL_Id": $scope.lmaL_Id,
                    "LMAL_LibraryName": $scope.lmaL_LibraryName,
                    //"IVRMUL_Id": $scope.ivrmuL_Id,

                }
                apiService.create("MasterLibrary/Savedata", data)
                    .then(function (promise) {
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.lmaL_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                        $scope.liballdata = promise.liballdata;
                                        $scope.cancel();
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                        $scope.liballdata = promise.liballdata;
                                        $scope.cancel();
                                    }

                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.lmaL_Id > 0) {
                                            swal('Record Not Update Successfully!!!');
                                        }
                                        else {
                                            swal('Record Not Saved Successfully!!!');
                                        }
                                    }
                                }

                            }
                            else {
                                swal("Record already exist");
                            }
                           // $state.reload();
                            $scope.Loaddata();
                        }
                        else {
                            swal("Kindly Contact Administrator!!!");
                        }

                    })

            }
            else {
                $scope.submitted = true;
            }
        };
        //=====================End---saverecord....


        //=====================Edit-record....
        $scope.EditData = function (user) {
            debugger;
            $scope.lmaL_Id = user.lmaL_Id;
            $scope.lmaL_LibraryName = user.lmaL_LibraryName;
            //$scope.ivrmuL_Id = user.ivrmuL_Id;
        }

        $scope.EditstaffData = function (user) {
            debugger;
            var data = {
                "LMAL_Id": user.lmaL_Id,
                "LUL_Id": user.luL_Id,
            }
            apiService.create("MasterLibrary/EditstaffData", data)
                .then(function (promise) {
                    debugger;
                    $scope.editlist = promise.editlist;
                    $scope.lmaL_Id = promise.editlist[0].lmaL_Id;                    
                 
                    $scope.luL_Id = promise.editlist[0].luL_Id,
                  
                    $scope.ivrmrT_Id = promise.editlist[0].ivrmrT_Id;
                    $scope.get_Role($scope.ivrmrT_Id );
                    $scope.ivrmuL_Id = promise.editlist[0].ivrmuL_Id;
                    

                });
        }
        //====================End---edit-record....

        //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            $scope.lmaL_Id = user.lmaL_Id;

            var dystring = "";
            if (user.lmaL_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.lmaL_ActiveFlag == 0) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MasterLibrary/deactiveY", user).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");

                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");

                                }
                                // $state.reload();
                                $scope.Loaddata();
                            })

                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }

                });
        }
        //================End----Activation/Deactivation--Record.........


        //=================Activation/Deactivation--Record.........
        $scope.deactiveYstf = function (user, SweetAlert) {
            debugger;
            $scope.luL_Id = user.luL_Id;

            var dystring = "";
            if (user.luL_ActiveFlg == 1) {
                dystring = "Deactivate";
            }
            else if (user.luL_ActiveFlg == 0) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MasterLibrary/deactiveYstf", user).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                // $state.reload();
                                $scope.Loaddata();
                            })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }
                });
        }
        //================End----Activation/Deactivation--Record.........

        //==========================Save Staff Mapping data

        $scope.saveclassdata = function () {
            debugger;
            $scope.classlstdata = [];
            if ($scope.myForm2.$valid) {

                if ($scope.classlist.length > 0) {
                    angular.forEach($scope.classlist, function (cls) {
                        if (cls.selected == true) {
                            $scope.classlstdata.push(cls);
                        }
                    });

                    var data = {
                        "LLC_Id": $scope.llC_Id,
                        "LMAL_Id": $scope.lmaL_Id,
                        "classlst": $scope.classlstdata,
                    }
                    apiService.create("MasterLibrary/saveclassdata", data)
                        .then(function (promise) {
                            if (promise.returnval != null) {
                                if (promise.returnval == true) {
                                    swal('Record Saved/Updated Successfully!!!');
                                    $scope.mappedclass = promise.mappedclass;
                                    $scope.cancel();
                                }
                                else {
                                    swal('Record Not Saved/Updated Successfully!!!');
                                }
                            }
                            else {
                                swal("Kindly Contact Administrator!!!");
                            }
                        })
                }
                else {
                    swal('Select Class!');
                }
            }
            else {
                $scope.submitted1 = true;
            }

        };



        $scope.savestaffdata = function () {
            debugger;
            $scope.classlstdata = [];
            if ($scope.myForm3.$valid) {

               
                    var data = {
                        "LUL_Id": $scope.luL_Id,
                        "LMAL_Id": $scope.lmaL_Id,
                        "IVRMUL_Id": $scope.ivrmuL_Id,
                     
                    }
                    apiService.create("MasterLibrary/savestaffdata", data)
                        .then(function (promise) {
                            if (promise.returnval != null && promise.duplicate != null) {
                                if (promise.duplicate == false) {
                                    if (promise.returnval == true) {
                                        if ($scope.luL_Id > 0) {
                                            swal('Record Updated Successfully!!!');
                                            $scope.alldata = promise.alldata;
                                            $scope.lmaL_Id = "";
                                            $scope.ivrmrT_Id="";
                                            $scope.ivrmuL_Id="";
                                            
                                        }
                                        else {
                                            swal('Record Saved Successfully!!!');
                                            $scope.alldata = promise.alldata;
                                            $scope.Loaddata();
                                            $scope.lmaL_Id = "";
                                            $scope.ivrmrT_Id = "";
                                            $scope.ivrmuL_Id = "";
                                        }

                                    }
                                    else {
                                        if (promise.returnval == false) {
                                            if ($scope.luL_Id > 0) {
                                                swal('Record Not Update Successfully!!!');
                                                
                                            }
                                            else {
                                                swal('Record Not Saved Successfully!!!');
                                            }
                                        }
                                    }

                                }
                                else {
                                    swal("Record already exist");
                                }
                                // $state.reload();
                                $scope.Loaddata();
                            }
                            else {
                                swal("Kindly Contact Administrator!!!");
                            }

                        })

                }
           
            else {
                $scope.submitted2 = true;
            }
        };


        $scope.EditclassData = function (user) {
            var data = {
                "LMAL_Id": user.lmaL_Id,                
            }
            apiService.create("MasterLibrary/EditclassData", data).
                then(function (promise) {

                    $scope.editlist = promise.editlist;
                    $scope.lmaL_Id = promise.editlist[0].lmaL_Id;
                    $scope.asmcL_Id = promise.editlist[0].asmcL_Id;
                    $scope.classlist = promise.classlist;

                
                   
                    angular.forEach($scope.classlist, function (ss) {
                        angular.forEach($scope.editlist, function (tt) {
                            if (tt.asmcL_Id == ss.asmcL_Id) {
                                ss.selected = true;
                            }
                        })
                    })

                    $scope.usercheckC = $scope.classlist.every(function (role) {
                        return role.selected;
                    });

                });

        }



        $scope.onmodelclick = function (obj) {
            var data = {
                "LMAL_Id": obj.lmaL_Id,
                "IVRMUL_Id": obj.ivrmuL_Id,
            }
            apiService.create("MasterLibrary/modalclsslst", data).
                then(function (promise) {

                    $scope.clssslistdata = promise.clssslist;
                });
        }

        $scope.get_MappedClasslist = function (obj) {
            var data = {
                "LMAL_Id": obj.lmaL_Id,               
            }
            apiService.create("MasterLibrary/get_MappedClasslist", data).
                then(function (promise) {

                    $scope.listclassdetails = promise.listclassdetails;

                });
            // $state.reload();
            $scope.Loaddata();
        }



        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };


       

        $scope.cancel = function () {
            $scope.lmaL_Id = "";
            $scope.ivrmuL_Id = "";
            $scope.ivrmrT_Id = "";
            $scope.lmaL_LibraryName = "";
            $scope.usercheckC = false;           
            $scope.tempclass = $scope.classlist;
            angular.forEach($scope.classlist, function (itm1) {
                itm1.selected = false;
            })
            $scope.submitted2 = false;
            $scope.submitted1 = false;
            $scope.submitted = false;

        }

        //========classlist CheckBox Field Validation===========//
        $scope.isOptionsRequired = function () {
            return !$scope.classlist.some(function (item) {
                return item.selected;
            });
        }


        //=======selection of checkbox....
        $scope.togchkbxC = function () {

            $scope.usercheckC = $scope.classlist.every(function (role) {
                return role.selected;
            });
        }


        //---------all checkbox Select...
        $scope.all_checkC = function (all) {

            $scope.usercheckC = all;
            var toggleStatus = $scope.usercheckC;
            angular.forEach($scope.classlist, function (role) {
                role.selected = toggleStatus;
            });
        }


        //=================Activation/Deactivation--Record.........
        $scope.deactivclsdata = function (user, SweetAlert) {
            debugger;
            $scope.llC_Id = user.llC_Id;

            var dystring = "";
            if (user.llC_ActiveFlg == 1) {
                dystring = "Deactivate";
            }
            else if (user.llC_ActiveFlg == 0) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MasterLibrary/deactivclsdata", user).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                // $state.reload();
                                $scope.Loaddata();
                                $scope.get_MappedClasslist(user);
                            })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }
                });
        }
        //================End----Activation/Deactivation--Record.........


        //=================================Role
       
        $scope.get_Role = function (id) {

            var data = {
                "IVRMRT_Id" : id,
            }
            apiService.create("MasterLibrary/getusername", data).
                then(function (promise) {
                    if (promise.stafflist.length > 0) {
                       
                        $scope.stafflist = promise.stafflist;
                    } else {
                        swal('No match found!');
                        
                    }
                })
        }
        //=============================================================End

        //=================================Role
        $scope.check_userclass = function () {
            debugger;
            var data = {
                "IVRMUL_Id": $scope.ivrmuL_Id,
                "LMAL_Id": $scope.lmaL_Id,
            }
            apiService.create("MasterLibrary/check_userclass", data).
                then(function (promise) {
                    if (promise.clsdata.length > 0) {                       
                        $scope.clsdata = promise.clsdata;


                        angular.forEach($scope.classlist, function (ss) {
                            angular.forEach($scope.clsdata, function (tt) {
                                if (tt.asmcL_Id == ss.asmcL_Id) {
                                    ss.selected = true;
                                }
                            })
                        })

                        $scope.usercheckC = $scope.classlist.every(function (role) {
                            return role.selected;
                        });
                    } 
                })
        }
        //=============================================================End
        
    }
})();

