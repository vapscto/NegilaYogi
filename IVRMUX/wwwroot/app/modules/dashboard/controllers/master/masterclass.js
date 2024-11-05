(function () {
    'use strict';
    angular
        .module('app')
        .controller('masterclassController', masterclassController)

    masterclassController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function masterclassController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {
        $scope.searc_button = true;
        $scope.sortKey = 'asmcL_Id';
        $scope.sortReverse = true;
        $scope.searchValue = "";
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));



        //class year validation
        $scope.validateMinYear = function (ASMCL_MinAgeYear) {
            if (ASMCL_MinAgeYear != "" && $scope.ASMCL_MaxAgeYear != "") {
                var maxage = parseInt($scope.ASMCL_MaxAgeYear);
                var minage = parseInt(ASMCL_MinAgeYear);
                if (minage > maxage) {
                    swal("Please Select Minimun Year less than or equal to Maximum year");
                    $scope.ASMCL_MinAgeYear = "";
                    return;
                }

            }
            $scope.ASMCL_MinAgeMonth = "";
            $scope.ASMCL_MinAgeDays = "";

        }
        $scope.validateMaxYear = function (ASMCL_MaxAgeYear) {
            if (ASMCL_MaxAgeYear != "" && $scope.ASMCL_MinAgeYear != "") {
                var minage = parseInt($scope.ASMCL_MinAgeYear);
                var maxage = parseInt(ASMCL_MaxAgeYear);
                if (maxage < minage) {
                    swal("Please Select Maximun Year greater than or equal to Minimum year");
                    $scope.ASMCL_MaxAgeYear = "";
                    return;
                }
            }
            $scope.ASMCL_MaxAgeMonth = "";
            $scope.ASMCL_MaxAgeDays = "";

        }
        $scope.validateMaxMon = function (ASMCL_MaxAgeMonth) {
            if ($scope.ASMCL_MaxAgeYear == $scope.ASMCL_MinAgeYear) {
                var minmon = parseInt($scope.ASMCL_MinAgeMonth);
                var maxmon = parseInt(ASMCL_MaxAgeMonth);
                if (maxmon < minmon) {
                    swal("Please Select Maximun Month greater than or equal to Minimum month");
                    $scope.ASMCL_MaxAgeMonth = "";
                    return;
                }
            }
            $scope.ASMCL_MaxAgeDays = "";

        }
        $scope.validateMinMon = function (ASMCL_MinAgeMonth) {
            if ($scope.ASMCL_MaxAgeYear == $scope.ASMCL_MinAgeYear) {
                var maxmon = parseInt($scope.ASMCL_MaxAgeMonth);
                var minmon = parseInt(ASMCL_MinAgeMonth);
                if (minmon > maxmon) {
                    swal("Please Select Minimun Month Less than or equal to Maximun month");
                    $scope.ASMCL_MinAgeMonth = "";
                    return;
                }
            }
            $scope.ASMCL_MinAgeDays = "";

        }
        $scope.validateMaxDays = function (ASMCL_MaxAgeDays) {
            if (($scope.ASMCL_MaxAgeYear == $scope.ASMCL_MinAgeYear) && ($scope.ASMCL_MinAgeMonth == $scope.ASMCL_MaxAgeMonth)) {
                var mindays = parseInt($scope.ASMCL_MinAgeDays);
                var maxdays = parseInt(ASMCL_MaxAgeDays);
                if (maxdays <= mindays) {
                    swal("Please Select Maximum days greater than  Minimun days");
                    $scope.ASMCL_MaxAgeDays = "";
                    return;
                }
            }

        }
        $scope.validateMinDays = function (ASMCL_MinAgeDays) {
            if (($scope.ASMCL_MaxAgeYear == $scope.ASMCL_MinAgeYear) && ($scope.ASMCL_MinAgeMonth == $scope.ASMCL_MaxAgeMonth)) {
                var maxdays = parseInt($scope.ASMCL_MaxAgeDays);
                var mindays = parseInt(ASMCL_MinAgeDays);
                if (mindays >= maxdays) {
                    swal("Please Select Minimum days less than  Maximun days");
                    $scope.ASMCL_MinAgeDays = "";
                    return;
                }
            }

        }

        $scope.School_M_ClassDropdownList = function () {
            $scope.myBtn = true;
            $scope.myBtn1 = false;
            var pageid = 2;
            apiService.getURI("School_M_Class/getalldetails", pageid).
                then(function (promise) {
                    // for pagination 
                    $scope.currentPage = 1;
                    $scope.itemsPerPage = paginationformasters;
                    $scope.school_M_ClassList = promise.school_M_ClassList;
                    $scope.presentCountgrid = $scope.school_M_ClassList.length;
                    console.log(promise.school_M_ClassList);
                    $scope.classlist = promise.getclasslist;
                    //$scope.sortKey = 'asmcL_ClassName';
                    //$scope.reverse = false;
                });
        };
        //$scope.searchValue = function (item) {

        //    return item === 'asmcL_ClassName' || item === 'asmcL_ClassCode' || item === 'asmcL_MaxCapacity' || item === 'asmcL_Order' || item === 'asmcL_ClassCode' || item === 'asmcL_MaxAgeYear' || item === 'asmcL_MinAgeYear';
        //};

        //$scope.order = function (keyname) {

        //    $scope.sortKey = keyname;   //set the sortKey to the param passed
        //    $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        //}

        //$scope.sort = function (key) {
        //    $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
        //    $scope.sortKey = key;
        //}

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.cleardata = function () {
            $state.reload();
        }
        $scope.submitted = false;
        $scope.saveSchool_M_Class = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                if (($scope.ASMCL_ClassName === undefined || $scope.ASMCL_ClassName === "") || ($scope.ASMCL_MaxCapacity === undefined || $scope.ASMCL_MaxCapacity === "")) {
                    if ($scope.ASMCL_ClassName === undefined || $scope.ASMCL_ClassName === "") {
                        swal("Please Select Class");
                        return;
                    }

                    if ($scope.ASMCL_MaxCapacity === undefined || $scope.ASMCL_MaxCapacity === "") {
                        swal("Please Select Class Capacity");
                        return;
                    }

                } else {
                    var maxage = parseInt($scope.ASMCL_MaxAgeYear);
                    var minage = parseInt($scope.ASMCL_MinAgeYear);
                    if (minage > maxage) {
                        swal("Please Select Minimun Year less than or equal to Maximum year");
                        $scope.ASMCL_MinAgeYear = "";
                        return;
                    }
                    else {

                        if ($scope.ASMCL_ActiveFlag == undefined || $scope.ASMCL_ActiveFlag == "") {

                        }
                        else {
                            $scope.ASMCL_ActiveFlag = $scope.ASMCL_ActiveFlag;
                        }
                        if ($scope.ASMCL_PreadmFlag == true) {
                            $scope.ASMCL_PreadmFlag = 1;
                        }
                        else {
                            $scope.ASMCL_PreadmFlag = 0;
                        }

                        var data = {
                            // "MI_Id": 2,
                            "ASMCL_ClassName": $scope.ASMCL_ClassName,
                            "ASMCL_MinAgeYear": $scope.ASMCL_MinAgeYear,
                            "ASMCL_MinAgeMonth": $scope.ASMCL_MinAgeMonth,
                            "ASMCL_MinAgeDays": $scope.ASMCL_MinAgeDays,
                            "ASMCL_MaxAgeYear": $scope.ASMCL_MaxAgeYear,
                            "ASMCL_MaxAgeMonth": $scope.ASMCL_MaxAgeMonth,
                            "ASMCL_MaxAgeDays": $scope.ASMCL_MaxAgeDays,
                            "ASMCL_Order": $scope.ASMCL_Order,
                            "ASMCL_ClassCode": $scope.ASMCL_ClassCode,
                            "ASMCL_ActiveFlag": $scope.ASMCL_ActiveFlag,
                            "ASMCL_MaxCapacity": $scope.ASMCL_MaxCapacity,
                            "ASMCL_Id": $scope.ASMCL_Id,
                            "ASMCL_PreadmFlag": $scope.ASMCL_PreadmFlag
                        }

                        apiService.create("School_M_Class/", data).
                            then(function (promise) {
                                $scope.school_M_ClassList = promise.school_M_ClassList;
                                $scope.presentCountgrid = $scope.school_M_ClassList.length;


                                if (promise.returnval == "add") {
                                    swal('Record Saved Successfully');
                                    $scope.cleardata();
                                }
                                else if (promise.returnval == "update") {
                                    swal('Record Updated Successfully');
                                    $scope.cleardata();
                                }
                                else {
                                    swal(promise.returnval);

                                    $scope.cleardata();
                                }
                                $scope.submitted = false;

                            })
                    }
                }
            }
        };

        $scope.edit = function (id) {

            apiService.getURI("School_M_Class/getSchool_M_ClassById", id).
                then(function (promise) {
                    angular.forEach(promise.school_M_ClassList, function (value, key) {
                        if (value.asmcL_Id !== 0) {
                            $scope.ASMCL_ClassName = value.asmcL_ClassName;
                            $scope.ASMCL_MinAgeYear = value.asmcL_MinAgeYear;
                            $scope.ASMCL_MinAgeMonth = value.asmcL_MinAgeMonth;
                            $scope.ASMCL_MinAgeDays = value.asmcL_MinAgeDays;
                            $scope.ASMCL_MaxAgeYear = value.asmcL_MaxAgeYear;
                            $scope.ASMCL_MaxAgeMonth = value.asmcL_MaxAgeMonth;
                            $scope.ASMCL_MaxAgeDays = value.asmcL_MaxAgeDays;
                            $scope.ASMCL_Order = value.asmcL_Order;
                            $scope.ASMCL_ClassCode = value.asmcL_ClassCode;
                            $scope.ASMCL_Id = value.asmcL_Id;
                            var flag = true;
                            if (value.asmcL_ActiveFlag === true) {
                                flag = true;
                            }
                            else {
                                flag = false;
                            }
                            if (value.asmcL_PreadmFlag === 1) {
                                $scope.ASMCL_PreadmFlag = true;
                            }
                            else {
                                $scope.ASMCL_PreadmFlag = false;
                            }

                            $scope.ASMCL_ActiveFlag = flag;
                            $scope.ASMCL_MaxCapacity = value.asmcL_MaxCapacity;

                        }


                    });

                })

        }


        $scope.reset = function () {
            $scope.School_M_ClassDropdownList();
            $scope.searc_button = true;
            $scope.searchValue = "";
            $scope.searchColumn = 0;
        }

        //search

        $scope.searchsource = function () {
            var entereddata = $scope.searchValue;

            var data = {
                "ASMCL_ClassName": $scope.searchValue,
                "ASMCL_ClassCode": $scope.type
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("School_M_Class/1", data).
                then(function (promise) {
                    $scope.school_M_ClassList = promise.school_M_ClassList;
                    $scope.presentCountgrid = $scope.school_M_ClassList.length;
                    swal("Searched Successfully");
                })
        }

        $scope.delete = function (data, SweetAlert) {


            var mgs = "";
            if (data.asmcL_ActiveFlag == false) {
                mgs = "Activate";
            }
            else {
                mgs = "De-Activate";
            }
            swal({
                title: "Are You Sure?",
                text: "Do You Want to " + mgs + " Class?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.getURI("School_M_Class/deletedetails", data.asmcL_Id).
                            then(function (promise) {

                                if (promise.message != null) {
                                    swal(promise.message);
                                }
                                else {
                                    if (promise.returnval === "true") {
                                        swal('Class De-Activated Successfully');
                                    }
                                    else {
                                        swal('Class Activated Successfully');
                                    }
                                }

                                $state.reload();
                            })
                    }
                    else {
                        swal("Cancelled");
                    }
                });
        }

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


        $scope.search_box = function () {
            if ($scope.searchValue != "" || $scope.searchValue != null) {
                $scope.searc_button = false;
            }
            else {
                $scope.searc_button = true;
            }
        }

        $scope.searchByColumn = function (searchValue, searchColumn) {


            if (searchValue != null || searchValue != undefined && searchColumn != null || searchColumn != undefined) {
                var data = {
                    "EnteredData": searchValue,
                    "SearchColumn": searchColumn,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("School_M_Class/SearchByColumn", data)

                    .then(function (promise) {

                        if (promise.count == 0) {
                            swal("No Records Found");
                            $scope.searc_button = true;
                            $state.reload();
                        }
                        if (promise.count > 0) {
                            $scope.school_M_ClassList = promise.school_M_ClassList;
                            $scope.presentCountgrid = $scope.school_M_ClassList.length;
                            swal("Record Searched Successfully");
                        }
                        else {
                            $scope.searchValue = "";
                            $scope.school_M_ClassList = promise.school_M_ClassList;
                            $scope.presentCountgrid = $scope.school_M_ClassList.length;
                        }
                    })
            }
            else {

            }
        }

        $scope.validateMinYear = function (ASMCL_MinAgeYear) {
            if (ASMCL_MinAgeYear != "" && $scope.ASMCL_MaxAgeYear != "") {
                var maxage = parseInt($scope.ASMCL_MaxAgeYear);
                var minage = parseInt(ASMCL_MinAgeYear);
                if (minage > maxage) {
                    swal("Please Select Minimun Year less than or equal to Maximum year");
                    $scope.ASMCL_MinAgeYear = "";
                    return;
                }
            }
            $scope.ASMCL_MinAgeMonth = "";
            $scope.ASMCL_MinAgeDays = "";
        }


        //class order
        $scope.getclassorder = function () {

            var pageid = 2;
            apiService.getURI("Classsectionorder/getdetails", pageid).
                then(function (promosie) {
                    if (promosie != null) {
                        $scope.newuser2 = promosie.classdetails
                        //$scope.newuser3 = promosie.sectiondetails;
                    }
                    else {
                        swal("No Records Found");
                    }
                })
        }

        $scope.resetLists = function () {
            $scope.configA = {
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
                    $scope.newuser2[index].order = Number(index) + 1;

                }
            }
        };

        $scope.save = function (newuser3) {
            var data = {
                "classorder": $scope.newuser2,
                "flagclass": 'class'
                // "secorder": $scope.newuser3,
            }
            apiService.create("Classsectionorder/save/", data).then(
                function (promoise) {
                    if (promoise != null) {
                        if (promoise.retruval == true) {
                            swal("Records Updated Sucessfully");
                            $state.reload();
                        }
                        else {
                            swal("Failed to Update the Record");
                            $state.reload();
                        }
                    }
                    else {
                        swal("No Records Updated");
                        $state.reload();
                    }
                    $scope.BindData();
                })
        }


        //$scope.filterValue = function (obj) {
        //    return (angular.lowercase(obj.asmcL_ClassName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (angular.lowercase(obj.asmcL_ClassCode)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (angular.lowercase(obj.asmcL_MaxCapacity)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (angular.lowercase(obj.asmcL_Order)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (angular.lowercase(obj.asmcL_MaxAgeYear)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (angular.lowercase(obj.asmcL_MaxAgeMonth)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (angular.lowercase(obj.asmcL_MaxAgeDays)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (angular.lowercase(obj.asmcL_MinAgeYear)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (angular.lowercase(obj.asmcL_MinAgeMonth)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (angular.lowercase(obj.asmcL_MinAgeDays)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
        //        (angular.lowercase(obj.asmcL_ActiveFlag)).indexOf(angular.lowercase($scope.searchValue)) >= 0

        //}
        $scope.filterValue = function (obj) {

            if (obj.asmcL_ActiveFlag == true) {
                $scope.active = "Active";
            }
            else {
                $scope.active = "InActive";
            }
            return (angular.lowercase(obj.asmcL_ClassName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.asmcL_ClassCode)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (JSON.stringify(obj.asmcL_MaxCapacity)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.asmcL_Order)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.asmcL_MaxAgeYear)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.asmcL_MaxAgeMonth)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.asmcL_MaxAgeDays)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.asmcL_MinAgeYear)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.asmcL_MinAgeMonth)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.asmcL_MinAgeDays)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase($scope.active)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
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
            var excelname = 'Master Class List Report.xls';
            var exportHref = Excel.tableToExcel(tableId, 'Master Class List Report');
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