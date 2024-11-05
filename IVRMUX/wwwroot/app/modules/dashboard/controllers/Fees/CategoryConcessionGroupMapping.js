(function () {
    'use strict';
    angular
        .module('app')
        .controller('CategoryConcessionGroupMapping', CategoryConcessionGroupMapping)

    CategoryConcessionGroupMapping.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', '$stateParams']
    function CategoryConcessionGroupMapping($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, $stateParams) {


        $scope.savedisable = true;
        $scope.editdisable = true;
        $scope.deletedisable = true;
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
                    $scope.editflg = true;
                    $scope.editbtn = true;

                }
                else {
                    $scope.editflg = false;
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
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;   //set the propertyName to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.Clearid = function () {
            $state.reload();

        };

        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;

        //========classlist CheckBox Field Validation===========//
        $scope.isOptionsRequired = function () {
            return !$scope.headlist.some(function (item) {
                return item.selected;
            });
        }

        //=======selection of checkbox....
        $scope.togchkbxC = function () {

            $scope.usercheckC = $scope.headlist.every(function (role) {
                return role.selected;
            });
        }


        $scope.all_checkC = function () {            
            var toggleStatus = $scope.usercheckC;          
            angular.forEach($scope.headlist, function (role) {
                role.selected = toggleStatus;
            }); 
        }

        //var paginationformasters;
        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        //} else {
        //    paginationformasters = 10;
        //}
        //$scope.currentPage = 1;
        //$scope.itemsPerPage = paginationformasters;

        $scope.search = "";
        $scope.submitted = false;

        $scope.loaddata = function () {

           // var pageid = 2;
            apiService.getURI("CategoryConcessionGroupMapping/loaddata", pageid).then(function (promise) {

                $scope.allacademicyear = promise.acayear;
                $scope.categorylist = promise.concession;
                $scope.grouplist = promise.group;
                $scope.alldata = promise.alldata;
                $scope.ASMAY_Id = promise.asmaY_Id;
            })
        }

        $scope.gethead = function () {
            $scope.usercheckC = 0;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "FMG_Id": $scope.FMG_Id,
                "FMCC_Id": $scope.FMCC_Id,
                "FTI_Id": $scope.FTI_Id
            }
            apiService.create("CategoryConcessionGroupMapping/gethead", data).then(function (promise) {
                if (promise.head.length > 0) {
                    $scope.headlist = promise.head;
                }
                else {
                    swal("No Head is Mapped To Selected Installment");
                    $scope.headlist = [];

                }
            })
        }

        $scope.getgroup = function () {
            $scope.headlist = [];
            $scope.usercheckC = '';
            $scope.FMCC_Id = '';
            $scope.FMG_Id = '';
            $scope.FTI_Id = '';
            $scope.installmentlist = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("CategoryConcessionGroupMapping/getgroup", data).then(function (promise) {
                if (promise.group.length > 0) {
                    $scope.grouplist = promise.group;
                }
                else {
                    swal("No Groups Are Mapped To Selected Year");
                }
            })
        }

        $scope.getconcession = function () {
            $scope.headlistdata = [];
            if ($scope.FMCC_Id == undefined || $scope.FMCC_Id == '' || $scope.FMCC_Id == null) {
                swal('Please Select Category Name!');
                $scope.FMG_Id = '';
            }
            else {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "FMG_Id": $scope.FMG_Id,
                }
                apiService.create("CategoryConcessionGroupMapping/getconcession", data).then(function (promise) {
                    if (promise.conce.length > 0) {
                        $scope.installmentlist = promise.conce;
                    }
                    else {
                        swal("No Installment is Mapped To Selected Group");
                    }
                })
            }
        }

        $scope.deactiveStudent = function (usersem, SweetAlert) {

            var dystring = "";

            if (usersem.fmccG_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (usersem.fmccG_ActiveFlag == false) {
                dystring = "Activate"
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
                        apiService.create("CategoryConcessionGroupMapping/deactiveStudent", usersem).
                            then(function (promise) {
                                if (promise.returnval == true) {


                                    swal("Record " + dystring + "d " + "Successfully!!!");
                                    $state.reload();

                                }
                                else {

                                    swal("Record Not " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }

                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }

                });
        }

        $scope.year = function () {
            angular.forEach($scope.headlist, function (itm) {
                if (itm.selected) {

                }
                $scope.FTI_Id = '';
                $scope.installmentlist = [];
                $scope.headlist = [];
                $scope.usercheckC = '';
                $scope.submitted = false;
                $scope.selected = '';
                $scope.searchchkbx1 = '';
            })
        }

        $scope.forhead = function () {
            angular.forEach($scope.headlist, function (itm) {
                if (itm.selected) {

                }
                $scope.headlist = [];
                $scope.usercheckC = '';
                $scope.submitted = false;
                $scope.selected = '';
                $scope.searchchkbx1 = '';
                $scope.usercheckC = 0;
            })
        }

        //==================================for Edit Records
        //$scope.editstudentflag = false;
        $scope.EditData = function (user) {

            var data = { "FMCCG_Id": user.fmccG_Id }
            apiService.create("CategoryConcessionGroupMapping/EditData", data).then(function (promise) {

                if (promise.editlist.length > 0) {
                    $scope.fmccG_Id = promise.editlist[0].fmccG_Id;
                    $scope.allacademicyear = promise.acayear;
                    $scope.categorylist = promise.concession;
                    $scope.grouplist = promise.group;
                    $scope.installmentlist = promise.conce;
                    $scope.headlist = promise.head;
                    $scope.ASMAY_Id = promise.asmaY_Id;
                    $scope.FMCC_Id = promise.fmcC_Id;
                    $scope.FMG_Id = promise.fmG_Id;
                    $scope.FTI_Id = promise.ftI_Id;
                    angular.forEach($scope.headlist, function (yy) {
                        angular.forEach(promise.editlist, function (uu) {
                            if (yy.fmH_Id == uu.fmH_Id) {
                                yy.selected = true;
                            }
                        })
                    })
                }
            })
        }

        $scope.save = function () {

            if ($scope.myForm.$valid) {
                $scope.headlistdata = [];
                angular.forEach($scope.headlist, function (ty) {
                    if (ty.selected) {
                        $scope.headlistdata.push({
                            fmH_Id: ty.fmH_Id,
                        });
                    }
                })
                var data = {
                    "FMCCG_Id": $scope.fmccG_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "FMG_Id": $scope.FMG_Id,
                    headlistdata: $scope.headlistdata,
                    "FMCC_Id": $scope.FMCC_Id,
                    "FTI_Id": $scope.FTI_Id
                }
                apiService.create("CategoryConcessionGroupMapping/save", data).then(function (promise) {

                    if (promise.msg == "saved") {
                        swal("Record saved Successfully...!!!");
                    }
                    else if (promise.msg == 'update') {
                        swal("Record Updated Successfully...!!!");
                       
                        $state.reload();
                    }
                    else if (promise.msg == 'Duplicate') {
                        swal("Record Already Exist");
                    }
                    else if (promise.msg == "savingFailed") {
                        swal("Failed to Save Record");
                    }
                    else if (promise.msg == "updateFailed") {
                        swal("Failed to Update Record");
                    }
                    else {
                        swal("Not Saved");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;

            }
        }; 
    }
})();