
(function () {
    'use strict';
    angular.module('app').controller('CumulativeReportController', CumulativeReportController)
    CumulativeReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', 'blockUI']
    function CumulativeReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, blockUI) {


        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.report = false;
        $scope.submitted = false;

        var count = 0;
        $scope.BindData = function () {
            var pageid = 2
            apiService.getURI("QRCode_Generation/Getdetails", pageid).then(function (promise) {
                $scope.yearlt = promise.yearList;
                $scope.clslist = promise.classList;
                $scope.seclist = promise.sectionList;
               // $scope.amstlt = promise.studentList;

            });
        };

        $scope.OnAcdyear = function (ASMAY_Id) {
            $scope.print = true;
            $scope.report = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };
            apiService.create("QRCode_Generation/get_classes", data).then(function (promise) {
                $scope.clslist = promise.classList;
                $scope.asmcL_Id = "";
                $scope.asmS_Id = "";

                $scope.seclist = [];


                if (promise.classList == null || promise.classList == "") {
                    swal("Classes are Not Mapped To Selected Academic Year!!!");
                }

            })
        };

        $scope.onchangeclass = function () {
            $scope.studentlistdetails = [];
            $scope.seclist = [];
            $scope.asmS_Id = "";


            $scope.report = false;
            if ($scope.asmaY_Id != "" && $scope.asmaY_Id != undefined && $scope.asmaY_Id != null) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id
                };

                apiService.create("QRCode_Generation/get_cls_sections", data).then(function (promise) {
                    $scope.seclist = promise.sectionList;

                    //
                    if (promise.sectionList == null || promise.sectionList == "") {
                        swal("Sections are Not Mapped To Selected Class!!!");
                    }
                });
            }
            else {
                swal("Please Select Academic Year  First !!!");
                $scope.asmcL_Id = "";
            }
        };
      
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all2;
            angular.forEach($scope.studentlistdetails, function (itm) {
                itm.selected = toggleStatus;
            });

        }
       
        $scope.onchangesection = function (ASMAY_Id, ASMCL_Id, ASMS_Id) {
            
            if ($scope.myForm.$valid) {
              
                    $scope.studentlistdetails = [];

                    var data = {
                        "ASMS_Id": $scope.asmS_Id,
                        "ASMCL_Id": $scope.asmcL_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "Flag": "StudentDetails",
                    };

                    apiService.create("QRCode_Generation/GetStudents", data).then(function (promise) {
                        $scope.StudentQRlist = promise.studentQRlist;

                        if (promise.studentQRlist == null || promise.studentQRlist == "") {
                            swal("Already QRcode generated for the Class and Section..");
                        }
                    });
               
                
            }
            else {
                $scope.submitted = true;
                
            }
        };
       
        $scope.QRReportDetails = function (ASMAY_Id, ASMCL_Id, ASMS_Id) {

            if ($scope.myForm.$valid) {
                var data = {
                    "ASMS_Id": $scope.asmS_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "Flag": "Qrdetails",
                };

                apiService.create("QRCode_Generation/GetStudents", data).then(function (promise) {

                    $scope.QRcodelistdetails = promise.studentQRlist;
                    $('#myModal').modal('show');
                    if (promise.studentQRlist == null || promise.studentQRlist == "") {
                        swal("Not Generated QR Code..");
                        $('#myModal').modal('hide');
                        $state.BindData();
                    }

                });
            }
            else {
                $scope.submitted = true;
            }

           

        };

        $scope.SaveQR_Code = function (sub) {
            
            $scope.qrlistarray = [];
            
            angular.forEach($scope.studentlistdetails, function (dd) {
                var imagge = document.getElementById("qrimage" + dd.AMST_Id + "").src
                if (imagge != null && imagge != "" ) {
                    $scope.qrlistarray.push({
                        Amst_Id: dd.AMST_Id,
                        IQRGD_QRCode: imagge
                    })
    
                }
                
            });

            var data = {

                "qrlistarray": $scope.qrlistarray

            };
            apiService.create("QRCode_Generation/SaveQR_Code", data).then(function (promise) {
                if (promise.message == 'saved') {
                    swal("Record  Saved Successfully", "", "success");
                    $state.reload();
                }
                if (promise.message == "recordexist") {
                    swal("Record Already Exist", "", "warning");
                    $scope.reload();
                }
            })
        };

        $scope.onselectcategory = function () {
            $scope.report = false;
            $scope.print = true;
        };



        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };
        $scope.submitted = false;
        $scope.show = "";
        $scope.GenratePdf = function () {
            $scope.submitted = true;
            count = 0;
            if ($scope.studentlistdetails != null && $scope.studentlistdetails.length > 0) {
                angular.forEach($scope.studentlistdetails, function (st) {
                    var value = 0;
                    $scope.show = 1;
                    if (st.AMST_Id != null && st.AMST_Id != "") {
                        var qr;
                        value = st.AMST_Id;
                        var valuex = value; // Example selector for an input element with id "qr-text"
                        $(".qr-code" + st.AMST_Id + "").attr("src", "https://chart.googleapis.com/chart?cht=qr&chl=" + valuex + "&chs=160x160&chld=L|0");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        }


        $scope.QRPrint = function () {
          

            var innerContents = document.getElementById("myModal1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +

                //'<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/idcardprint.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


        $scope.StudentQRCodeGenerate = function () {
            var qrlistarray12 = [];
            angular.forEach($scope.StudentQRlist, function (itm) {
                
                    qrlistarray12.push({
                        AMST_Id: itm.AMST_Id,
                    });
                
            });
            var data = {
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                qrlistarray12: qrlistarray12,
                "Flag": "StudentDetailsQR",

            };
            apiService.create("QRCode_Generation/StudentQRCode", data).then(function (promise) {
                $scope.StudentQRlist = promise.studentQRlist;
                //if (promise.staffList == null && promise.staffList == "") {
                //    swal("Already QRcode generated Emplyees");
                //}

            });
        };





    }
})();