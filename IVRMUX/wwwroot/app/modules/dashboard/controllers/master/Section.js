
(function () {
    'use strict';
    angular
.module('app')
.controller('sectionController', sectionController)

    sectionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function sectionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache,Excel, $timeout) {

        $scope.sortKey = 'asmS_Id';
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

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));


        $scope.MasterSectionCl = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("MasterSection/getalldetails", pageid).then(function (promise) {

                $scope.masterse = promise.masterSectionData;
                $scope.presentCountgrid = $scope.masterse.length;
                $scope.sectionlist = promise.getsectionlist;
            });
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.DeletRecord = function (data, SweetAlertt) {
            
            var mgs = "";
            if (data.asmC_ActiveFlag == 0) {

                mgs = "Active";

            }
            else {
                mgs = "De-Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to to " + mgs + " Section?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.getURI("MasterSection/Deletedetails", data.asmS_Id).
                    then(function (promise) {
                        if (promise.message != null) {
                            swal(promise.message);
                        }
                        else {
                            if (promise.returnval === "true") {
                                swal('Section De-Activate Successfully');
                                $state.reload();
                            }
                            else {
                                swal('Section Activate Successfully');
                                $state.reload();
                            }
                        }
                        $state.reload();
                    })
                }
                else {
                    swal("Cancelled");
                }
            });



            //$scope.editEmployee = employee.asmC_Id;
            //var orgaid = $scope.editEmployee

            // swal({

            //     title: "Are you sure?",
            //     text: "Do you want to delete record !!!!!!!!",
            //     type: "warning",
            //     showCancelButton: true,
            //     confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
            //     cancelButtonText: "Cancel!!!!!!",
            //     closeOnConfirm: false,
            //     closeOnCancel: false
            // },
            //function (isConfirm) {
            //    if (isConfirm) {
            //        apiService.DeleteURI("MasterSection/Deletedetails", orgaid).
            //        then(function (promise) {
            //            swal(promise.returnval);
            //            $state.reload();
            //        })
            //    }
            //    else {
            //        swal("Record Deletion Cancelled", "Failed");
            //    }
            //});


            //})
        }

        $scope.cance = function () {
            $state.reload();

        }


        $scope.EditMasterSectvalue = function (employee) {
            $scope.editEmployee = employee.asmS_Id;
            var orgid = $scope.editEmployee;
            //orgid = 12;
            apiService.getURI("MasterSection/Editdetails", orgid).
            then(function (promise) {

                $scope.ASMC_Id = promise.masterSectionData[0].asmS_Id;
                // $scope.MI_Id = promise.masterSectionData[0].mI_Id;
                $scope.ASMC_SectionName = promise.masterSectionData[0].asmC_SectionName;
                $scope.ASMC_SectionCode = promise.masterSectionData[0].asmC_SectionCode;
                $scope.ASMC_Order = promise.masterSectionData[0].asmC_Order;
                $scope.ASMC_ActiveFlag = promise.masterSectionData[0].asmC_ActiveFlag;
                $scope.ASMC_MaxCapacity = promise.masterSectionData[0].asmC_MaxCapacity;

            })
        }

        $scope.searchsource = function () {
            var entereddata = $scope.search;

            var data = {
                "ASMC_SectionName": $scope.search,
                "ASMC_SectionCode": $scope.type
            }

            apiService.create("MasterSection/1", data).
        then(function (promise) {
            $scope.masterse = promise.masterSectionData;
            $scope.presentCountgrid = $scope.masterse.length;
            swal("searched Successfully");
        })
        }

        $scope.submitted = false;
        $scope.saveMasterdata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "ASMC_SectionName": $scope.ASMC_SectionName,
                    "ASMC_SectionCode": $scope.ASMC_SectionCode,
                    "ASMC_Order": $scope.ASMC_Order,
                    "ASMC_MaxCapacity": $scope.ASMC_MaxCapacity,
                    "ASMC_Id": $scope.ASMC_Id
                }

                apiService.create("MasterSection/", data).
                then(function (promise) {
                    swal(promise.returnval);
                    $state.reload();
                    $scope.submitted = false;
                })

            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }

        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        }


        //section order
        $scope.getclassorder = function () {
            
            var pageid = 2;
            apiService.getURI("Classsectionorder/getdetails", pageid).
            then(function (promosie) {
                if (promosie != null) {
                    //$scope.newuser2 = promosie.classdetails
                    $scope.newuser3 = promosie.sectiondetails;
                }
                else {
                    swal("No Records Found");
                }
            })
        }

        $scope.resetLists = function () {
            $scope.configB = {
                // Changed sorting within list
                onUpdate: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement
                    // + indexes from onEnd
                }
            };
        }

        $scope.init = function () {
            $scope.resetLists();
        };
        $scope.init();


        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.table1) {
                    $scope.newuser3[index].order = Number(index) + 1;

                }
            }
        };


        $scope.save = function (newuser3) {
            var data = {
                //"classorder": $scope.newuser2,
                "secorder": $scope.newuser3,
                "flagsec": 'section'
            }
            apiService.create("Classsectionorder/save/", data).then(
                function (promoise) {
                    if (promoise != null) {
                        if (promoise.retruval == true) {
                            swal("Records Updated Sucessfully");
                            $state.reload();
                            $('#myModalreadmit').modal('hide');
                        }
                        else {
                            swal("Failed to Update the Record");
                            $state.reload();
                            $('#myModalreadmit').modal('hide');
                        }
                    }
                    else {
                        swal("No Records Updated");
                        $state.reload();
                        $('#myModalreadmit').modal('hide');
                    }
                    $scope.BindData();
                })
        }

        $scope.searchValue = '';
        //$scope.filterValue = function (obj) {
        //    
        //    return (angular.lowercase(obj.asmC_SectionName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (angular.lowercase(obj.asmC_MaxCapacity)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (angular.lowercase(obj.asmC_SectionCode)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        //}


        $scope.filterValue = function (obj) {

            if (obj.asmC_ActiveFlag == 1) {
                $scope.active = "De-Activate";
            }
            else {
                $scope.active = "Activate";
            }
            return (angular.lowercase(obj.asmC_SectionName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (JSON.stringify(obj.asmC_MaxCapacity)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.asmC_SectionCode)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase($scope.active)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (JSON.stringify(obj.asmC_Order)).indexOf($scope.searchValue) >= 0;
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
            var excelname = 'Master Section List Report.xls';
            var exportHref = Excel.tableToExcel(tableId, 'Master Section List Report');
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