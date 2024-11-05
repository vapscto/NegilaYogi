(function () {
    'use strict';
    angular
        .module('app')
        .controller('Classwisestudentconteoller', Classwisestudentconteoller)

    Classwisestudentconteoller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function Classwisestudentconteoller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {
        $scope.searchValue = '';
        $scope.objdd = {};
        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;

        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        //  $scope.imgname = logopath;

        $scope.grid_flag = false;
        $scope.albumNameArraycolumn = [];
        $scope.columnsTest = [];
        $scope.printstudents = [];
        $scope.objj = {};
        $scope.loaddata = function () {

            $scope.currentPage = 1;
            // $scope.itemsPerPage = 10;
            var pageid = 1;
            apiService.getURI("Classwisestudentconteoller/getalldetails", pageid).then(function (promise) {
                $scope.yearlst = promise.fillyear;
                $scope.arrclasslist = promise.fillclass;

                $scope.categoryDropdown = promise.category_list;

                $scope.categoryflag = promise.categoryflag;
                $scope.albumNameArraycolumn = [];
                $scope.columnsTest = [];
                $scope.headertest = [
                    //{ id: 'serial_num', checked: true, value: 'SL No' },
                    { id: 'AMST_FirstName', checked: true, value: 'Student Name' },
                    { id: 'AMST_AdmNo', checked: true, value: 'Adm. No.' },
                    { id: 'AMAY_RollNo', checked: true, value: 'Roll No.' },
                    { id: 'AMST_RegistrationNo', checked: true, value: 'Reg. No.' },
                    { id: 'ASMCL_ClassName', checked: true, value: 'Current Class' },
                    { id: 'ASMC_SectionName', checked: true, value: 'Current Section' },
                    { id: 'JoinedYear', checked: true, value: 'JoinedYear' },
                    { id: 'JoinedClass', checked: true, value: 'JoinedClass' },
                    { id: 'AMST_Sex', checked: true, value: 'Gender' },
                    { id: 'AMST_BloodGroup', checked: true, value: 'Blood Group' },
                    { id: 'AMST_MotherTongue', checked: true, value: 'Mother Tongue' },
                    { id: 'AMST_MobileNo', checked: true, value: 'Phone No.' },
                    { id: 'AMST_emailId', checked: true, value: 'Email ID' },
                    { id: 'AMST_PerAdd3', checked: true, value: 'Permanent Address' },
                    { id: 'AMST_ConCity', checked: true, value: 'Present Address' },
                  //  { id: 'IVRMMC_CountryName', checked: true, value: 'Nationality' },

                    { id: 'IVRMMC_Nationality', checked: true, value: 'Nationality' },
                    { id: 'AMST_BiometricId', checked: true, value: 'Biometric Code' },
                    { id: 'AMST_Date', checked: true, value: 'DOA' },
                    { id: 'AMST_DOB', checked: true, value: 'DOB' },
                    { id: 'AMST_DOB_Words', checked: true, value: "DOB(Words)" },
                    { id: 'PASR_Age', checked: true, value: 'Age' },
                    { id: 'AMST_Photoname', checked: true, value: 'Student Photo' },
                    { id: 'IMC_CasteName', checked: true, value: 'Caste' },
                    { id: 'IMCC_CategoryName', checked: true, value: 'Caste Category' },
                    { id: 'AMC_Name', checked: true, value: 'Category' },
                    { id: 'IVRMMR_Name', checked: true, value: 'Religion' },
                    { id: 'AMST_PerStreet', checked: true, value: 'Street' },
                    { id: 'AMST_PerCity', checked: true, value: 'Town' },
                    { id: 'IVRMMS_Name', checked: true, value: 'State' },
                    { id: 'AMST_FatherName', checked: true, value: 'Father Name' },
                    { id: 'AMST_FatherMobleNo', checked: true, value: 'Father Mobile Number' },
                    { id: 'AMST_FatheremailId', checked: true, value: 'Father Email Id' },
                    { id: 'AMST_FatherOccupation', checked: true, value: 'Father Occupation' },
                    { id: 'AMST_FatherMonIncome', checked: true, value: 'Father Income' },
                    { id: 'AMST_FatherEducation', checked: true, value: 'Father Qualification' },
                    { id: 'AMST_MotherName', checked: true, value: 'Mother Name' },
                    { id: 'AMST_MotherMobileNo', checked: true, value: 'Mother Mobile Number' },
                    { id: 'AMST_MotherEmailId', checked: true, value: 'Mother Email Id' },
                    { id: 'AMST_MotherOccupation', checked: true, value: 'Mother Occupation' },
                    { id: 'AMST_MotherMonIncome', checked: true, value: 'Mother’s Income' },
                    { id: 'AMST_MotherEducation', checked: true, value: 'Mother Qualification' },
                    { id: 'AMST_AadharNo', checked: true, value: 'Aadhar Card Number' },
                    { id: 'AMST_Tpin', checked: true, value: 'Tpin' },
                    { id: 'AMST_GovtAdmno', checked: true, value: 'GovtAdmno' },
                    { id: 'AMST_BPLCardNo', checked: true, value: 'GovtNo' },
                    { id: 'AMST_BirthPlace', checked: true, value: 'Place Of Birth' },
                    { id: 'AMSTPS_PrvSchoolName', checked: true, value: 'Previous School Name' },
                    { id: 'AMSTPS_PreviousClass', checked: true, value: 'Previous Class' },
                    { id: 'AMSTPS_PrvTCDate', checked: true, value: 'Previous TC Date' },
                    { id: 'AMSTPS_PrvTCNO', checked: true, value: 'Previous TCNo' },
                    { id: 'AMST_Village', checked: true, value: 'Village' },
                    { id: 'AMST_Town', checked: true, value: 'Town' },
                    { id: 'AMST_Taluk', checked: true, value: 'Taluk' },
                    { id: 'IVRMMD_Name', checked: true, value: 'District' },
                    { id: 'AMST_PlaceOfBirthState', checked: true, value: 'State' },
                    { id: 'AMST_PlaceOfBirthCountry', checked: true, value: 'Country' },
                    { id: 'SPCCMH_HouseName', checked: true, value: 'House' },
                    { id: 'Electives', checked: true, value: 'Electives' },
                    { id: 'AMST_PENNo', checked: true, value: 'Pen No' }
                ]
                // 
            });
        };

        $scope.clssec = true;
        $scope.albumNameArraycolumn = [];
        //category
        //$scope.isOptionsRequiredclass = function () {
        //    return !$scope.categoryDropdown.some(function (item) {
        //        return item.selected;
        //    });
        //};

        //$scope.al_checkcategory = function (all, ASMCL_Id) {


        //    $scope.categorylistarray = [];
        //    $scope.obj.usercheckCC = all;

        //    var toggleStatus = $scope.obj.usercheckCC;
        //    angular.forEach($scope.categoryDropdown, function (role) {
        //        role.selected = toggleStatus;
        //    });


        //    $scope.categorylistarray = [];
        //    angular.forEach($scope.categoryDropdown, function (qq) {
        //        if (qq.selected == true) {
        //            $scope.categorylistarray.push({ AMC_Id: qq.amC_Id })
        //        }
        //    });




        //}


        //$scope.togchkbxC = function () {
        //    $scope.categorylistarray = [];
        //    angular.forEach($scope.categoryDropdown, function (qq) {
        //        if (qq.selected == true) {
        //            $scope.categorylistarray.push({ AMC_Id: qq.amC_Id })
        //        }
        //    })
        //}
        //


        $scope.al_checkclass = function (all, ASMCL_Id) {

            $scope.arrseclist = [];
           
            $scope.classlistarray = [];
            $scope.obj.usercheckCC = all;
            if ($scope.obj.usercheckC == true) {
                $scope.obj.usercheckC = false;
            }

            var toggleStatus = $scope.obj.usercheckCC;
            angular.forEach($scope.arrclasslist, function (role) {
                role.selected = toggleStatus;
            });


            $scope.classlistarray = [];
            angular.forEach($scope.arrclasslist, function (qq) {
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

        $scope.getsection = function () {
            $scope.arrseclist = [];
     
            
            $scope.arrseclist = [];

            $scope.classlistarray = [];
            angular.forEach($scope.arrclasslist, function (qq) {
                if (qq.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: qq.asmcL_Id, ASMCL_Id: qq.asmcL_Id })
                }
            });
            var data = {
                "ASMCL_Id": $scope.ASMCL,
                "ASMAY_Id": $scope.ASMAY,
                "classlsttwo": $scope.classlistarray
            };

            apiService.create("Classwisestudentconteoller/getsection", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.fillsection != null && promise.fillsection.length > 0) {
                        $scope.arrseclist = promise.fillsection;
                    }
                   
                }
                else {
                    swal("No Record Found");
                }
            });
        };


        $scope.isOptions = function () {
            return !$scope.arrclasslist.some(function (optionn) {
                return optionn.selected;
            });
        }
        $scope.isOptions1 = function () {
            return !$scope.arrseclist.some(function (optionn1) {
                return optionn1.selected;
            });
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


        $scope.getindsection = function ( ASMCL_Id) {

            $scope.sectionlistarray = [];
            angular.forEach($scope.arrseclist, function (qq) {
                if (qq.selected == true) {
                    $scope.sectionlistarray.push({ ASMS_Id: qq.asmS_Id, ASMCL_Id: qq.asmcL_Id })


                }
            });


        }

        $scope.addColumn = function (role) {
            $scope.all2 = $scope.headertest.every(function (itm) { return itm.selected; });
            if (role.selected === true) {
                $scope.albumNameArraycolumn.push(role);
            }
            else {
                var som = $scope.albumNameArraycolumn.indexOf(role);
                $scope.albumNameArraycolumn.splice($scope.albumNameArraycolumn.indexOf(role), 1);
            }
        };

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.value).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };

        $scope.submitted = false;
        $scope.classlistarray = [];
        $scope.sectionlistarray = [];
        $scope.ShowReport = function () {

            $scope.printstudents = [];
            $scope.search = "";
            $scope.objdd.all = false;
            $scope.albumNameArraycolumn = [];
            angular.forEach($scope.headertest, function (role) {
                if (!!role.selected) $scope.albumNameArraycolumn.push({
                    columnID: role.id,
                    columnName: role.value
                });
            });

         

            $scope.classlistarray = [];
            angular.forEach($scope.arrclasslist, function (qq) {
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

            var AMC_Id = 0
            if ($scope.objj.amC_Id != 'All') {
                AMC_Id = $scope.objj.amC_Id
            }
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY,
                    "classlsttwo": $scope.classlistarray,
                    "sectionlistarray": $scope.sectionlistarray,
                    "TempararyArrayheadList": $scope.albumNameArraycolumn,
                    "flag": $scope.admstdpro,
                    "AMC_Id": AMC_Id

                    // "categorylistarray": $scope.categorylistarraynew
                };
                apiService.create("Classwisestudentconteoller/Getreportdetails", data).then(function (promise) {
                    if (promise.alldatagridreport !== null && promise.alldatagridreport.length > 0) {

                        $scope.students = promise.alldatagridreport;
                        $scope.presentCountgrid = $scope.students.length;

                        $scope.columnsTest = promise.tempararyArrayheadList;

                      

                        $scope.columnsTest2 = [];
                        angular.forEach($scope.columnsTest, function (ddd) {
                            $scope.columnsTest2.push(ddd);
                        });


                        if (promise.AMC_logo != null) {
                            $scope.imgname = promise.AMC_logo[0].amC_FilePath;
                        }
                        else {
                            $scope.imgname = logopath;
                        }

                        angular.forEach($scope.columnsTest2, function (qwe) {
                            qwe.field = qwe.columnID;
                            qwe.title = qwe.columnName;
                            qwe.width = 120;
                        });


                        if ($scope.format === "1") {
                            var gridall;

                            var id = 0;
                            angular.forEach($scope.students, function (dd) {
                                id = id + 1;
                                dd.SNo = id;
                            });

                            console.log($scope.students);
                            console.log($scope.columnsTest2);

                            angular.forEach($scope.columnsTest2, function (dd) {
                                id = id + 1;
                                dd.SNo = id;
                            });

                            $(document).ready(function () {
                                initGridall();
                            });

                            function initGridall() {
                                $('#gridlst').empty();
                                gridall = $("#gridlst").kendoGrid({
                                    toolbar: ["excel", "pdf"],
                                    excel: {
                                        fileName: "Student Wise Class Report.xlsx",
                                        proxyURL: "",
                                        filterable: true,
                                        allPages: true
                                    },
                                    pdf: {
                                        fileName: "Student Wise Class Report.pdf",
                                        allPages: true
                                    },
                                    dataSource: {
                                        data: $scope.students,
                                        pageSize: 10
                                    },
                                    sortable: true,
                                    pageable: true,
                                    groupable: false,
                                    filterable: true,
                                    columnMenu: true,
                                    reorderable: true,
                                    resizable: true,
                                    columns: $scope.columnsTest2,
                                    dataBound: function () {
                                        var pagenum = this.dataSource.page();
                                        var pageitms = this.dataSource.pageSize()
                                        var rows = this.items();
                                        $(rows).each(function () {
                                            var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                            var rowLabel = $(this).find(".row-numberind");
                                            $(rowLabel).html(index);
                                        });
                                    }

                                }).data("kendoGrid");
                            }
                            $scope.showbutton = false;
                        }

                        else {
                            angular.forEach($scope.students, function (objectt) {
                                if (objectt.AMST_FirstName != undefined) {
                                    var string = objectt.AMST_FirstName;
                                    objectt.AMST_FirstName = string.replace(/  +/g, ' ');
                                }
                                if (objectt.AMST_FatherOccupation != '0' && objectt.AMST_FatherOccupation != undefined) {

                                    objectt.AMST_FatherOccupation = objectt.AMST_FatherOccupation;
                                } else {
                                    objectt.AMST_FatherOccupation = 'Not Available';
                                }

                                if (objectt.AMST_MotherOccupation != '0' && objectt.AMST_MotherOccupation != undefined) {

                                    objectt.AMST_MotherOccupation = objectt.AMST_MotherOccupation;
                                } else {
                                    objectt.AMST_MotherOccupation = 'Not Available';
                                }

                                if (objectt.AMST_FatherEducation != '0' && objectt.AMST_FatherEducation != undefined) {

                                    objectt.AMST_FatherEducation = objectt.AMST_FatherEducation;
                                } else {
                                    objectt.AMST_FatherEducation = 'Not Available';
                                }

                                if (objectt.AMST_MotherEducation != '0' && objectt.AMST_MotherEducation != undefined) {

                                    objectt.AMST_MotherEducation = objectt.AMST_MotherEducation;
                                } else {
                                    objectt.AMST_MotherEducation = 'Not Available';
                                }

                                if (objectt.AMST_BirthPlace != '0' && objectt.AMST_BirthPlace != undefined) {

                                    objectt.AMST_BirthPlace = objectt.AMST_BirthPlace;
                                } else {
                                    objectt.AMST_BirthPlace = 'Not Available';
                                }
                            });
                            $scope.showbutton = true;
                        }

                        $scope.grid_flag = true;


                        //angular.forEach($scope.arrclasslist, function (cls) {
                        //    if (cls.asmcL_Id == $scope.obj.usercheckCC) {
                        //        $scope.class = cls.asmcL_ClassName;
                        //    }
                        //});

                        $scope.class = '';
                        angular.forEach($scope.arrclasslist, function (qq) {
                            if (qq.selected == true) {
                                //$scope.classlistarray.push({ ASMCL_Id: qq.asmcL_Id })
                                $scope.class += qq.asmcL_ClassName + ',';

                            }
                        });

                        //angular.forEach($scope.arrseclist, function (clse) {
                        //    if (clse.asmS_Id == $scope.obj.usercheckC) {
                        //        $scope.section = clse.asmC_SectionName;
                        //    }
                        //});


                        $scope.section = '';
                        angular.forEach($scope.arrseclist, function (qq) {
                            if (qq.selected == true) {
                               // $scope.sectionlistarray.push({ ASMS_Id: qq.asmS_Id, ASMCL_Id: qq.asmcL_Id })
                                $scope.section += qq.ASMC_SectionName + ',';
                            }
                        });

                      //  $scope.dd = $scope.class.trim(",");


                        var lastCharcls = $scope.class.slice(-1);
                        if (lastCharcls == ',') {
                            $scope.classname = $scope.class.slice(0, -1);
                        }

                        var lastCharsec = $scope.section.slice(-1);
                        if (lastCharsec == ',') {
                            $scope.sectionname = $scope.section.slice(0, -1);
                        }
                        $scope.section = $scope.section.trim(',');
                        angular.forEach($scope.yearlst, function (clse) {
                            if (clse.asmaY_Id == $scope.ASMAY) {
                                $scope.yearname = clse.asmaY_Year;
                            }
                        });
                    }
                    else {
                        $scope.showbutton = false;
                        $scope.grid_flag = false;
                        $scope.students = "";
                        swal("No Records Found !!");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.searchsource = function () {
            var entereddata = $scope.search;
            var data = {
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("Classwisestudentconteoller/1", data).then(function (promise) {
                $scope.students = promise.alldatagridreport;
                swal("searched Sucessfully");
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.headertest.some(function (options) {
                return options.selected;
            });
        };

        //Toggle all header check box
        $scope.Toggle_header = function () {

            var toggleStatus = $scope.all2;
            angular.forEach($scope.headertest, function (itm) {
                itm.selected = toggleStatus;
            });
        };

        //for export to excel
        $scope.exportToExcel = function (export_id) {

            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {


                var exportHref = Excel.tableToExcel(export_id, 'sheet name');
                var excelname = "Class Wise Students Details Report.xlsx";
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelname;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);


                //var exportHref = Excel.tableToExcel(export_id, 'WireWorkbenchDataExport');
                //$timeout(function () {
                //    location.href = exportHref;
                //}, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        };


        $scope.fetchclassbyYearId = function () {
$scope.arrclasslist=[];
            var data = {
                "ASMAY_Id": $scope.ASMAY
            };
            apiService.create("Classwisestudentconteoller/fetchclassbyYearId", data).then(function (promise) {
                    if (promise.fillclass !== null && promise.fillclass.length > 0) {
                        $scope.arrclasslist = promise.fillclass;
                    }else{
$scope.arrclasslist=[];
}
                });
        };

        //for print     
        $scope.printData = function (printSectionId) {
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
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
        };

        $scope.toggleAll = function () {

            var toggleStatus = $scope.objdd.all;
            angular.forEach($scope.students, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.objdd.all == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
        };

        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.objdd.all = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }
        };

        $scope.clear = function () {
            $state.reload();
            $scope.grid_flag = false;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Onchagneformat = function () {
            $scope.students = [];
            $scope.showbutton = false;
            $scope.grid_flag = false;
        };

        $scope.Onchagnetype = function () {
            $scope.students = [];
            $scope.showbutton = false;
            $scope.grid_flag = false;
        };
    }
})();

