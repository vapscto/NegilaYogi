(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeStudentHeadWiseReportController', FeeStudentHeadWiseReportController)

    FeeStudentHeadWiseReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$compile']
    function FeeStudentHeadWiseReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $compile) {
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }


        $scope.logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            $scope.logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.print = false;
        $scope.rndind = "All";
        $scope.individual_Name_Regno = false;
        $scope.individual_Student = false;

        $scope.classwise = true;
        $scope.filterdata = "NameRegNo";
        $scope.StudentName = "";
        $scope.AMST_AdmNo = "";
        $scope.classname = "";
        $scope.sectionname = "";
        $scope.FatherName = "";
        $scope.AMST_MobileNo = "";
        $scope.Address = "";
        $scope.total = "";
        $scope.termname = "";


        $scope.totcountsearch = 0;


        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        $scope.cfg = {};
        $scope.loaddata = function () {
            $scope.printdatatablegrp = [];
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("FeeOpeningBalance/getalldetails123", pageid).
                then(function (promise) {

                    $scope.yearlist = promise.acayear;

                    $scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;

                    $scope.sectiondrpre = promise.sectionlist;
                    $scope.clsdrpdown = promise.classlist;
                    $scope.term = promise.busroutelist;

                    $scope.fillmasterhead = promise.fillmasterhead;



                    $scope.sort = function (keyname) {
                        $scope.sortKey = keyname;   //set the sortKey to the param passed
                        $scope.reverse = !$scope.reverse; //if true make it false and vice versa
                    }

                })
        }


        $scope.changeacademicyear = function () {
            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
            }

            apiService.create("FeeOpeningBalance/onselectacademicyear", data).
                then(function (promise) {

                    if (promise.tempararyArrayhEADListnew.length > 0) {
                        $scope.students_list = promise.tempararyArrayhEADListnew;
                        $scope.totcountfirst = promise.tempararyArrayhEADListnew.length;
                    }
                    else {
                        swal("No Records Found")
                        $scope.students_list = {};
                    }

                })
        }

        $scope.onselectmodeof = function () {

            if ($scope.rndind == "All" && $scope.Balance_report == true && $scope.status != null && $scope.status != "") {
                $scope.ShowReportdata();
            }
            else {
                if ($scope.clsdrp != null && $scope.clsdrp != "") {
                    var data = {
                        "asmay_id": $scope.cfg.ASMAY_Id,
                        "filterinitialdata": $scope.filterdata,
                        "fillseccls": $scope.sectiondrp,
                        "fillclasflg": $scope.clsdrp,
                        "studenttype": $scope.stustatus,
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("FeeOpeningBalance/getgroupmappedheads", data).
                        then(function (promise) {

                            if (promise.admsudentslist != null && promise.admsudentslist != "") {
                                $scope.studentlst = promise.admsudentslist;
                                $scope.Amst_Id = "";

                            }
                            else {
                                if ($scope.rndind == "Individual") {
                                    swal("No Student Found");
                                }
                                $scope.studentlst = "";
                            }
                        })
                }
                else {
                    swal("Select Class");
                }
            }
        };




        $scope.onclickloaddata = function () {
            if ($scope.rndind == "All") {
                //$scope.rbnsforall = true;
                $scope.individual_Name_Regno = false;
                //$scope.rbnsNameforall = true;    
                $scope.individual_Student = false;
                $scope.classwise = true;
                $scope.categorywise = false;

            }
            else if ($scope.rndind == "Individual") {
                //$scope.rbnsforall = false;
                $scope.individual_Name_Regno = true;
                // $scope.rbnsNameforall = false;
                $scope.individual_Student = true;
                $scope.classwise = true;
                $scope.categorywise = false;

            }

        };
        $scope.onselectclass = function () {

            if ($scope.clsdrp != 0) {


                var data = {
                    "asmay_id": $scope.cfg.ASMAY_Id,
                    "ASMCL_Id": $scope.clsdrp,
                    //  "Adm_no_name": $scope.radio_button,
                }
                apiService.create("FeeOpeningBalance/getclshead/", data).then(function (promise) {

                    if (promise.sectionlist != null && promise.sectionlist != "") {
                        $scope.sectiondrpre = promise.sectionlist;
                        $scope.headlst = promise.fillmasterhead;
                    }
                    else {
                        swal("No Section Found Kindly select Another Class/Year");
                        $scope.fee_head_flag = true;
                        $scope.fee_head = false;
                    }

                });
            }
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.ShowReportdata = function () {

            //var FMTId = [];
            //$scope.termlist = [];
            if ($scope.myForm.$valid) {


                //angular.forEach($scope.term, function (ty) {
                //    if (ty.fmT_Id_chk) {
                //        FMTId.push(ty.fmT_Id);
                //        $scope.termlist.push(ty);
                //    }
                //})




                    var data = {

                        "asmay_id": $scope.cfg.ASMAY_Id,
                        "fillclasflg": $scope.clsdrp,
                        "fillseccls": $scope.sectiondrp,

                    }

                apiService.create("FeeHeadWiseReport/getreport/", data).
                    then(function (promise) {

                        if (promise.getreportdata != null && promise.getreportdata != "") {
                            $scope.getreportdata = promise.getreportdata;

                            $scope.studentlist = [];

                            angular.forEach($scope.getreportdata, function (dev) {
                                if ($scope.studentlist.length === 0) {
                                    $scope.studentlist.push({
                                        AMST_Id: dev.AMST_Id,
                                        StudentName: dev.StudentName,
                                        AMST_AdmNo: dev.AMST_AdmNo,
                                        ASMCL_ClassName: dev.ASMCL_ClassName,
                                        ASMC_SectionName: dev.ASMC_SectionName
                                    });
                                } else if ($scope.studentlist.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.studentlist, function (emp) {
                                        if (emp.AMST_Id === dev.AMST_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.studentlist.push({
                                            AMST_Id: dev.AMST_Id,
                                            StudentName: dev.StudentName,
                                            AMST_AdmNo: dev.AMST_AdmNo,
                                            ASMCL_ClassName: dev.ASMCL_ClassName,
                                            ASMC_SectionName: dev.ASMC_SectionName
                                            
                                        });
                                    }
                                }
                            });


                            console.log($scope.studentlist);

                            angular.forEach($scope.studentlist, function (ddd) {
                                $scope.templist = [];
                                var totalcharges = 0;
                                var totalpaid = 0;
                                var totalbalance = 0;
                                var headcount = 0; 
                                 
                                angular.forEach($scope.getreportdata, function (dd) {
                                    if (dd.AMST_Id === ddd.AMST_Id) {
                                        totalcharges += dd.TotalCharges;
                                        totalpaid += dd.Paid;
                                        totalbalance += dd.TotalTobePaid;
                                        headcount += 1;
                                        $scope.templist.push({ AMST_Id: dd.AMST_Id, FMH_FeeName: dd.FMH_FeeName, TotalCharges: dd.TotalCharges, Paid: dd.Paid, TotalTobePaid: dd.TotalTobePaid ,totheadcount: headcount });
                                    }
                                });
                                ddd.headdetails = $scope.templist;
                                ddd.tot_charges = totalcharges;
                                ddd.total_paid = totalpaid;
                                ddd.total_balance = totalbalance;
                            });

                            //Added By Praveen gouda 09/12/2023                            
                                var totalcharges = 0;
                                var totalpaid = 0;
                                var totalbalance = 0;
                                angular.forEach($scope.studentlist, function (gp) {
                                    totalcharges += gp.tot_charges;
                                    totalpaid += gp.total_paid;
                                    totalbalance += gp.total_balance; 
                                })
                                $scope.totalcharges = totalcharges;
                                $scope.totalpaid = totalpaid;
                                $scope.totalbalance = totalbalance;                          
                        }
                        else {
                            swal("No Record Found");

                            $scope.Clearid();
                        }
                    })

            }
            else {
                $scope.submitted = true;

            }
        }
        //search start

        $scope.search_flag = false;
        $scope.search123 = "";
        var search_s = "";
        var list_s = [];
        $scope.onselectsearch = function () {
            search_s = $scope.search123;
            list_s = $scope.receiptgrid;
            if (search_s == "" || search_s == undefined) {
                swal("Select Any Field For Search");
                $scope.search_flag = false;
            }
            else {
                $scope.search_flag = true;

                if ($scope.search123 == "3" || $scope.search123 == "4") {
                    $scope.txt = false;
                    $scope.numbr = true;
                    // $scope.dat = false;

                }
                //else if ($scope.search123 == "4") {

                //    $scope.txt = false;
                //    $scope.numbr = false;
                //    $scope.dat = true;

                //}
                else {
                    $scope.txt = true;
                    $scope.numbr = false;
                    // $scope.dat = false;

                }
                $scope.searchtxt = "";
                //   $scope.searchdat = "";
                $scope.searchnumbr = "";

            }
        }
        $scope.ShowSearch_Report = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            if ($scope.searchtxt != "" || $scope.searchnumbr != "") {
                if ($scope.search123 == "3" || $scope.search123 == "4") {
                    var data = {
                        "searchType": $scope.search123,
                        "searchnumber": $scope.searchnumbr
                    }
                }
                else {
                    var data = {
                        "searchType": $scope.search123,
                        "searchtext": $scope.searchtxt
                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeOpeningBalance/searching", data).
                    then(function (promise) {

                        $scope.students_list = promise.tempararyArrayhEADListnew;
                        $scope.totcountfirst = promise.tempararyArrayhEADListnew.length;

                        if (promise.tempararyArrayhEADListnew == null || promise.tempararyArrayhEADListnew == "") {
                            swal("Record Does Not Exist For Searched Data !!!!!!")
                        }
                    })
            }
            else {
                swal("Data Is Needed For Search ");
            }
        }
        $scope.clearsearch = function () {

            $state.reload();
            $scope.search123 = "";
            $scope.search_flag = false;
            $scope.searchtxt = "";
            $scope.searchnumbr = "";
            $scope.searchdat = "";
        }
        //search end

        $scope.submitted = false;
        $scope.students1 = [];
        $scope.printToCart = function () {

            var innerContents = document.getElementById("template").innerHTML;

            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BArrearReport/BArrearReportPdf.css"/>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


        $scope.printToReport = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                //'<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BArrearReport/BArrearReportPdf.css"/>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.exportToExcel = function () {
            var exportHref = Excel.tableToExcel(printSection, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }

        $scope.Clearid = function () {
            $state.reload();
            $scope.loaddata();
        }

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
                itm.checkedvalue = toggleStatus;
            });
        }

        $scope.optionToggled = function (user) {
            $scope.all = $scope.students.every(function (itm) { return itm.checkedvalue; })
        }


        $scope.saveUseronce = function (StudentName, AMST_AdmNo, classname, sectionname, FatherName, AMST_MobileNo, Address, total) {
            $scope.StudentName = "";
            $scope.AMST_AdmNo = "";
            $scope.classname = "";
            $scope.sectionname = "";
            $scope.FatherName = "";
            $scope.AMST_MobileNo = "";
            $scope.Address = "";
            $scope.total = "";
            $scope.termname = "";
            $('#myModalswal').modal('show');

            $scope.StudentName = StudentName;
            $scope.AMST_AdmNo = AMST_AdmNo;
            $scope.classname = classname;
            $scope.sectionname = sectionname;
            $scope.FatherName = FatherName;
            $scope.AMST_MobileNo = AMST_MobileNo;
            $scope.Address = Address;
            $scope.total = total;

            for (var i = 0; i < $scope.termlist.length; i++) {
                $scope.termname += $scope.termlist[i].fmT_Name + " ,"
            }



        }

        $scope.templateselect = function () {
            $scope.str2 = "";



            for (var i = 0; i < $scope.getfeedefaultertemplate.length; i++) {

                if (Number($scope.defaulter) == $scope.getfeedefaultertemplate[i].iseS_Id) {
                    $scope.words = $scope.amountinwords(Number($scope.total));
                    $scope.htmldata = $scope.getfeedefaultertemplate[i].iseS_MailBody;
                    $scope.htmldata = $scope.htmldata.replace("[FATHERNAME]", $scope.FatherName);
                    $scope.htmldata = $scope.htmldata.replace("[ADDRESS]", $scope.Address);
                    $scope.htmldata = $scope.htmldata.replace("[ADMNO]", $scope.AMST_AdmNo);
                    $scope.htmldata = $scope.htmldata.replace("[NAME]", $scope.StudentName);
                    $scope.htmldata = $scope.htmldata.replace("[CLASS]", $scope.classname);
                    $scope.htmldata = $scope.htmldata.replace("[AMOUNT]", $scope.total);
                    $scope.htmldata = $scope.htmldata.replace("[CONTACTNO]", $scope.AMST_MobileNo);
                    $scope.htmldata = $scope.htmldata.replace("[FATHERNAME]", $scope.FatherName);

                    $scope.htmldata = $scope.htmldata.replace("[ADMNO]", $scope.AMST_AdmNo);
                    $scope.htmldata = $scope.htmldata.replace("[NAME]", $scope.StudentName);

                    $scope.htmldata = $scope.htmldata.replace("[AMOUNTINWORDS]", $scope.words);
                    $scope.htmldata = $scope.htmldata.replace("[TERMNAME]", $scope.termname);
                    var e1 = angular.element(document.getElementById("template"));
                    $compile(e1.html($scope.htmldata))(($scope));
                }
            }


        }


        $scope.amountinwords = function convertNumberToWords(atotalc) {
            var words = new Array();
            words[0] = '';
            words[1] = 'One';
            words[2] = 'Two';
            words[3] = 'Three';
            words[4] = 'Four';
            words[5] = 'Five';
            words[6] = 'Six';
            words[7] = 'Seven';
            words[8] = 'Eight';
            words[9] = 'Nine';
            words[10] = 'Ten';
            words[11] = 'Eleven';
            words[12] = 'Twelve';
            words[13] = 'Thirteen';
            words[14] = 'Fourteen';
            words[15] = 'Fifteen';
            words[16] = 'Sixteen';
            words[17] = 'Seventeen';
            words[18] = 'Eighteen';
            words[19] = 'Nineteen';
            words[20] = 'Twenty';
            words[30] = 'Thirty';
            words[40] = 'Forty';
            words[50] = 'Fifty';
            words[60] = 'Sixty';
            words[70] = 'Seventy';
            words[80] = 'Eighty';
            words[90] = 'Ninety';
            atotalc = atotalc.toString();
            var atemp = atotalc.split(".");
            var number = atemp[0].split(",").join("");
            var n_length = number.length;
            var words_string = "";
            if (n_length <= 9) {
                var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
                var received_n_array = new Array();
                for (var i = 0; i < n_length; i++) {
                    received_n_array[i] = number.substr(i, 1);
                }
                for (var i = 9 - n_length, j = 0; i < 9; i++ , j++) {
                    n_array[i] = received_n_array[j];
                }
                for (var i = 0, j = 1; i < 9; i++ , j++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        if (n_array[i] == 1) {
                            n_array[j] = 10 + parseInt(n_array[j]);
                            n_array[i] = 0;
                        }
                    }
                }
                atotalc = "";
                for (var i = 0; i < 9; i++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        atotalc = n_array[i] * 10;
                    } else {
                        atotalc = n_array[i];
                    }
                    if (atotalc != 0) {
                        words_string += words[atotalc] + " ";
                    }
                    if ((i == 1 && atotalc != 0) || (i == 0 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Crores ";
                    }
                    if ((i == 3 && atotalc != 0) || (i == 2 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Lakhs ";
                    }
                    if ((i == 5 && atotalc != 0) || (i == 4 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Thousand ";
                    }
                    if (i == 6 && atotalc != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                        words_string += "Hundred and ";
                    } else if (i == 6 && atotalc != 0) {
                        words_string += "Hundred ";
                    }
                }
                words_string = words_string.split("  ").join(" ");
            }
            return words_string;
        }

    }
})();

