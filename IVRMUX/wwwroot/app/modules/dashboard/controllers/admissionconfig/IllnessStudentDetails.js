(function () {
    'use strict';
    angular.module('app').controller('IllnessStudentDetailsController', IllnessStudentDetailsController)

    IllnessStudentDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$stateParams', '$filter', 'Excel', '$timeout']
    function IllnessStudentDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $stateParams, $filter, Excel, $timeout) {

        $scope.edit = false;
        $scope.excel_flag = true;
        $scope.excel_flag = true;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        if (privlgs !== null && privlgs.length > 0) {
            for (var i = 0; i < privlgs.length; i++) {
                if (privlgs[i].pageId == pageid) {
                    $scope.userPrivileges = privlgs[i];
                    console.log($scope.userPrivileges);
                }
            }
        }

        $scope.obj = {};
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        $scope.maxdate = new Date();

        $scope.OnLoadVaccineStudentDetails = function () {
            var pageid = 2;
            apiService.getURI("VaccineAgeCriteria/OnLoadIllnessStudentDetails", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.GetAccademicYear = promise.getAccademicYear;
                    $scope.GetAgeCriteriaStudentDetails = promise.getAgeCriteriaStudentDetails;
                    $scope.Getstudentvaccinedetails = promise.getstudentvaccinedetails;
                    $scope.ASMAY_Id = promise.asmaY_Id;
                }
            });
        };

        $scope.searchfilter = function (objj) {
            var data = "";
            if (objj.search.length >= '3') {
                $scope.studentlst = "";
                data = {
                    "searchfilter": objj.search,
                    "ASMAY_Id": $scope.ASMAY_Id
                };

                apiService.create("VaccineAgeCriteria/GetStudentDetailsBySearch", data).then(function (promise) {
                    if (promise.getStudentSearchList !== null || promise.getStudentSearchList.length > 0) {
                        $scope.GetStudentSearchList = promise.getStudentSearchList;
                    } else {
                        $scope.AMST_Id = "";
                        swal("No students are found for your search");
                    }
                });
            }
        };

        $scope.OnChangeYear = function () {
            $scope.GetStudentSearchList = [];
            $scope.GetAgeCriteriaVaccineDetails = [];
            $scope.AMST_Id = "";
            $scope.ASVAC_Id = "";
        };

        $scope.OnChangeStudent = function () {
            $scope.ASVAC_Id = "";
            $scope.GetAgeCriteriaVaccineDetails = [];
        };

        $scope.OnChangeAgeCriteria = function () {
            $scope.GetAgeCriteriaVaccineDetails = [];
        };

        $scope.SearchVaccineStudentDetails = function () {
            $scope.GetAgeCriteriaVaccineDetails = [];
            $scope.submitted = false;
            if ($scope.myform.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASVAC_Id": $scope.ASVAC_Id,
                    "AMST_Id": $scope.AMST_Id.amsT_Id
                };

                apiService.create("VaccineAgeCriteria/SearchVaccineStudentDetails", data).then(function (promise) {

                    if (promise !== null) {
                        $scope.GetAgeCriteriaVaccineDetails = promise.getAgeCriteriaVaccineDetails;
                        $scope.GetSavedStudentVaccineDetails = promise.getSavedStudentVaccineDetails;

                        angular.forEach($scope.GetAgeCriteriaVaccineDetails, function (mv) {
                            mv.ASWVD_Id = 0;
                            mv.Selected = false;
                            angular.forEach($scope.GetSavedStudentVaccineDetails, function (sv) {
                                if (mv.asvacD_Id == sv.asvacD_Id) {
                                    mv.Selected = true;
                                    mv.ASWVD_NextDoseDate = new Date(sv.aswvD_NextDoseDate);
                                    mv.ASWVD_AdministeredBy = sv.aswvD_AdministeredBy;
                                    if (sv.aswvD_DateGiven !== null) {
                                        mv.ASWVD_DateGiven = new Date(sv.aswvD_DateGiven);
                                    }
                                    mv.ASWVD_Id = sv.aswvD_Id;
                                }
                            });
                        });
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.GetAgeCriteriaVaccineDetails.some(function (options) {
                return options.Selected;
            });
        };

        $scope.SaveStudentVaccineDetails = function () {
            $scope.submitted2 = false;
            $scope.tempdetails = [];

            if ($scope.myform1.$valid) {

                angular.forEach($scope.GetAgeCriteriaVaccineDetails, function (dd) {
                    if (dd.Selected) {
                        $scope.givendate = null;

                        if (dd.ASWVD_DateGiven !== undefined && dd.ASWVD_DateGiven !== null) {
                            $scope.givendate = new Date(dd.ASWVD_DateGiven).toDateString();
                        }
                            
                        $scope.tempdetails.push({
                            ASWVD_Id: dd.ASWVD_Id, ASWVD_NextDoseDate: new Date(dd.ASWVD_NextDoseDate).toDateString(),
                            ASWVD_DateGiven: $scope.givendate, ASWVD_AdministeredBy: dd.ASWVD_AdministeredBy, ASVACD_Id: dd.asvacD_Id
                        });                        
                    }
                });

                var data = {
                    "ASVAC_Id": $scope.ASVAC_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMST_Id": $scope.AMST_Id.amsT_Id,
                    "SaveStudentVaccineDetails_Temp": $scope.tempdetails
                };

                apiService.create("VaccineAgeCriteria/SaveStudentVaccineDetails", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnValue === true) {
                            swal("Record Saved / Updated Successfully");
                            $scope.cleardata();
                        } else {
                            swal("Failed To Save / Update Record");
                            $scope.cleardata();
                        }
                    }
                });
            } else {
                $scope.submitted2 = true;
            }
        };

        $scope.OnClickViewStudentVaccineDetails = function (obj) {

            $scope.studentname = "";
            $scope.studentname = obj.studentName + " / " + obj.amsT_AdmNo;
            $scope.agecriteria = obj.vaccineagecriteria;
            var data = {
                "ASVAC_Id": obj.asvaC_Id,
                "AMST_Id": obj.amsT_Id
            };
            apiService.create("VaccineAgeCriteria/OnClickViewStudentVaccineDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.GetViewstudentvaccinedetails = promise.getViewstudentvaccinedetails;
                    if ($scope.GetViewstudentvaccinedetails !== null && $scope.GetViewstudentvaccinedetails.length > 0) {
                        $('#popup11').modal('show');
                    }
                }
            });
        };

        $scope.ActiveDeactiveVaccineDetails = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.asvacD_ActiveFlag === true) {
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
                        apiService.create("VaccineAgeCriteria/ActiveDeactiveVaccineDetails/", user).then(function (promise) {
                            if (promise.returnValue == true) {
                                swal("Record" + " " + confirmmgs + " " + "Successfully");
                            }
                            else {
                                swal("Record" + " " + confirmmgs + " " + " Successfully");
                            }

                            $scope.GetViewVaccineDetails = promise.getViewVaccineDetails;
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.cleardata = function () {
            $scope.submitted = false;
            $scope.submitted2 = false;
            $scope.AMST_Id = "";
            $scope.GetStudentSearchList = [];
            $scope.GetAgeCriteriaVaccineDetails = [];

            $scope.ASVAC_Id = "";
            $scope.ASMAY_Id = "";
            $scope.OnLoadVaccineStudentDetails();
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        $scope.OnClickPrint = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        }
    }
})();