(function () {
    'use strict';
    angular.module('app').controller('HM_MasterIllnessController', HM_MasterIllnessController)
    HM_MasterIllnessController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$stateParams', '$filter', 'Excel','$timeout']
    function HM_MasterIllnessController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $stateParams, $filter, Excel, $timeout) {

        var paginationformasters = 0;
        var copty = "";
        $scope.search = "";

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

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var data = 2;
            apiService.getURI("HM_Master/Load_illness", data).then(function (promise) {
                if (promise.getIllnessLoadDataList !== null || promise.getIllnessLoadDataList > 0) {
                    $scope.GetIllnessLoadDataList = promise.getIllnessLoadDataList;
                }
            });
        };

        $scope.savedata = function () {
            $scope.submitted = false;
            if ($scope.myForm.$valid) {
                var data = {
                    "HMMILL_Id": $scope.HMMILL_Id,
                    "HMMILL_IllnessName": $scope.hmmilL_IllnessName,
                    "HMMILL_IllnessDesc": $scope.hmmilL_IllnessDesc

                };
                apiService.create("HM_Master/Save_illness", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message == "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }                           
                        }
                        else if (promise.message == "Update") {
                            if (promise.returnval === true) {
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
                "HMMILL_Id": obj.hmmilL_Id
            };
            apiService.create("HM_Master/Edit_illness", data).then(function (promise) {
                if (promise.editIllnessDataList !== null && promise.editIllnessDataList.length > 0) {
                    $scope.HMMILL_Id = promise.editIllnessDataList[0].hmmilL_Id;
                    $scope.hmmilL_IllnessName = promise.editIllnessDataList[0].hmmilL_IllnessName;
                    $scope.hmmilL_IllnessDesc = promise.editIllnessDataList[0].hmmilL_IllnessDesc;
                }
            });
        };

        $scope.ActiveDeactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";

            if (user.hmmilL_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }

            var data = {
                "HMMILL_Id": user.hmmilL_Id                
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
                        apiService.create("HM_Master/ActiveDeactive_illness", data).then(function (promise) {
                            if (promise.returnval === true) {
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
            $scope.hmmilL_IllnessDesc = "";
            $scope.hmmilL_IllnessName = "";
            $scope.search = "";
            $scope.HMMILL_Id = 0;
            $scope.GetIllnessLoadDataList = [];
            $scope.loaddata();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
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