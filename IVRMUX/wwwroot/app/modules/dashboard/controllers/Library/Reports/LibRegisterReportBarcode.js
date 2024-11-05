

(function () {
    'use strict';
    angular
        .module('app')
        .controller('BookRegisterReportController', BookRegisterReportController)

    BookRegisterReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', 'Excel', '$timeout']
    function BookRegisterReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, Excel, $timeout) {

        $scope.submitted = false;
        $scope.tablediv = false;;
        $scope.printd = false;
        $scope.searchValue = '';

        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        $scope.searchValue = "";
        $scope.searchValuetw = "";
        $scope.obj = {};
        $scope.obj.allCheckDD = false;
       // $scope.allCheck = false;
        //-------------Load-data...
        var count = 0;
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 15;
            $scope.search = "";

            var pageid = 2;
            apiService.getURI("BookRegisterReport/getdetails", pageid).then(function (promise) {
              //  $scope.booktype = promise.booktype;
                $scope.deptlist = promise.deptlist;
                $scope.clsslist = promise.clsslist;
                $scope.Master_book = promise.master_book;
            })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }      
        $scope.AllChecked = function () {
            angular.forEach($scope.reportlist, function (st) {
                if ($scope.obj.allCheckDD == true) {
                    st.selected = true;
                }
                else {
                    st.selected = false;
                }

            });
           
        }
        $scope.ReportlistArray = function () {


            $scope.getReportwo();
        }
        $scope.BookType = function () {
            $scope.alldata = [];
            $scope.reportlist = [];
            var data = {
                "Type": "BOOK",
                "LMAL_Id": $scope.LMAL_Id,
                "LMB_ISBNNo": $scope.Typea
            }
            apiService.create("BookRegisterReport/BarCode", data).then(function (promise) {
                if (promise.alldata.length > 0) {
                    $scope.alldata = promise.alldata;
                    angular.forEach($scope.deptlist, function (st) {
                        st.stck == false;
                    });
                }
            })
        }
        $scope.all_checkST = function (stri) {
            $scope.stall = stri;
            var toggleStatus = $scope.stall;
            angular.forEach($scope.alldata, function (st) {
                st.stck = toggleStatus;
            });
        };
        //all_checkST
        $scope.all_checkSTY = function (stri) {
            $scope.stalll = stri;
            var toggleStatus = $scope.stalll;
            angular.forEach($scope.deptlist, function (st) {
                st.stck = toggleStatus;
            });
            $scope.departments();
        };
        $scope.isOptionsRequiredY = function () {
            return !$scope.deptlist.some(function (options) {
                return options.stck;
            });
        };
        $scope.isOptionsRequired = function () {
            return !$scope.alldata.some(function (options) {
                return options.stck;
            });
        };
        $scope.togchkbxST = function () {
            $scope.stall = $scope.alldata.every(function (str) {
                return str.stck;
            });

        };
        //togchkbxSTY
        $scope.togchkbxSTY = function () {
            $scope.stalll = $scope.deptlist.every(function (str) {
                return str.stck;
            });
            $scope.departments();
        };
        $scope.departments = function () {
            $scope.alldata = [];
            $scope.deptlisttemp = [];
            $scope.reportlist = [];
            if ($scope.deptlist != null && $scope.deptlist.length > 0) {
                angular.forEach($scope.deptlist, function (st) {
                    if (st.stck == true) {
                        $scope.deptlisttemp.push({
                            LMD_Id: st.lmD_Id
                        })
                    }

                });
            }
            if ($scope.LMAL_Id > 0) {
                var data = {
                    "Type": "Department",
                    "LMAL_Id": $scope.LMAL_Id,
                    "LMB_ISBNNo": $scope.Typea,
                    "DepartMent_list": $scope.deptlisttemp
                }
                apiService.create("BookRegisterReport/BarCode", data).then(function (promise) {
                    if (promise.alldata.length > 0) {
                        $scope.alldata = promise.alldata;
                    }
                    else {
                        $scope.LMAL_Id = "";
                    }
                })
            }
            else {
                swal("Select Master Libarary !")
            }

        }
        $scope.getReportwo = function () {
            $scope.reportlisttwo = [];
            if ($scope.reportlist != null && $scope.reportlist.length > 0) {
                angular.forEach($scope.reportlist, function (st) {
                    if (st.selected == true) {
                        $scope.reportlisttwo.push(st);
                    }

                });
            }
        }
        $scope.get_report = function () {
            count = 0;
            if ($scope.myForm.$valid) {
                $scope.deptlisttemp = [];
                $scope.Booktemp = [];
                $scope.reportlist = [];
                if ($scope.deptlist != null && $scope.deptlist.length > 0) {
                    angular.forEach($scope.deptlist, function (st) {
                        if (st.stck == true) {
                            $scope.deptlisttemp.push({
                                LMD_Id: st.lmD_Id
                            })
                        }

                    });
                }
                if ($scope.alldata != null && $scope.alldata.length > 0) {
                    angular.forEach($scope.alldata, function (st) {
                        if (st.stck == true) {
                            $scope.Booktemp.push({
                                LMB_Id: st.lmB_Id
                            })
                        }

                    });
                }
                var data = {
                    "Type": "Report",
                    "DepartMent_list": $scope.deptlisttemp,
                    "Book_List": $scope.Booktemp,
                    "LMB_ISBNNo": $scope.Typea,
                    "LMAL_Id": $scope.LMAL_Id
                }



                apiService.create("BookRegisterReport/BarCode", data).then(function (promise) {
                    if (promise.alldata != null && promise.alldata.length > 0) {

                        $scope.reportlist = promise.alldata;
                        angular.forEach($scope.reportlist, function (st) {
                            st.selected = true;
                        });
                        $scope.obj.allCheckDD = true;
                        $scope.getReportwo();
                    }
                    else {
                        swal('Record Not Available!!!');
                        $state.reload();
                    }
                })

            }
            else {
                $scope.submitted = true;
            }

        }
        //------------End-Get-Report...


        //===========print===========//
        $scope.printData = function () {
            if (count > 0) {
                var innerContents = document.getElementById("printtable").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/BookRegisterReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else {
                count = count + 1;
                angular.forEach($scope.reportlist, function (dd) {
                    if (dd.lmbanO_AccessionNo != null && dd.lmbanO_AccessionNo != "") {
                        var canvas = document.getElementById("barcode" + dd.lmbanO_AccessionNo);
                        dd.ImagePath = canvas.toDataURL();
                    }
                   
                });
            }
        }
        //==============End==============//


        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //-----------clear-field
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.ExportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }
        $scope.GenratePdf = function () {
            count = 0;
            if ($scope.reportlist != null && $scope.reportlist.length > 0) {
                angular.forEach($scope.reportlist, function (st) {
                    var value = 0;
                    if (st.lmbanO_AccessionNo != null && st.lmbanO_AccessionNo != "") {
                        value = st.lmbanO_AccessionNo;
                        JsBarcode("#barcode" + st.lmbanO_AccessionNo + "", value);
                    }
                });
                $scope.getReportwo();

            }

        }
    }
})();

