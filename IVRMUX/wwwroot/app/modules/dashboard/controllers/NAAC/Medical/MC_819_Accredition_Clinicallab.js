(function () {
    'use strict';
   
    angular
        .module('app')
        .controller('MC_819_Accredition_ClinicallabController', MC_819_Accredition_ClinicallabController);

    MC_819_Accredition_ClinicallabController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce', 'myFactorynaac'];

    function MC_819_Accredition_ClinicallabController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce, myFactorynaac) {
        $scope.searc_button = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        var miid = myFactorynaac.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.search = "";
        $scope.institute_flag = false;
        //=======================Page Load
        $scope.loaddata = function () {
            $scope.NCMCCL819_Id = 0;
            $scope.change_institution();
            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }
            apiService.getURI("MC_819_Accredition_Clinicallab/loaddata", $scope.mI_Id).then(function (promise) {
            
                $scope.institutionlist = promise.institutionlist;
                $scope.mI_Id = promise.mI_Id;
                $scope.yearlist = promise.yearlist;
                $scope.alldata = promise.alldata819MC;

            })
        }

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        //========================
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.submitted = false;
        $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {

                if ($scope.check1 == '1') {
                    $scope.NCMCCL819_NABHAccnTechHoslFlg = true;
                }
                else {
                    $scope.NCMCCL819_NABHAccnTechHoslFlg = false;
                }
                if ($scope.check2 == '1') {
                    $scope.NCMCCL819_NABHAccnTechlabslFlg = true;
                }
                else {
                    $scope.NCMCCL819_NABHAccnTechlabslFlg  = false;
                }
                if ($scope.check3 == '1') {
                    $scope.NCMCCL819_CertificationDeptlFlg = true;
                }
                else {
                    $scope.NCMCCL819_CertificationDeptlFlg = false;
                }
                if ($scope.check4 == '1') {
                    $scope.NCMCCL819_OtherRecAccCertificationFlg = true;
                }
                else {
                    $scope.NCMCCL819_OtherRecAccCertificationFlg  = false;
                }

                var data = {
                    "MI_Id": $scope.mI_Id,
                    "NCMCCL819_Id": $scope.ncmccL819_Id,
                    "ASMAY_Id": $scope.NCMCCL819_Year,
                    "NCMCCL819_NABHAccnTechHoslFlg": $scope.NCMCCL819_NABHAccnTechHoslFlg,
                    "NCMCCL819_NABHAccnTechlabslFlg": $scope.NCMCCL819_NABHAccnTechlabslFlg,
                    "NCMCCL819_CertificationDeptlFlg": $scope.NCMCCL819_CertificationDeptlFlg,
                    "NCMCCL819_OtherRecAccCertificationFlg": $scope.NCMCCL819_OtherRecAccCertificationFlg,
                   
                }

                apiService.create("MC_819_Accredition_Clinicallab/savedata", data).then(function (promise) {
                    debugger;
                    if (promise.duplicate == false) {
                        if (promise.returnval == true) {
                            if ($scope.ncmccL819_Id > 0) {
                                swal('Record Updated Successfully!');
                            }
                            else {
                                swal('Record Saved Successfully!');
                            }
                            $state.reload();
                        }
                        else {
                            if ($scope.ncmccL819_Id > 0) {
                                swal('Record Nolt Updated Successfully!');
                            }
                            else {
                                swal('Record Not Saved Successfully!');
                            }
                        }
                    }
                    else {
                        swal('Record Already Exist!');
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }



        //=========================Edit For Tab2 Mapping data
      
        $scope.edittab2 = function (user) {
            debugger;
            var data = {
                "NCMCCL819_Id": user.ncmccL819_Id,
                "MI_Id": user.mI_Id,
            }
            apiService.create("MC_819_Accredition_Clinicallab/editdata", data).then(function (promise) {
                debugger;
                $scope.institute_flag = true;
                if (promise.editlisttab1[0].ncmccL819_StatusFlg == "approved") {
                    
                }
                $scope.editdata = promise.editdata;
               
                $scope.NCMCCL819_Id = promise.editdata[0].ncmccL819_Id;
                //$scope.mI_Id = promise.editdata[0].mI_Id;
                $scope.NCMCCL819_Year = promise.editdata[0].ncmccL819_Year;

                if (promise.editdata[0].ncmccL819_NABHAccnTechHoslFlg == true) {
                    $scope.check1 = 1;
                }
                if (promise.editdata[0].ncmccL819_NABHAccnTechlabslFlg == true) {
                    $scope.check2 = 1;
                }
                if (promise.editdata[0].ncmccL819_CertificationDeptlFlg == true) {
                    $scope.check3 = 1;
                }
                if (promise.editdata[0].ncmccL819_OtherRecAccCertificationFlg == true) {
                    $scope.check4 = 1;
                }
                
            })
        }
        //===========deactive and active for Tab1
        $scope.deactivYTab1 = function (usersem, SweetAlert) {
            $scope.NCMCCL819_Id = usersem.ncmccL819_Id
            var dystring = "";
            if (usersem.ncmccL819_ActiveFlag == true) {
                dystring = "Deactivated";
            }
            else if (usersem.ncmccL819_ActiveFlag == false) {
                dystring = "Activated";
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
                        apiService.create("MC_819_Accredition_Clinicallab/deactivate", usersem).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + " Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + " Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }
        //==========================cancel Button  for Tab2
        $scope.canceltab = function () {
            $state.reload();
        }

        $scope.change_institution = function () {
           // $scope.materaldocuupload = [{ id: 'Materal1' }];
            $scope.ASMAY_Id = "";
            $scope.NCMCCL819_Id = 0;
            $scope.NCMCCL819_NABHAccnTechHoslFlg = '';
            $scope.NCMCCL819_NABHAccnTechlabslFlg = '';
            $scope.NCMCCL819_CertificationDeptlFlg = '';
            $scope.NCMCCL819_OtherRecAccCertificationFlg = '';
            $scope.NCMCCL819_Year = '';
            //$scope.submitted = false;
        }

        //=======added comments and status flag for data=========//
        $scope.getlocationaldata = function (obj) {

            apiService.create("MC_819_Accredition_Clinicallab/getcomment", obj).then(function (promise) {

                if (promise !== null) {

                    $scope.commentlist = promise.commentlist;
                }
            });
        };
        // for comment
        $scope.addcomments = function (obje) {

            $scope.ccc = obje.ncmccL819_Id;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };
        //========= Save DATA WISE Comments 
        $scope.savedatawisecomments = function (obj) {

            console.log("Save Comments");
            console.log(obj);

            var data = {
                "Remarks": obj.generalcomments,
                "filefkid": $scope.ccc
            };

            apiService.create("MC_819_Accredition_Clinicallab/savecomments", data).then(function (promise) {

                if (promise !== null) {
                    if (promise.returnval === true) {
                        swal("Comments Saved Successfully");
                    } else {
                        swal("Failed To Save Comments");
                    }
                    $('#mymodaladdcomments').modal('hide');
                    $('#mymodalviewuploaddocument').modal('hide');
                    $scope.valued = "2";
                    $scope.onload();
                }
            });
        };

    }

})();
