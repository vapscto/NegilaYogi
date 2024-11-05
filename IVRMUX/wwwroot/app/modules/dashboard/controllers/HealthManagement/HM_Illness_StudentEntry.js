(function () {
    'use strict';
    angular.module('app').controller('HM_Illness_StudentEntryController', HM_Illness_StudentEntryController)
    HM_Illness_StudentEntryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$stateParams', '$filter', 'Excel', '$timeout']
    function HM_Illness_StudentEntryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $stateParams, $filter, Excel, $timeout) {

        var paginationformasters = 0;
        var copty = "";
        $scope.obj = {};
        $scope.obj.search = "";
        $scope.obj.smschecked = false;
        $scope.obj.emailchecked = false;
        $scope.obj.whatsappchecked = false;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
        }

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.maxdate = new Date();
        $scope.HMTILL_Date = new Date();
        $scope.editflag = false;

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var data = 2;
            apiService.getURI("HM_Illness_StudentEntry/LoadStudentIllnessData", data).then(function (promise) {

                $scope.GetMasterAcademicYearList = promise.getMasterAcademicYearList;
                $scope.ASMAY_Id = promise.asmaY_Id;
                $scope.GetMasterIllnessList = promise.getMasterIllnessList;

                if (promise.getTransactionIllnessList !== null && promise.getTransactionIllnessList.length > 0) {
                    $scope.GetTransactionIllnessList = promise.getTransactionIllnessList;
                }
            });
        };

        $scope.OnChangeYear = function () {

            $scope.AMST_Id = "";
            $scope.HMMILL_Id = "";
            $scope.GetMasterStudentList = [];          
        };

        $scope.searchfilter = function (objj) {
            var data = "";
            $scope.HMMILL_Id = "";
            
            if (objj.search.length >= '3') {
                $scope.studentlst = "";
                data = {
                    "Searchfilter": objj.search,
                    "ASMAY_Id": $scope.ASMAY_Id
                };

                apiService.create("HM_Illness_StudentEntry/GetStudentDetailsBySearch", data).then(function (promise) {
                    if (promise.getMasterStudentList !== null || promise.getMasterStudentList.length > 0) {
                        $scope.GetMasterStudentList = promise.getMasterStudentList;
                    } else {
                        $scope.AMST_Id = "";
                        swal("No students are found for your search");
                    }
                });
            }
        };

        $scope.onstudentnamechange = function (objj) {
            var data = "";
            $scope.HMMILL_Id = "";
            data = {
                "AMST_Id": $scope.AMST_Id.amsT_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("HM_Illness_StudentEntry/OnStudentNameChange", data).then(function (promise) {
                if (promise.getStudentYearData !== null || promise.getStudentYearData.length > 0) {
                    $scope.GetStudentYearData = promise.getStudentYearData;

                    $scope.yearname = $scope.GetStudentYearData[0].yearName;
                    $scope.SectionName = $scope.GetStudentYearData[0].sectionName;
                    $scope.ClassName = $scope.GetStudentYearData[0].className;
                    $scope.AdmissionNo = $scope.GetStudentYearData[0].admissionNo;
                    $scope.StudentName = $scope.GetStudentYearData[0].studentName;
                    $scope.ASMCL_Id = $scope.GetStudentYearData[0].asmcL_Id;
                    $scope.ASMS_Id = $scope.GetStudentYearData[0].asmS_Id;
                } else {
                    swal("No students are found for your search");
                }
            });

        };

        $scope.savedata = function () {
            $scope.submitted = false;
            if ($scope.myForm.$valid) {
                var data = {
                    "HMTILL_Id": $scope.HMTILL_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "AMST_Id": $scope.AMST_Id.amsT_Id,
                    "HMMILL_Id": $scope.HMMILL_Id.hmmilL_Id,
                    "HMTILL_Date": new Date($scope.HMTILL_Date).toDateString(),
                    "smschecked": $scope.obj.smschecked,
                    "emailchecked": $scope.obj.emailchecked,
                    "whatsappchecked": $scope.obj.whatsappchecked,

                };
                apiService.create("HM_Illness_StudentEntry/SaveStudentIllnessData", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "Add") {
                            if (promise.returnValue === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        }
                        else if (promise.message === "Update") {
                            if (promise.returnValue === true) {
                                swal("Record Update Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        }
                        else if (promise.message === "Duplicate") {
                            swal("Record Already Exists");
                        } else {
                            swal("Failed To Save/Update");
                        }

                        $scope.cleardata();
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.editdata = function (obj) {
            var data = {
                "HMTILL_Id": obj.hmtilL_Id
            };
            apiService.create("HM_Illness_StudentEntry/EditStudentIllnessData", data).then(function (promise) {
                if (promise.getEditStudentIllnessData !== null && promise.getEditStudentIllnessData.length > 0) {
                    $scope.editflag = true;
                    $scope.HMMILL_Id = promise.getEditIllnessData[0];
                    $scope.AMST_Id = promise.getEditStudentData[0];

                    $scope.HMTILL_Date = new Date(promise.getEditStudentIllnessData[0].hmtilL_Date);
                    $scope.HMTILL_Id = promise.getEditStudentIllnessData[0].hmtilL_Id;

                    $scope.ASMAY_Id = promise.getEditStudentIllnessData[0].asmaY_Id;
                    $scope.ASMCL_Id = promise.getEditStudentIllnessData[0].asmcL_Id;
                    $scope.ASMS_Id = promise.getEditStudentIllnessData[0].asmS_Id;

                    $scope.yearname = $scope.getEditStudentData[0].yearName;
                    $scope.SectionName = $scope.getEditStudentData[0].sectionName;
                    $scope.ClassName = $scope.getEditStudentData[0].className;
                    $scope.AdmissionNo = $scope.getEditStudentData[0].admissionNo;
                    $scope.StudentName = $scope.getEditStudentData[0].studentName_Edit;
                }
            });
        };

        $scope.ActiveDeactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";

            if (user.hmtilL_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }

            var data = {
                "HMTILL_Id": user.hmtilL_Id
            };

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
                        apiService.create("HM_Illness_StudentEntry/ActiveDeactiveStudentIllnessData", data).then(function (promise) {
                            if (promise.returnValue === true) {
                                swal("Record " + confirmmgs + " " + "successfully");
                            }
                            else {
                                swal("Record " + mgs + " Failed");
                            }
                            $scope.loaddata();
                        });
                    }
                });
        };

        $scope.cleardata = function () {
            $scope.ASMAY_Id = "";
            $scope.AMST_Id = "";
            $scope.HMMILL_Id = "";
            $scope.obj.search = "";
            $scope.GetMasterStudentList = [];
            $scope.GetMasterIllnessList = [];
            $scope.HMTILL_Date = new Date();
            $scope.maxdate = new Date();
            $scope.HMTILL_Id = 0;
            $scope.GetMasterAcademicYearList = [];
            $scope.GetMasterIllnessList = [];
            $scope.GetTransactionIllnessList = [];
            $scope.editflag = false;
            $scope.obj = {};
            $scope.obj.search = "";
            $scope.obj.smschecked = false;
            $scope.obj.emailchecked = false;
            $scope.obj.whatsappchecked = false;
            $scope.loaddata();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.filterValue = function (obj) {
            return ($filter('date')(obj.hmtilL_Date, 'dd/MM/yyyy').indexOf($scope.obj.search) >= 0) ||                 
                (angular.lowercase(obj.studentName)).indexOf(angular.lowercase($scope.obj.search)) >= 0 ||
                (angular.lowercase(obj.admissionNo)).indexOf(angular.lowercase($scope.obj.search)) >= 0 ||
                (angular.lowercase(obj.yearName)).indexOf(angular.lowercase($scope.obj.search)) >= 0 ||
                (angular.lowercase(obj.className)).indexOf(angular.lowercase($scope.obj.search)) >= 0 ||
                (angular.lowercase(obj.sectionName)).indexOf(angular.lowercase($scope.obj.search)) >= 0 ||
                (angular.lowercase(obj.hmmilL_IllnessName)).indexOf(angular.lowercase($scope.obj.search)) >= 0;
        };

        $scope.ExportToExcel = function () {
            var exportHref = Excel.tableToExcel('#tableId', 'sheet name');
            var excelname = "Master Illness Report.xlsx";
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.Print = function () {
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
        };
    }
})();