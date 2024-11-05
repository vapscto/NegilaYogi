
(function () {
    'use strict';
    angular
.module('app')
        .controller('RoomMappingController', RoomMappingController)

    RoomMappingController = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function RoomMappingController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {


        $scope.temp_grid = [];


        //to get class by category      
   
        $scope.get_class1 = function () {
            debugger;
            if ($scope.ASMAY_Id === "") {
                swal("Please Select The Academic Year !");
                $scope.ttmC_Id = "";
            }
            else if ($scope.TTMC_Id != "" && $scope.ASMAY_Id != "") {
                var data = {
                    "TTMC_Id": $scope.TTMC_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                }
                apiService.create("ClassWiseTT/getclass_catg", data).
                    then(function (promise) {
                        debugger;
                        $scope.class_list = promise.classlist;
                        $scope.asmcL_Id = "";
                        $scope.usercheck = 0;
                        $scope.cls_flag = false;
                        if (promise.classlist == "" || promise.classlist == null) {
                            swal("No classes Are Mapped To Selected Category");
                        }
                    })
            }
        };

        $scope.onyrchange = function () {
            $scope.temp_grid = [];
            $scope.tempmainarray = [];
            $scope.TTMC_Id = '';
            $scope.AMCO_Id = '';
            $scope.AMSE_Id = '';
            $scope.AMB_Id = '';
            $scope.ACMS_Id = '';
            $scope.grid_view = false;


        }
        $scope.tempgridarray = [];
        $scope.addgrid = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.flistdat = [];
                angular.forEach($scope.subjectlist, function (ss) {
                    if (ss.stf == true) {
                        $scope.flistdat.push({ ismS_Id: ss.ismS_Id, ismS_SubjectName: ss.ismS_SubjectName});
                    }

                })

                if ($scope.flistdat.length > 0) {


                    var cors = "";
                    var bran = "";
                    var sem = "";
                    var sect = "";
                    var subj = "";
                    var year = "";
                    var rmm = "";

                    angular.forEach($scope.academic, function (cc) {
                        if (cc.asmaY_Id == $scope.ASMAY_Id) {
                            year = cc.asmaY_Year;
                        }
                    })

                    angular.forEach($scope.class_list, function (cc) {
                        if (cc.asmcL_Id == $scope.ASMCL_Id) {
                            cors = cc.asmcL_ClassName;
                        }
                    })

                   
                    angular.forEach($scope.section_list, function (scc) {
                        if (scc.asmS_Id == $scope.ASMS_Id) {
                            sect = scc.asmC_SectionName;
                        }
                    })
                    angular.forEach($scope.roomlist, function (scc) {
                        if (scc.ttmrM_Id == $scope.TTMRM_Id) {
                            rmm = scc.ttmrM_RoomName;
                        }
                    })
                 
                 


                    angular.forEach($scope.flistdat, function (zz) {

                        angular.forEach($scope.subjectlist, function (sbb) {
                            if (sbb.ismS_Id == zz.ismS_Id) {
                                subj = sbb.ismS_SubjectName;
                            }
                        })

                        var count = 0;
                        angular.forEach($scope.tempgridarray, function (mm) {
                            if (mm.ASMAY_Id == $scope.ASMAY_Id && mm.AMCO_Id == $scope.AMCO_Id && mm.AMB_Id == $scope.AMB_Id && mm.AMSE_Id == $scope.AMSE_Id && mm.ACMS_Id == $scope.ACMS_Id && zz.ismS_Id == mm.ISMS_Id) {
                                count += 1;

                            }
                      


                        });
                        if (count == 0) {
                            var id = $scope.tempgridarray.length;
                            $scope.tempgridarray.push({
                                ID: id + 1, ASMAY_Id: $scope.ASMAY_Id, ASMCL_Id: $scope.ASMCL_Id, ASMS_Id: $scope.ASMS_Id, ISMS_Id: zz.ismS_Id, TTMRM_Id: $scope.TTMRM_Id, asmaY_Year:year ,crssName: cors,
                                secname: sect,
                                subjectname: subj, room: rmm })
                        }

                    });


                    $scope.ASMAY_Id = "";
                    $scope.TTMC_Id = "";
                    $scope.ASMCL_Id = "";
                    $scope.ASMS_Id = "";
                    $scope.HRME_Id = "";
                    $scope.ISMS_Id = "";
                    $scope.AMSE_Id = "";
                    $scope.ACMS_Id = "";

                    angular.forEach($scope.subjectlist, function (eee) {

                        eee.stf = false;
                        

                    })
                }


                console.log($scope.tempgridarray);


            }

        }

        $scope.itemsPerPage1 = 15;
        $scope.currentPage1 = 1;

        $scope.DelTemp = function (obj) {

            for (var i = $scope.tempgridarray.length - 1; i >= 0; i--) {
                if ($scope.tempgridarray[i].ID == obj.ID) {
                    $scope.tempgridarray.splice(i, 1);
                }
            }

        }

        //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            var dystring = "";
            if (user.TTCSRM_ActiveFlg == 1) {
                dystring = "Deactivate";
            }
            else if (user.TTCSRM_ActiveFlg == 0) {
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
                        apiService.create("RoomMapping/deactiveY", user).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");

                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");

                                }
                                $state.reload();
                            })

                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }

                });
        }
         //================End----Activation/Deactivation--Record.........

        $scope.grid_view = false;
        $scope.disble = false;
        $scope.gridOptions = {};
        $scope.datareport = false;
        $scope.submitted = false;
        $scope.GetReport = function () {
            $scope.temp_grid = [];
            $scope.tempmainarray = [];
            $scope.savearray = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                angular.forEach($scope.perioddetails, function (ee) {

                    if (ee.checkedvalue == true) {
                        $scope.savearray.push(ee);
                    }
                })
                if ($scope.savearray.length > 0) {
          

                var data = {
                    "TTCSRM_Id": $scope.TTCSRM_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "TTMD_Id": $scope.TTMD_Id,
                    "TTMC_Id": $scope.TTMC_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    savedata: $scope.savearray
                }
                apiService.create("RoomMapping/savedetail", data).
                    then(function (promise) {
                        if (promise.returnval == true) {
                            swal('Record Saved/Updated Successfully');
                                $state.reload();
                            }
                            else {
                                swal('Error');
                            }

                        })
                }
                else {
                    swal('Select One Record');
                }
            }


        };
        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.ismS_SubjectName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }
        $scope.togchkbx = function () {
            $scope.usercheck = $scope.subjectlist.every(function (options) {
                return options.stf;
            });
        }
        $scope.all_check = function () {
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.subjectlist, function (itm) {
                itm.stf = toggleStatus;
            });

        }

        $scope.get_roomfacility = function (employee) {
            debugger;
            apiService.create("CLGRoomMapping/get_roomfacility", employee).
                then(function (promise) {

                    employee.fcdetails = promise.facilitylist;

                })
        }
        $scope.getperioddetails = function () {
            //$scope.temp_grid = [];
            $scope.perioddetails1 = [];
            $scope.existingperioddetails = [];
            $scope.perioddetails = [];
            debugger;
            $scope.submitted = true;
            // if ($scope.myForm.$valid) {

            var data = {
                "TTCSRM_Id": $scope.TTCSRM_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "TTMD_Id": $scope.TTMD_Id,
                "TTMC_Id": $scope.TTMC_Id,
                "ASMS_Id": $scope.ASMS_Id,
            }
            apiService.create("RoomMapping/getpossiblePeriod", data).
                then(function (promise) {
                    $scope.perioddetails1 = promise.perioddetails;
                    $scope.existingperioddetails = promise.existingperioddetails;
                    if ($scope.perioddetails1.length > 0) {

                        angular.forEach($scope.perioddetails1, function (tt) {
                            var cntt = 0;
                            angular.forEach($scope.existingperioddetails, function (rr) {

                                if (parseInt(rr.ASMAY_Id) == parseInt(tt.ASMAY_Id) && parseInt(rr.ASMCL_Id) == parseInt(tt.ASMCL_Id) && parseInt(rr.ASMS_Id) == parseInt(tt.ASMS_Id)  && parseInt(rr.TTMD_Id) == parseInt(tt.TTMD_Id) && parseInt(rr.TTMP_Id) == parseInt(tt.TTMP_Id) && parseInt(rr.ISMS_Id) == parseInt(tt.ISMS_Id)) {
                                    cntt += 1;

                                }


                            })


                            if (cntt == 0) {
                                $scope.perioddetails.push(tt);
                            }

                        })

                        if ($scope.perioddetails.length == 0) {
                            swal('No Record Found');
                        }

                    }
                    else {
                        swal('No Record Found');
                    }


                    //angular.forEach($scope.perioddetails, function (mm) {
                    //    mm.

                    //})

                })
            //  }

        };


        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.perioddetails, function (itm) {
                itm.checkedvalue = toggleStatus;
            });
        }
        $scope.optionToggled = function (user) {
            $scope.all = $scope.perioddetails.every(function (itm) { return itm.checkedvalue; })

            if (user.checkedvalue == false) {
                user.TTMRM_Id = '';
            }

        }

        $scope.canceldata = function () {
            $state.reload();
        }


        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            debugger;
            apiService.getDATA("RoomMapping/getalldetails").
       then(function (promise) {

           $scope.clearid();
           $scope.academic = promise.academiclist;
           $scope.categorylst = promise.catelist;
           $scope.stafflst = promise.staffDrpDwn;
           $scope.temp_classlist = promise.classlist;
           $scope.subjectlist = promise.subjectlist;
           $scope.class_list = promise.classlist;
           $scope.section_list = promise.sectionlist;
           $scope.roomlist = promise.roomlist;
           $scope.datalst = promise.datalst;
           $scope.daylist = promise.daylist;
       })
        };

        $scope.isOptionsRequired = function () {

            return !$scope.subjectlist.some(function (options) {
                return options.stf;
            });
        }

        $scope.cleariddd = function () {

            $scope.ASMAY_Id = '';
            $scope.TTMC_Id = "";
            $scope.TTMD_Id = "";
            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
          
          
            $scope.TTCSRM_Id = 0;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.submitted = false;
            $scope.disble = false;
            $scope.perioddetails = [];


        };
        //TO clear  data
        $scope.clearid = function () {

            $scope.ASMAY_Id = "";
            $scope.TTMC_Id = "";
            $scope.HRME_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.grid_view = false;
            $scope.submitted = false;
            $scope.datareport = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $scope.from = "";
            $scope.to = "";


        };
        $scope.from = "";
        $scope.to = "";
        $scope.tempmainarray = [];
       


        $scope.EditData = function (employee) {
            debugger;
            $scope.disble = true;
            apiService.create("RoomMapping/editdata", employee).
                then(function (promise) {

                   $scope.class_list = promise.courselist;
                 
                    

                    $scope.TTCSRM_Id = promise.editlist[0].ttcsrM_Id;
                    $scope.ASMAY_Id = promise.editlist[0].asmaY_Id;

                    if ($scope.ASMAY_Id != undefined && $scope.ASMAY_Id != null && $scope.ASMAY_Id && $scope.ASMAY_Id != '') {
                        $scope.onyrchange();
                    }



                    $scope.TTMC_Id = promise.ttmC_Id;
                    $scope.ASMCL_Id = promise.editlist[0].asmcL_Id;
                    $scope.ASMS_Id = promise.editlist[0].asmS_Id;
                    $scope.TTMRM_Id = promise.editlist[0].ttmrM_Id;

                    $scope.TTMD_Id = promise.editlist[0].ttmD_Id;
                    $scope.perioddetails = promise.perioddetails;


                    angular.forEach($scope.perioddetails, function (ee) {
                        ee.checkedvalue = true;

                        $scope.get_roomfacility(ee);

                    })


                })

        }






        //to get class by category      
        $scope.ttmC_Id = "";
        $scope.get_class = function () {
            if ($scope.ttmC_Id != "") {
                var data = {
                    "TTMC_Id": $scope.ttmC_Id,

                }
                apiService.create("RoomMapping/getclass_catg", data).
        then(function (promise) {

            $scope.class_list = promise.classlist;
            $scope.ASMCL_Id = "";
            if (promise.classlist == "" || promise.classlist == null) {
                swal("No classes Are Mapped To Selected Category");
            }
        })
            }
        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.get_category = function () {

            if ($scope.asmaY_Id === "") {
                swal("Please Select the Academic Year !");
            }
            else {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                }
                apiService.create("RoomMapping/get_catg", data).
         then(function (promise) {

             $scope.categorylst = promise.catelist;
             $scope.asmcL_Id = "";
             if (promise.catelist === "" || promise.catelist === null) {
                 swal("No Category Are Mapped To Selected Academic Year");
             }
         })
            }
        };

      
    }
})();