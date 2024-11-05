(function () {
    'use strict';
    angular.module('app').controller('AdharNotEnteredListController', AdharNotEnteredListController)

    AdharNotEnteredListController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter']
    function AdharNotEnteredListController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter) {

        $scope.gridflag = false;
        $scope.print_flag = true;
        $scope.excel_flag = true;
        $scope.currentPage = 1;
        //  $scope.itemsPerPage = 10;
        $scope.printdatatable = [];
        $scope.AdhaarNotEnteredList = {};
        $scope.usrname = localStorage.getItem('username');
        
        var pageid = $stateParams.pageId;

        $scope.ddate = {};
        $scope.ddate = new Date();

       
        $scope.obj = {};
        $scope.obj.All = false;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters = 0;
        var copty = "";

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
            copty = "";
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.StuAttRptDropdownList = function () {
            $scope.classname = false;
            $scope.sectionname = false;
            $scope.entryTypeDropDown = [];
            
            apiService.get("AdharNotEnteredList/getdetails/").then(function (promise) {

                $scope.all = true;
                $scope.yearDropdown = promise.academicList;
                $scope.allAcademicYear1 = promise.academicListdefault;

                for (var i = 0; i < $scope.yearDropdown.length; i++) {
                    name = $scope.yearDropdown[i].asmaY_Id;
                    for (var j = 0; j < $scope.allAcademicYear1.length; j++) {
                        if (parseInt(name) === parseInt($scope.allAcademicYear1[j].asmaY_Id)) {
                            $scope.yearDropdown[i].Selected = true;
                            $scope.asmaY_Id = $scope.allAcademicYear1[j].asmaY_Id;
                        }
                    }
                }
                $scope.imgname = promise.photopathname;
                //$scope.classDropdown = promise.classlist;
                $scope.sectionDropdown = promise.sectionList;
                $scope.monthDropdown = promise.monthList;
                $scope.entryTypeDropDown = promise.entryType;
            });
        };

        $scope.saveUseronceemail = function () {


                $scope.albumNameArray = [];
            angular.forEach($scope.printdatatable, function (user) {
                    if (!!user.selected) {
                        $scope.albumNameArray.push(user);
                    }
                })
                if ($scope.albumNameArray.length > 0) {

                   
                        $scope.clslst2 = $scope.albumNameArray;
                        $('#myModalswal').modal('show');
                        $("#myModalswal").modal({ backdrop: false });
                 
                }
                else {
                    swal('Kindly select atleast one student..!');
                    return;
                }
          
        };


        $scope.emailsend = function () {
            $scope.albumNameArray = [];
            angular.forEach($scope.printdatatable, function (user) {
                if (!!user.selected) {
                   // $scope.albumNameArray.push(user);
                    //$scope.albumNameArray.push({ ASMCL_className: user.CLASSNAME, ASMC_SectionName: user.SECTIONNAME, fromdate: user.NOTENTERED_DATE, hrmE_Id: user.hrmE_Id, employeename: user.HRME_EmployeeFirstName, AMST_emailId:user.HRME_EMAIL_Id});
                    $scope.albumNameArray.push({
                        ASMCL_className: user.CLASSNAME,
                        ASMC_SectionName: user.SECTIONNAME,
                        AMAY_DateTime: user.NOTENTERED_DATE,
                        AMST_Id: user.hrmE_Id,
                        AMST_FirstName: user.HRME_EmployeeFirstName,
                        AMST_MiddleName: user.HRME_EMAIL_Id
                    });

                }
            });
            if ($scope.albumNameArray.length > 0) {
                var datalist = {
                   
                    //ASAET_Att_TypeArray: $scope.albumNameArray,
                    resultData: $scope.albumNameArray,
                    flag:"NotEntered"
                };
                //apiService.create("AdharNotEnteredList/emailsend", datalist).then(function (promise) {
                apiService.create("AdmissionRegister/getclass", datalist).then(function (promise) {
                
                    if (promise.returnMsg === "success") {
                        swal('Email Sent  Successfully...!', 'success');
                        $state.reload();
                    }
                    else {
                        swal('Failed to Send SMS..!', 'Failure');
                        return;
                    }
                });
               
            }
        };

        $scope.al_checkentry = function (all) {



            $scope.entrytypearray = [];
            $scope.obj.userentryoption = all;

            var toggleStatus = $scope.obj.userentryoption;
            angular.forEach($scope.entryTypeDropDown, function (role) {
                role.selected = toggleStatus;
            });


            $scope.entrytypearray = [];
            angular.forEach($scope.entryTypeDropDown, function (qq) {
                if (qq.selected == true) {
                    $scope.entrytypearray.push({ ASAET_Att_Type: qq.asaeT_Att_Type })
                }
            });


            if ($scope.obj.userentryoption == true) {
                $scope.getClass();
            }
            else {
                $scope.sectionlist = [];

            }

        }



        $scope.al_checkclass = function (all, ASMCL_Id) {



            $scope.classlistarray = [];
            $scope.obj.usercheckCC = all;

            var toggleStatus = $scope.obj.usercheckCC;
            angular.forEach($scope.classDropdown, function (role) {
                role.selected = toggleStatus;
            });


            $scope.classlistarray = [];
            angular.forEach($scope.classDropdown, function (qq) {
                if (qq.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: qq.asmcL_Id, ASMCL_Id: qq.asmcL_Id })
                }
            });


            if ($scope.obj.usercheckCC == true) {
                $scope.getsection();
                $scope.classflag = true;
            }
            else {
                $scope.sectionlist = [];

            }

        }

        $scope.all_checkC = function (all, ASMCL_Id) {
            $scope.sectionlistarray = [];
            $scope.obj.usercheckC = all;

            var toggleStatus = $scope.obj.usercheckC;
            angular.forEach($scope.arrseclist, function (role) {
                role.selected = toggleStatus;
            });

            $scope.sectionlistarray = [];
            angular.forEach($scope.arrseclist, function (qq) {
                if (qq.selected == true) {
                    $scope.sectionlistarray.push({ ASMS_Id: qq.asmS_Id, ASMCL_Id: qq.asmcL_Id })


                }
            });
        }


        $scope.getclassdata = function () {
            $scope.arrseclist = [];
            $scope.entrytypearray = [];
            $scope.classlistarray = [];
            angular.forEach($scope.entryTypeDropDown, function (qq) {
                if (qq.selected == true) {
                    $scope.entrytypearray.push({ ASAET_Att_Type: qq.asaeT_Att_Type  })
                }
            });
            if ($scope.entrytypearray.length > 0) {
                $scope.getClass();
            }
            else {
                $scope.classlistarray = [];
            }
            $scope.obj.userentryoption = $scope.entryTypeDropDown.every(function (options) {
                return options.selected;
            });
        }

        $scope.getClass = function () {
            $scope.gridflag = false;
            $scope.classDropdown = [];
            $scope.entrytypearray = [];
            if ($scope.entryType == "All") {
                $scope.entrytypearray.push({ ASAET_Att_Type: "D" }, { ASAET_Att_Type: "H" })
            }
            else {
                $scope.entrytypearray.push({ ASAET_Att_Type: $scope.entryType})
            }

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASAET_Att_TypeArray": $scope.entrytypearray
            };
            apiService.create("AdharNotEnteredList/getClassEntryType", data).then(function (promise) {
                $scope.classDropdown = promise.classlist;
            });

        };


        $scope.getsectiondata = function (ASMCL_Id) {
            $scope.arrseclist = [];

            $scope.classlistarray = [];
            angular.forEach($scope.classDropdown, function (qq) {
                if (qq.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: qq.asmcL_Id, ASMCL_Id: qq.asmcL_Id })
                }
            });
            if ($scope.classlistarray.length > 0) {
                $scope.getsection();
            }
            else {
                $scope.sectionlist = [];
            }
            $scope.obj.usercheckCC = $scope.classDropdown.every(function (options) {
                return options.selected;
            });
        }


        $scope.getstudent = function (ASMCL_Id) {

            $scope.obj.usercheckC = $scope.arrseclist.every(function (options) {
                return options.selected;
            });
        }

        $scope.getEntryType = function (asmaY_Id) {
            $scope.arrseclist = [];
            $scope.entryTypeDropDown = [];
            var data = {
                "ASMAY_Id": asmaY_Id
            };

            apiService.create("AdharNotEnteredList/getEntryType", data).then(function (promise) {
                if (promise.entryType !== null && promise.entryType.length >0 ) {

                    $scope.entryTypeDropDown = promise.entryType;
                }
                else {
                    swal("No Record Found");
                }
            });
        };

        $scope.getsection = function () {
            $scope.arrseclist = [];
            var data = {
                "ASMCL_Id": $scope.ASMCL,
                "classlsttwo": $scope.classlistarray,
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("AdharNotEnteredList/getsectionlist", data).then(function (promise) {
                if (promise !== null) {
                    //$scope.entryTypeDropDown = promise.entryType;
                    if (promise.fillsection != null && promise.fillsection.length > 0) {
                        $scope.arrseclist = promise.fillsection;
                    }

                }
                else {
                    swal("No Record Found");
                }
            });
        };

       
        
        $scope.savetmpldata = function () {
          
            $scope.printdatatable = [];
            $scope.searchValue = "";
            $scope.attendenceNotDoneReport = [];


            $scope.classlistarray = [];
            angular.forEach($scope.classDropdown, function (qq) {
                if (qq.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: qq.asmcL_Id })

                }
            });
            $scope.sectionlistarray = [];
            angular.forEach($scope.arrseclist, function (qq) {
                if (qq.selected == true) {
                    $scope.sectionlistarray.push({ ASMS_Id: qq.asmS_Id, ASMCL_Id: qq.asmcL_Id })

                }
            });


            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "flag": $scope.TyPE_Flag,
                "ASAET_Att_Type": $scope.entryType,
                "classlsttwo": $scope.classlistarray,
                "sectionlistarray": $scope.sectionlistarray,
                "fromdate": new Date($scope.FromDate).toDateString(),
                "todate": new Date($scope.ToDate).toDateString(),

            };

            if ($scope.myForm.$valid) {

                        apiService.create("AdharNotEnteredList/getAttendencenotDoneReport", data)
                            .then(function (promise) {

                                $scope.date = new Date();
                                if (promise.attendenceNotDoneReport !== null) {

                                    if (promise.attendenceNotDoneReport.length === 0) {
                                        swal("No Record Found");
                                        $scope.gridflag = false;
                                        $scope.print_flag = true;
                                        $scope.excel_flag = true;
                                    }
                                    else {
                                        $scope.attendenceNotDoneReport = promise.attendenceNotDoneReport;

                                        $scope.presentCountgrid = $scope.attendenceNotDoneReport.length;
                                      
                                        $scope.gridflag = true;
                                        $scope.print_flag = false;
                                        $scope.excel_flag = false;

                                    }
                                }
                                else {

                                    swal("No record Found !");
                                    $scope.gridflag = false;
                                    $scope.print_flag = true;
                                    $scope.excel_flag = true;
                                }
                            });
                    
                

               
            }

            else {
                $scope.submitted = true;
            }
        };

        //$scope.toggleAll = function () {

        //    var toggleStatus = $scope.all2;
        //    angular.forEach($scope.students, function (itm) {
        //        itm.selected = toggleStatus;
        //        if ($scope.all2 == true) {
        //            $scope.printdatatable.push(itm);
        //        }
        //        else {
        //            $scope.printdatatable.splice(itm);
        //        }
        //    });
        //};


        $scope.toggleall = function (All) {
            $scope.printdatatable = [];
            var toggleStatus = All;
            angular.forEach($scope.attendenceNotDoneReport, function (itm) {
                itm.selected = toggleStatus;
                if (All == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        };

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.namme)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.AMST_AdmNo)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.AMST_RegistrationNo)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.ASMCL_ClassName)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.ASMC_SectionName)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.classes)).indexOf($scope.searchValue) >= 0
        };

        //$scope.optionToggled = function (SelectedStudentRecord, index) {

        //    $scope.all2 = $scope.students.every(function (itm) { return itm.selected; });
        //    if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
        //        $scope.printdatatable.push(SelectedStudentRecord);
        //    }
        //    else {
        //        $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
        //    }
        //};


        $scope.optionToggled = function (attendenceNotDoneReport, index) {
            //angular.forEach($scope.attendenceNotDoneReport, function (itm) {
            //    if (itm.selected == true) {
            //        $scope.obj.All = false;
            //    }
            //});
            //$scope.obj.All = $scope.attendenceNotDoneReport.every(function (itm) { return itm.selected; });
            //if ($scope.printdatatable.indexOf(attendenceNotDoneReport) === -1) {
            //    $scope.printdatatable.push(attendenceNotDoneReport);
            //}
            //else {
            //    $scope.printdatatable.splice($scope.printdatatable.indexOf(attendenceNotDoneReport), 1);
            //}




            $scope.all = $scope.attendenceNotDoneReport.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(attendenceNotDoneReport) === -1) {
                if ($scope.printdatatable.length == 0) {
                    $scope.printdatatable.push(attendenceNotDoneReport);
                }
                else {


                    $scope.printdatatable.push(attendenceNotDoneReport);
                }



                $scope.export_flag = false;
            }
            else {

                $scope.printdatatable.splice($scope.printdatatable.indexOf(attendenceNotDoneReport), 1);
            }

        }

        $scope.print = function () {
            
            //$scope.attdate = new Date(ts.FromDate).toDateString();
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


        $scope.printData = function () {

            //apiService.get("AttendanceReport/getdetails/").then(function (promise) {
            //    
            //    if (promise.photopathname != null) {
            //        $scope.imgname = promise.photopathname;
            //    }
            //});


            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
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
            else {
                swal("Please Select Records to be Printed");
            }
        }

        $scope.exportToExcel = function (tableId) {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }

        }

        $scope.submitted = false;

        $scope.angularData =
            {
                'nameList': []
            };

        $scope.vals = [];

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };





        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.hide = function () {
            $scope.gridflag = false;
        };

    }
})();
