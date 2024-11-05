
(function () {
    'use strict';
    angular
        .module('app')
        .controller('preadmission_statusController', preadmission_statusController)

    preadmission_statusController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache', 'Excel', '$timeout']
    function preadmission_statusController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache, Excel, $timeout) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.sortKey = 'iC_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.get("DocumentviewReport/StatusGetdetails/2").then(function (promise) {

                if (promise.status_array.length > 0) {
                    $scope.newuser = promise.status_array;
                    //$scope.presentCountgrid = $scope.newuser.length;
                    //$scope.castelist = promise.getcastelist;
                }
                else {
                    swal("No Records Found");
                }

                $scope.Categaries = promise.mastercastename;
            })
        };

        //delete record
        $scope.Deletemasterdata = function (DeleteRecord, SweetAlert) {

            $scope.deleteId = DeleteRecord.pamsT_Id;
            var MdeleteId = $scope.deleteId;

            // swal(id);
            swal({
                title: "Are you sure?",
                text: "Do you want to delete the Status ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel..!", 
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.getURI("DocumentviewReport/MasterDeleteModulesDTO", MdeleteId).then(function (promise) {
                            if (promise.msg != "" && promise.msg != null) {
                                swal(promise.msg);
                                return;
                            }
                            if (promise.returnVal == true) {
                                swal("Status Deleted Successfully");
                                $state.reload();
                            }
                            else {
                                swal("Failed to Delete the Status");
                            }
                        })
                    }
                    else {
                        swal("Status Deletion Cancelled");
                    }

                });
        }

        // to Edit Data
        $scope.Editmasterdata = function (EditRecord) {

            $scope.EditId = EditRecord.pamsT_Id;
            var MEditId = $scope.EditId;
            apiService.getURI("DocumentviewReport/GetSelectedRowDetails/", MEditId).then(function (promise) {

                $scope.PAMST_Status = promise.gridviewDetails[0].pamsT_Status;
                $scope.PAMST_StatusFlag = promise.gridviewDetails[0].pamsT_StatusFlag;
                $scope.PAMST_Id = promise.gridviewDetails[0].pamsT_Id;
               
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
        $scope.savemasterdata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "PAMST_Id": $scope.PAMST_Id,
                    "PAMST_Status": $scope.PAMST_Status,
                    "PAMST_StatusFlag": $scope.PAMST_StatusFlag
                  
                }
                apiService.create("DocumentviewReport/mastersaveDTO", data).then(function (promise) {

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
                        swal("Status Name Already Exists");
                    }
                    else if (promise.returnVal == false) {
                        swal("Failed to Save");
                    }
                    else if (promise.returnVal_update == false) {
                        swal("Failed to Update");
                    }
                    $state.reload();
                })
            }
        };

        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }

        //$scope.IsHidden2 = true;
        //$scope.ShowHide2 = function () {
        //    //If DIV is hidden it will be visible and vice versa.
        //    $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        //}

        $scope.cancel = function () {
            $state.reload();
        }

        $scope.searchValue = "";
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.pamsT_Status)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.pamsT_StatusFlag)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        };

        $scope.printData = function () {
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

        $scope.exportToExcel = function (tableId) {
            var excelname = 'Master Status List Report.xls';
            var exportHref = Excel.tableToExcel(tableId, 'Master Status List Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

    }

})();