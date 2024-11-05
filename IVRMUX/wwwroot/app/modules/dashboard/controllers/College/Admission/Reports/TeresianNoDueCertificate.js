(function () {
    'use strict';

    angular
        .module('app')
        .controller('TeressianCertificateController', TeressianCertificateController);

    TeressianCertificateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout'];

    function TeressianCertificateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("TeressianCertificate/getalldetails", pageid).
                then(function (promise) {
                    $scope.yearlist = promise.acdlist;
                    $scope.courselist = promise.courselist;
                    $scope.semesterlist = promise.semlist;
                    $scope.sectionlist = promise.seclist;
                    $scope.branchlist = promise.branchlist;
                    $scope.quotalist = promise.quotalist;
                    $scope.studentlist = promise.studentlist;

                })
        }

        $scope.getCourse = function (acmAY_Id) {
            $scope.acmAY_Id = '';
            $scope.courselist = [];
            var data = {
                "ASMAY_Id": acmAY_Id
            }
            apiService.create("TeressianCertificate/getcoursedata", data).then(function (promise) {
                $scope.courselist = promise.courselist;
            });
        }
        $scope.getbranchdata = function (ASMAY_Id, AMCO_Id) {
            var data = {
                "AMCO_Id": AMCO_Id,
                "ASMAY_Id": ASMAY_Id
            }
            apiService.create("TeressianCertificate/getbranchdata", data).then(function (promise) {
                $scope.branchlist = promise.branchlist;
            })
        };

        $scope.getsemisterdata = function (ASMAY_Id, AMCO_Id, AMB_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id,
                "AMCO_Id": AMCO_Id,
                "AMB_Id": AMB_Id
            }
            apiService.create("TeressianCertificate/getsemisterdata", data).then(function (promise) {
                $scope.semlist = promise.semlist;
            })
        };


        $scope.getsstudentdata = function (ASMAY_Id, AMCO_Id, AMB_Id, AMSE_ID, ACMS_ID) {
            $scope.albumNameArray1 = [];
            $scope.amcsT_Id = "";
            var data = {
                "ASMAY_ID": ASMAY_Id,
                "AMCO_ID": AMCO_Id,
                "AMSE_ID": AMSE_ID,
                "ACMS_ID": ACMS_ID,
                "AMB_ID": AMB_Id
            }
            apiService.create("TeressianCertificate/getsstudentdata", data).then(function (promise) {

                if (promise.studentlist !== null && promise.studentlist.length > 0) {
                    for (var i = 0; i < promise.studentlist.length; i++) {
                        if (promise.studentlist[i].amcsT_FirstName !== '') {
                            if (promise.studentlist[i].amcsT_MiddleName !== null) {
                                if (promise.studentlist[i].amcsT_LastName !== null) {

                                    $scope.albumNameArray1.push({ amcsT_FirstName: promise.studentlist[i].amcsT_FirstName + ' ' + promise.studentlist[i].amcsT_MiddleName + ' ' + promise.studentlist[i].amcsT_LastName, amcsT_Id: promise.studentlist[i].amcsT_Id });
                                }
                                else {
                                    $scope.albumNameArray1.push({ amcsT_FirstName: promise.studentlist[i].amcsT_FirstName + ' ' + promise.studentlist[i].amcsT_MiddleName, amcsT_Id: promise.studentlist[i].amcsT_Id });
                                }
                            }
                            else {
                                if (promise.studentlist[i].amcsT_LastName !== null) {

                                    $scope.albumNameArray1.push({ amcsT_FirstName: promise.studentlist[i].amcsT_FirstName + ' ' + promise.studentlist[i].amcsT_LastName, amcsT_Id: promise.studentlist[i].amcsT_Id });
                                }
                                else {
                                    $scope.albumNameArray1.push({ amcsT_FirstName: promise.studentlist[i].amcsT_FirstName, amcsT_Id: promise.studentlist[i].amcsT_Id });

                                }
                            }
                        }
                    }
                    $scope.studentlist = $scope.albumNameArray1;
                } else {
                    swal("No Records Found");
                }

            })
        };
        $scope.get_report = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMCO_Id": $scope.amcO_Id,
                    "AMSE_ID": $scope.amsE_Id,
                    "ACMS_ID": $scope.acmS_Id,
                    "AMB_Id": $scope.amB_Id,
                    "AMCST_ID": $scope.amcsT_Id,
                    "report_name": 'noduecer',
                    "param": ''
                }
                apiService.create("TeressianCertificate/GetCertificate", data).
                    then(function (promise) {

                        $scope.reportpart = true;
                       // if ($scope.customercertificate === 'noduecer') {
                            $scope.Name = 'NoDue Certificate';
                            $scope.studentname = promise.getreportdata[0].studentname;
                            $scope.course = promise.getreportdata[0].coursename;
                            $scope.startyear = promise.getreportdata[0].coustart;
                            $scope.endyear = promise.getreportdata[0].couend;
                       // }
                        //else if ($scope.customercertificate === 'coursecer') {
                        //    $scope.Name = 'Course Certificate';
                        //    $scope.studentname = promise.getreportdata[0].studentname;
                        //    $scope.course = promise.getreportdata[0].coursename;
                        //    $scope.startyear = promise.getreportdata[0].coustart;
                        //    $scope.endyear = promise.getreportdata[0].couend;
                        //    $scope.syslabus = promise.getreportdata[0].syslabus;
                        //}
                        //else if ($scope.customercertificate === 'tetransfer') {
                        //    $scope.Name = 'Transfer Certificate';
                        //    $scope.studentname = promise.getreportdata[0].studentname;
                        //    $scope.admissionno = promise.getreportdata[0].admissionno;
                        //    $scope.tcnod = $scope.tcno;
                        //    $scope.dob = promise.getreportdata[0].dob;
                        //    $scope.dobw = promise.getreportdata[0].dobw;
                        //    $scope.nationality = promise.getreportdata[0].nationality;
                        //    $scope.fathername = promise.getreportdata[0].fathername;
                        //    $scope.mothername = promise.getreportdata[0].mothername;
                        //    $scope.religion = promise.getreportdata[0].religion;
                        //    $scope.caste = promise.getreportdata[0].caste;
                        //    $scope.dateofjoin = promise.getreportdata[0].doj;
                        //    $scope.dateofleave = $scope.leaving_DateTime;
                        //    $scope.languages = promise.getreportdata[0].languages;
                        //    $scope.optionals = promise.getreportdata[0].optionals;
                        //    $scope.feedue = promise.getreportdata[0].feedue;
                        //    $scope.characters = $scope.CharId;
                        //}
                    })
            }

        };
        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EligibilityCert/EligibilitycertPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.Clear = function () {
            $state.reload();
        }

        $scope.cleredata = function () {
            $scope.reportpart = false;
        }
        $scope.exportToExcel = function (datatable) {

            var exportHref = Excel.tableToExcel(datatable, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

    }
})();
