﻿(function () {
    'use strict';
    angular
        .module('app')
        .controller('NonBookTransactionController', NonBookTransactionController)

    NonBookTransactionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function NonBookTransactionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

        $scope.submitted = false;
        $scope.minDate = new Date();
        $scope.maxDate = new Date();
        $scope.status = false;
        $scope.BokStud = false;
        $scope.issuebutton = true;
        $scope.returndate = false;
        $scope.showflag = false;
        $scope.issuedate = false;
        $scope.butnfield = true;

        //-------------change date according to maxday's
        $scope.ChangeDate = function (Issue_Date) {
            debugger;
            if ($scope.Issue_Date != "") {
                $scope.returndate = true;
                debugger;
                $scope.newdate = new Date($scope.Issue_Date);

                $scope.ret_date = $scope.newdate.setDate($scope.newdate.getDate() + $scope.maxdays);
                $scope.Return_Date = new Date($scope.ret_date);
                $scope.Due_Date = new Date($scope.ret_date);
            }
            else {
                $scope.returndate = false;
            }
        }

        $scope.transtypechange = function () {
            $scope.issuedate = false;
            $scope.returndate = false;
            $scope.newdate = '';
            $scope.ret_date = '';
            $scope.Issue_Date = '';
            $scope.Return_Date = '';
            $scope.Due_Date = '';
            $scope.LMBANO_Id = '';
            $scope.AMST_Id = '';
            $scope.HRME_Id = '';
            $scope.HRMD_Id = '';
            $scope.barcode1 = '';
            $scope.barcode = '';
            $scope.Loaddata();
        }


        $scope.booktype = 'issue';
        $scope.issuertype1 = 'std';
        $scope.bookornonbook = 'book';

        $scope.onSelectlibrary = function () {

            $scope.Loaddata();
        }



        $scope.search = '';
        $scope.filterValue = function (obj) {
            debugger;
            if ($scope.search != '' && $scope.search != null && $scope.search != undefined) {
                return (angular.lowercase(obj.lmbanO_AccessionNo)).indexOf(angular.lowercase($scope.search)) >= 0;
            }

        }

        //-------------Page Load
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            $scope.msterliblist = [];
            var data = {
                "booktype": $scope.booktype,
                "issuertype": $scope.issuertype1,
                "bookcat_type": $scope.bookornonbook,
                "LMAL_Id": $scope.LMAL_Id,

            }

            debugger;
            var pageid = 2;
            apiService.create("NonBookTransaction/getdetails", data).then(function (promise) {

                if (promise.msg != 'N') {
                    if ($scope.issuertype1 == 'std') {

                        if (promise.clamsg != 'NS') {
                            $scope.fillacademiyear = promise.yearlist;
                            $scope.studlist = promise.studentlist;
                            $scope.msterliblist = promise.msterliblist;
                            $scope.msterliblist1 = promise.msterliblist1;

                            if ($scope.msterliblist.length > 0) {
                                $scope.LMAL_Id = promise.lmaL_Id;
                            }

                            //for (var i = 0; i < $scope.msterliblist.length; i++) {
                            //    //  if ($scope.currentYear[0].asmaY_Id == $scope.fillacademiyear[i].asmaY_Id) {
                            //    $scope.LMAL_Id = $scope.msterliblist[i].lmaL_Id;
                            //    // }
                            //}
                            debugger;
                            for (var i = 0; i < $scope.fillacademiyear.length; i++) {
                                //  if ($scope.currentYear[0].asmaY_Id == $scope.fillacademiyear[i].asmaY_Id) {
                                $scope.ASMAY_Id = $scope.fillacademiyear[i].asmaY_Id;
                                // }
                            }

                            if (promise.classlist != null && promise.classlist.length > 0) {
                                $scope.classlist = promise.classlist;
                                $scope.ASMCL_Id = $scope.classlist[0].asmcL_Id;
                                $scope.sectionlist = promise.sectionlist;
                                $scope.asmS_Id = $scope.sectionlist[0].asmS_Id;
                            }


                            if (promise.studentlist != null || promise.studentlist != undefined) {
                                //$scope.count = promise.studentCount;
                                $scope.studentList = promise.studentlist;
                                console.log($scope.studentList);
                            }

                            //if (promise.studentCount > 0) {
                            //    $scope.count = promise.studentCount;
                            //    $scope.studentList = promise.studentlist;
                            //    console.log($scope.studentList);
                            //}
                            else {
                                swal("No records found for selected academicYear,class and section");
                                $scope.count = 0;
                            }
                        }
                        else {
                            swal("Class is Not mapped for this User");
                        }


                    }

                    $scope.booktitle = promise.booktitle;

                    $scope.getalldata = promise.getalldata;
                    $scope.stafftlist = promise.stafftlist;
                    $scope.departmentlist = promise.departmentlist;
                    $scope.filldepartment = promise.filldepartment;
                    $scope.filldesignation = promise.filldesignation;
                    $scope.configdata = promise.configdata;
                }
                else {
                    swal("Libaray is Not mapped for this User");
                }



                // console.log($scope.booktitle);

            })


        }
        //----------------------END Load

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }


        //======================Issue Book==================//
        $scope.Savedata = function () {
            // alert($scope.maxitem)
            debugger;
            if ($scope.myForm.$valid) {


                var b_id = $scope.LMBANO_Id.lmB_Id;
                var a_id = $scope.LMBANO_Id.lmbanO_Id;
                var data = {}

                var issuedate = $scope.Issue_Date == null ? "" : $filter('date')($scope.Issue_Date, "yyyy-MM-dd");
                var returndate = $scope.Return_Date == null ? "" : $filter('date')($scope.Return_Date, "yyyy-MM-dd");
                var duedate = $scope.Due_Date == null ? "" : $filter('date')($scope.Due_Date, "yyyy-MM-dd");
                if ($scope.issuertype1 == 'std') {
                    var sid = $scope.AMST_Id.amsT_Id

                    data = {
                        "Book_Trans_Id": $scope.Book_Trans_Id,
                        "LMBANO_Id": a_id,
                        "LMB_Id": b_id,
                        "LMB_BookTitle": $scope.LMB_BookTitle,
                        "LMB_Price": $scope.LMB_Price,
                        "Issue_Date": issuedate,
                        "Due_Date": duedate,
                        "Return_Date": returndate,
                        "Book_Trans_Status": $scope.Book_Trans_Status,
                        "AMST_Id": sid,
                        "booktype": $scope.booktype,
                        "issuertype": $scope.issuertype1,
                        "maxitem": $scope.maxitem,
                        "bookcat_type": $scope.bookornonbook,

                    }
                }
                else if ($scope.issuertype1 == 'stf') {
                    var data = {
                        "Book_Trans_Id": $scope.Book_Trans_Id,
                        "LMBANO_Id": a_id,
                        "LMB_Id": b_id,
                        "LMB_BookTitle": $scope.LMB_BookTitle,
                        "LMB_Price": $scope.LMB_Price,
                        "Issue_Date": issuedate,
                        "Due_Date": duedate,
                        "Return_Date": returndate,
                        "Book_Trans_Status": $scope.Book_Trans_Status,
                        "HRME_Id": $scope.HRME_Id,
                        "booktype": $scope.booktype,
                        "issuertype": $scope.issuertype1,
                        "maxitem": $scope.maxitem,
                        "bookcat_type": $scope.bookornonbook,

                    }
                }

                else if ($scope.issuertype1 == 'dep') {
                    var data = {
                        "Book_Trans_Id": $scope.Book_Trans_Id,
                        "LMBANO_Id": a_id,
                        "LMB_Id": b_id,
                        "LMB_BookTitle": $scope.LMB_BookTitle,
                        "LMB_Price": $scope.LMB_Price,
                        "Issue_Date": issuedate,
                        "Due_Date": duedate,
                        "Return_Date": returndate,
                        "Book_Trans_Status": $scope.Book_Trans_Status,
                        "HRMD_Id": $scope.HRMD_Id,
                        "booktype": $scope.booktype,
                        "issuertype": $scope.issuertype1,
                        "maxitem": $scope.maxitem,
                        "bookcat_type": $scope.bookornonbook,

                    }
                }
                else if ($scope.issuertype1 == 'gst') {
                    var data = {
                        "Book_Trans_Id": $scope.Book_Trans_Id,
                        "LMBANO_Id": a_id,
                        "LMB_Id": b_id,
                        "LMB_BookTitle": $scope.LMB_BookTitle,
                        "LMB_Price": $scope.LMB_Price,
                        "Issue_Date": issuedate,
                        "Due_Date": duedate,
                        "Return_Date": returndate,
                        "Book_Trans_Status": $scope.Book_Trans_Status,
                        "booktype": $scope.booktype,
                        "issuertype": $scope.issuertype1,
                        "LBTR_GuestName": $scope.LBTR_GuestName,
                        "LBTR_GuestContactNo": $scope.LBTR_GuestContactNo,
                        "LBTR_GuestEmailId": $scope.LBTR_GuestEmailId,
                        "maxitem": $scope.maxitem,
                        "bookcat_type": $scope.bookornonbook,

                    }
                }



                apiService.create("NonBookTransaction/Savedata", data)
                    .then(function (promise) {
                        if (promise.returnval != null && promise.duplicate != null) {

                            if (promise.maxmsg != "exceed") {
                                if (promise.duplicate == false) {
                                    if (promise.returnval == true) {
                                        if ($scope.Book_Trans_Id > 0) {
                                            swal('Book Already Return');
                                        }
                                        else {
                                            swal('Book Issued Successfully!!!');
                                        }

                                    }
                                    else {
                                        if (promise.returnval == false) {
                                            if ($scope.Book_Trans_Id > 0) {
                                                swal('Book Not Return Successfully!!!');
                                            }
                                            else {
                                                swal('Book Not Issued ');
                                            }
                                        }
                                    }
                                }
                                else {
                                    swal("Book already Assign");
                                }
                                // $state.reload();
                                $scope.Loaddata();
                                $scope.saveclear();
                                $scope.clearselecteddetails();
                            }
                            else {
                                swal("Exceeds the Number of book Issue.");
                            }

                        }
                        else {
                            swal("Kindly Contact Administrator!!!");
                        }

                    })

            }
            else {
                $scope.submitted = true;
            }
        };

        //=====================End-Issue Book==================//

        $scope.saveclear = function () {
            $scope.Issue_Date = '';
            $scope.Return_Date = '';
            $scope.Due_Date = '';
            $scope.AMST_Id = '';
            $scope.LMBANO_Id = '';
            $scope.HRMD_Id = '';
            $scope.HRMDES_Id = '';
            $scope.HRME_Id = '';
            $scope.BokStud = false;
            $scope.issuedate = false;
            $scope.returndate = false;
            $scope.LBTR_GuestName = '';
            $scope.LBTR_GuestContactNo = '';
            $scope.LBTR_GuestEmailId = '';
            $scope.Book_Trans_Id = 0;
            $scope.barcode1 = '';
            $scope.barcode = '';
        }

        $scope.get_staff1 = function () {
            $scope.stafftlist = [];
            $scope.issuedate = false;
            $scope.HRME_Id = '';
            var data = {
                //"LMBANO_Id": a_id,
                //"HRME_Id": $scope.HRME_Id,
                //"LMB_Id": b_id,
                //"booktype": $scope.booktype,
                //"issuertype": $scope.issuertype1,
                //"bookcat_type": $scope.bookornonbook,
                "HRMDES_Id": $scope.HRMDES_Id,
                "HRMD_Id": $scope.HRMD_Id,
            }

            apiService.create("NonBookTransaction/get_staff1", data).then(function (promise) {

                $scope.stafftlist = promise.stafftlist;

            })


        }




        //=====================Get designation ==================//
        $scope.getdepchange = function () {
            debugger;
            $scope.stafftlist = [];
            $scope.issuedate = false;
            $scope.filldesignation = [];
            $scope.HRME_Id = '';
            $scope.HRMDES_Id = '';
            var data = {

                "HRMD_Id": $scope.HRMD_Id,
            }

            apiService.create("NonBookTransaction/getdepchange", data).then(function (promise) {
                $scope.filldesignation = promise.filldesignation;
                //  $scope.stafftlist = promise.stafftlist;

            })


        }



        //=====================End ==================//
        //=====================Get Student Record ==================//
        $scope.get_Studentdetails = function () {
            debugger;
            $scope.circularparamdetails = [];
            var b_id = $scope.LMBANO_Id.lmB_Id;
            var a_id = $scope.LMBANO_Id.lmbanO_Id;
            var data = {}
            if ($scope.issuertype1 == 'std') {
                var sid = $scope.AMST_Id.amsT_Id

                data = {
                    "LMBANO_Id": a_id,
                    "AMST_Id": sid,
                    "LMB_Id": b_id,
                    "booktype": $scope.booktype,
                    "issuertype": $scope.issuertype1,
                    "bookcat_type": $scope.bookornonbook,
                }
            } else if ($scope.issuertype1 == 'stf') {


                data = {
                    "LMBANO_Id": a_id,
                    "HRME_Id": $scope.HRME_Id,
                    "LMB_Id": b_id,
                    "booktype": $scope.booktype,
                    "issuertype": $scope.issuertype1,
                    "bookcat_type": $scope.bookornonbook,
                    //"HRMDES_Id": $scope.HRMDES_Id,
                    // "HRMD_Id": $scope.HRMD_Id,
                }
            }
            else if ($scope.issuertype1 == 'dep') {
                data = {
                    "LMBANO_Id": a_id,
                    "HRMD_Id": $scope.HRMD_Id,
                    "LMB_Id": b_id,
                    "booktype": $scope.booktype,
                    "issuertype": $scope.issuertype1,
                    "bookcat_type": $scope.bookornonbook,
                }
            }


            apiService.create("NonBookTransaction/studentdetails", data).then(function (promise) {
                if ($scope.issuertype1 == 'std') {
                    $scope.studentdata = promise.studentdata;
                    if ($scope.studentdata.length != 0 && $scope.studentdata != null) {

                        $scope.AMST_FirstName = promise.studentdata[0].amsT_FirstName;
                        $scope.AMST_RegistrationNo = promise.studentdata[0].amsT_RegistrationNo;
                        $scope.AMST_AdmNo = promise.studentdata[0].amsT_AdmNo;
                        $scope.AMAY_RollNo = promise.studentdata[0].amaY_RollNo;
                        $scope.ASMCL_ClassName = promise.studentdata[0].asmcL_ClassName;
                        $scope.ASMC_SectionName = promise.studentdata[0].asmC_SectionName;
                        $scope.AMST_Photoname = promise.studentdata[0].amsT_Photoname;
                        $scope.amsT_FatherMobleNo = promise.studentdata[0].amsT_FatherMobleNo;
                        $scope.asmS_Id = promise.studentdata[0].asmS_Id;
                        $scope.ASMCL_Id = promise.studentdata[0].asmcL_Id;
                        //  $scope.ASMAY_Id = promise.studentdata[0].asmaY_Id;

                        $scope.circularparamdetails = promise.circularparamdetails;
                        if (promise.circularparamdetails.length > 0) {
                            $scope.maxdays = promise.circularparamdetails[0].max_Issue_Days - 1;
                            $scope.maxrenew = promise.circularparamdetails[0].max_No_Renewals;
                            $scope.maxitem = promise.circularparamdetails[0].max_Issue_Items;
                            $scope.issuedate = true;
                        }
                        else {
                            swal("Circular parameters are not set!!");
                        }

                    }
                    else {
                        /// $state.reload();
                        // swal("Data Not Available!!");
                    }
                }
                else if ($scope.issuertype1 == 'stf') {
                    $scope.selctstaffdata = promise.selctstaffdata;
                    if ($scope.selctstaffdata.length != 0 && $scope.selctstaffdata != null) {

                        $scope.HRME_EmployeeCode = promise.selctstaffdata[0].hrmE_EmployeeCode;
                        $scope.HRME_EmployeeFirstName = promise.selctstaffdata[0].hrmE_EmployeeFirstName;
                        $scope.HRMD_DepartmentName = promise.selctstaffdata[0].hrmD_DepartmentName;
                        $scope.HRME_MobileNo = promise.selctstaffdata[0].hrmE_MobileNo;
                        $scope.HRMDES_DesignationName = promise.selctstaffdata[0].hrmdeS_DesignationName;
                        $scope.HRME_Photo = promise.selctstaffdata[0].hrmE_Photo;
                        $scope.circularparamdetails = promise.circularparamdetails;
                        if (promise.circularparamdetails.length > 0) {
                            $scope.maxdays = promise.circularparamdetails[0].max_Issue_Days - 1;
                            $scope.maxrenew = promise.circularparamdetails[0].max_No_Renewals;
                            $scope.maxitem = promise.circularparamdetails[0].max_Issue_Items;
                            $scope.issuedate = true;
                        }
                        else {
                            swal("Circular parameters are not set!!");
                        }
                    }
                    else {
                        //swal("Data Not Available!!");
                    }
                }
                else if ($scope.issuertype1 == 'dep') {
                    // $scope.selctstaffdata = promise.selctstaffdata;
                    if ($scope.HRMD_Id != '' && $scope.HRMD_Id != undefined) {
                        if (promise.circularparamdetails.length > 0 && ($scope.HRMD_Id != '' && $scope.HRMD_Id != undefined)) {
                            $scope.maxdays = promise.circularparamdetails[0].max_Issue_Days - 1;
                            $scope.maxrenew = promise.circularparamdetails[0].max_No_Renewals;
                            $scope.maxitem = promise.circularparamdetails[0].max_Issue_Items;
                            $scope.issuedate = true;
                        }
                        else {
                            swal("Circular parameters are not set!!");
                        }
                    }

                    //if ($scope.selctstaffdata.length != 0 && $scope.selctstaffdata != null) {

                    //    $scope.HRME_EmployeeCode = promise.selctstaffdata[0].hrmE_EmployeeCode;
                    //    $scope.HRME_EmployeeFirstName = promise.selctstaffdata[0].hrmE_EmployeeFirstName;
                    //    $scope.HRMD_DepartmentName = promise.selctstaffdata[0].hrmD_DepartmentName;
                    //    $scope.HRME_MobileNo = promise.selctstaffdata[0].hrmE_MobileNo;
                    //    $scope.HRMDES_DesignationName = promise.selctstaffdata[0].hrmdeS_DesignationName;
                    //    $scope.HRME_Photo = promise.selctstaffdata[0].hrmE_Photo;
                    //    $scope.circularparamdetails = promise.circularparamdetails;
                    //    if (promise.circularparamdetails.length > 0) {
                    //        $scope.maxdays = promise.circularparamdetails[0].max_Issue_Days - 1;
                    //        $scope.maxrenew = promise.circularparamdetails[0].max_No_Renewals;
                    //        $scope.issuedate = true;
                    //    }
                    //}
                    //else {
                    //   // swal("Data Not Available!!");
                    //}
                }


                //$scope.alldata = promise.alldata;

                //if ($scope.alldata.length != 0 && $scope.alldata != null) {

                // $scope.Issue_Date = new Date();
                // $scope.Due_Date = new Date(promise.alldata[0].due_Date);
                // $scope.Return_Date = new Date(promise.alldata[0].return_Date);
                //    $scope.Book_Trans_Status = promise.alldata[0].book_Trans_Status;

                //$scope.LMB_EntryDate = new Date(promise.alldata[0].LMB_EntryDate);
                //$scope.LMB_BookTitle = promise.alldata[0].LMB_BookTitle;
                //$scope.LMB_VolNo = promise.alldata[0].LMB_VolNo;
                //$scope.LMB_Price = promise.alldata[0].LMB_Price;
                //$scope.LMB_ClassNo = promise.alldata[0].LMB_ClassNo;
                //$scope.AMST_FirstName = promise.alldata[0].amsT_FirstName;
                //$scope.AMST_RegistrationNo = promise.alldata[0].amsT_RegistrationNo;
                //$scope.AMST_AdmNo = promise.alldata[0].amsT_AdmNo;
                //$scope.AMAY_RollNo = promise.alldata[0].amaY_RollNo;
                //$scope.ASMCL_ClassName = promise.alldata[0].asmcL_ClassName;
                //$scope.ASMC_SectionName = promise.alldata[0].asmC_SectionName;

                //}
                //else {
                //    $state.reload();
                //    swal("Data Not Available!!");
                //}


            })
        }
        //=====================End Student Record ==================//
        //=====================Start Book name Search ==================//


        $scope.barcodechange = function (dd) {

            if (dd.length >= '1') {
                var data = {
                    "searchfilter": dd,
                    "booktype": $scope.booktype,
                    "issuertype": $scope.issuertype1,
                    "bookcat_type": $scope.bookornonbook,
                    "LMAL_Id": $scope.LMAL_Id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("NonBookTransaction/searchfilterbarcode", data).
                    then(function (promise) {
                        $scope.booktitle = promise.booktitle;
                        if ($scope.booktitle.length > 0) {
                            $scope.LMBANO_Id = promise.booktitle[0];
                            $scope.get_bookdetails();

                            $scope.barcode = '';
                        }
                        else {
                            swal('Book does not exist for issue or book does not  belongs to the selected parameter');
                            $scope.barcode = '';
                        }

                    })
            }

        }

        $scope.barcodechange1 = function (dd) {


            var data = {
                "searchfilter": dd,
                "booktype": $scope.booktype,
                "issuertype": $scope.issuertype1,
                "bookcat_type": $scope.bookornonbook,
                "LMAL_Id": $scope.LMAL_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("NonBookTransaction/searchfilterbarcode1", data).
                then(function (promise) {
                    debugger;
                    $scope.getalldata = promise.getalldata;


                })


        }
        $scope.barcode1 = '';
        $scope.clearbarcode = function () {

            $scope.barcode1 = '';
            var data = {
                "searchfilter": $scope.barcode1,
                "booktype": $scope.booktype,
                "issuertype": $scope.issuertype1,
                "bookcat_type": $scope.bookornonbook,
                "LMAL_Id": $scope.LMAL_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("NonBookTransaction/searchfilterbarcode1", data).
                then(function (promise) {
                    debugger;
                    $scope.getalldata = promise.getalldata;


                })


        }


        $scope.searchfilter = function (objj) {

            if (objj.search.length >= '1') {

                var data = {
                    "searchfilter": objj.search,
                    "booktype": $scope.booktype,
                    "issuertype": $scope.issuertype1,
                    "bookcat_type": $scope.bookornonbook,
                    "LMAL_Id": $scope.LMAL_Id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("NonBookTransaction/searchfilter", data).
                    then(function (promise) {

                        $scope.booktitle = promise.booktitle;

                        angular.forEach($scope.booktitle, function (objectt) {
                            if (objectt.amsT_FirstName.length > 0) {
                                var string = objectt.amsT_FirstName;
                                objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                            }
                        })
                    })
            }

        }

        //=====================Get Book Record ==================//
        $scope.maxrenew = 0;
        $scope.maxitem = 0;
        $scope.get_bookdetails = function () {
            debugger;
            var studid = $scope.LMBANO_Id.lmB_Id;
            var ascnid = $scope.LMBANO_Id.lmbanO_Id;
            //  $scope.LMB_Id = studid;
            var data = {
                "LMBANO_Id": ascnid,
                "LMB_Id": studid,
                "booktype": $scope.booktype,
                "issuertype": $scope.issuertype1,
            }
            apiService.create("NonBookTransaction/get_bookdetails", data).then(function (promise) {

                $scope.bookdetails = promise.bookdetails;

                if ($scope.bookdetails.length != 0 && $scope.bookdetails != null) {

                    $scope.LMB_EntryDate = new Date(promise.bookdetails[0].lmB_EntryDate);
                    $scope.LMB_BookTitle = promise.bookdetails[0].lmB_BookTitle;
                    $scope.LMB_VolNo = promise.bookdetails[0].lmB_VolNo;
                    $scope.LMB_Price = promise.bookdetails[0].lmB_Price;
                    $scope.LMB_ClassNo = promise.bookdetails[0].lmB_ClassNo;
                    $scope.authorname = promise.bookdetails[0].authorname;
                    $scope.callno = promise.bookdetails[0].LMB_CallNo;
                    $scope.cater = promise.bookdetails[0].LMC_CategoryName;

                    if ($scope.issuertype1 == 'gst') {

                        if (promise.circularparamdetails.length > 0) {
                            $scope.maxdays = promise.circularparamdetails[0].max_Issue_Days - 1;
                            $scope.maxrenew = promise.circularparamdetails[0].max_No_Renewals;
                            $scope.maxitem = promise.circularparamdetails[0].max_Issue_Items;
                            $scope.issuedate = true;
                        }
                        else {
                            $scope.issuedate = false;
                            swal("Circular parameters are not set!!");
                        }

                    }

                }
                else {
                    $state.reload();
                    swal("Data Not Available!!");
                }

            })
        }
        //====================End Book Record ==================//
        $scope.datalist = false;




        //==========Edit Record For Return & Renewal-Book==================//
        $scope.EditData = function (user) {
            debugger;
            var data = {}

            if ($scope.issuertype1 == 'std') {
                data = {
                    "Book_Trans_Id": user.book_Trans_Id,
                    // "LMB_Id":$scope.LMB_Id,
                    "ASMCL_Id": user.asmcL_Id,
                    "AMST_Id": user.amsT_Id,
                    "issuertype": $scope.issuertype1,
                    "booktype": $scope.booktype,
                    "bookcat_type": $scope.bookornonbook,

                }
            } else if ($scope.issuertype1 == 'stf') {
                data = {
                    "Book_Trans_Id": user.book_Trans_Id,
                    // "LMB_Id":$scope.LMB_Id,
                    "HRMGT_Id": user.hrmgT_Id,
                    "HRME_Id": user.hrmE_Id,
                    "issuertype": $scope.issuertype1,
                    "booktype": $scope.booktype,
                    "bookcat_type": $scope.bookornonbook,
                }
            }
            else if ($scope.issuertype1 == 'dep') {
                data = {
                    "Book_Trans_Id": user.book_Trans_Id,
                    // "LMB_Id":$scope.LMB_Id,
                    // "HRMGT_Id": user.hrmgT_Id,
                    "HRMD_Id": user.hrmD_Id,
                    "issuertype": $scope.issuertype1,
                    "booktype": $scope.booktype,
                    "bookcat_type": $scope.bookornonbook,
                }
            }
            else if ($scope.issuertype1 == 'gst') {
                data = {
                    "Book_Trans_Id": user.book_Trans_Id,
                    // "LMB_Id":$scope.LMB_Id,
                    // "HRMGT_Id": user.hrmgT_Id,
                    "issuertype": $scope.issuertype1,
                    "booktype": $scope.booktype,
                    "bookcat_type": $scope.bookornonbook,
                }
            }
            debugger;

            apiService.create("NonBookTransaction/Editdata", data)
                .then(function (promise) {
                    if ($scope.issuertype1 == 'std') {
                        $scope.booktitle = promise.booktitle;
                        $scope.editlist = promise.editlist;
                        $scope.Book_Trans_Id = promise.editlist[0].book_Trans_Id;
                        // $scope.LMB_Id = promise.editlist[0].lmB_Id;
                        // $scope.LMB_Id = promise.editlist[0];
                        //$scope.LMB_Id.lmB_Id = promise.editlist[0].lmB_Id;
                        $scope.LMBANO_Id = promise.editlist[0];

                        //  $scope.AMST_Id = promise.editlist[0].amsT_Id;
                        $scope.AMST_Id = promise.editlist[0];
                        if ($scope.LMBANO_Id.length != 0 && $scope.AMST_Id.length != 0) {
                            $scope.get_bookdetails();
                            $scope.get_Studentdetails();
                        }
                        $scope.Issue_Date = new Date(promise.editlist[0].issue_Date);
                        $scope.Due_Date = new Date(promise.editlist[0].due_Date);
                        $scope.Return_Date = new Date(promise.editlist[0].return_Date);
                        $scope.Book_Trans_Status = promise.editlist[0].book_Trans_Status;
                        debugger;
                        $scope.maxrenew = promise.circularparamdetails[0].max_No_Renewals;
                        $scope.maxitem = promise.circularparamdetails[0].max_Issue_Items;

                        $scope.BokStud = true;
                        $scope.issuebutton = false;
                        $scope.duedate = true;
                        $scope.returndate = true;
                        $scope.showflag = true;
                    }
                    else if ($scope.issuertype1 == 'stf') {
                        $scope.booktitle = promise.booktitle;
                        $scope.editlist = promise.editlist;
                        $scope.Book_Trans_Id = promise.editlist[0].book_Trans_Id;
                        // $scope.LMB_Id = promise.editlist[0].lmB_Id;
                        // $scope.LMB_Id = promise.editlist[0];
                        $scope.LMBANO_Id = promise.editlist[0];

                        $scope.HRMD_Id = promise.editlist[0].hrmD_Id;
                        $scope.HRMDES_Id = promise.editlist[0].hrmdeS_Id;
                        $scope.HRME_Id = promise.editlist[0].hrmE_Id;
                        if ($scope.LMBANO_Id.length != 0 && $scope.HRME_Id.length != 0) {
                            $scope.get_bookdetails();
                            $scope.get_Studentdetails();
                        }
                        $scope.Issue_Date = new Date(promise.editlist[0].issue_Date);
                        $scope.Due_Date = new Date(promise.editlist[0].due_Date);
                        $scope.Return_Date = new Date(promise.editlist[0].return_Date);
                        $scope.Book_Trans_Status = promise.editlist[0].book_Trans_Status;
                        debugger;
                        $scope.maxrenew = promise.circularparamdetails[0].max_No_Renewals;
                        $scope.maxitem = promise.circularparamdetails[0].max_Issue_Items;

                        $scope.BokStud = true;
                        $scope.issuebutton = false;
                        $scope.duedate = true;
                        $scope.returndate = true;
                        $scope.showflag = true;
                    }
                    else if ($scope.issuertype1 == 'dep') {
                        $scope.booktitle = promise.booktitle;
                        $scope.editlist = promise.editlist;
                        $scope.Book_Trans_Id = promise.editlist[0].book_Trans_Id;
                        //  $scope.LMB_Id = promise.editlist[0].lmB_Id;
                        // $scope.LMB_Id = promise.editlist[0];
                        $scope.LMBANO_Id = promise.editlist[0];

                        $scope.HRMD_Id = promise.editlist[0].hrmD_Id;
                        if ($scope.LMBANO_Id.length != 0 && $scope.HRMD_Id.length != 0) {
                            $scope.get_bookdetails();
                            $scope.get_Studentdetails();
                        }
                        $scope.Issue_Date = new Date(promise.editlist[0].issue_Date);
                        $scope.Due_Date = new Date(promise.editlist[0].due_Date);
                        $scope.Return_Date = new Date(promise.editlist[0].return_Date);
                        $scope.Book_Trans_Status = promise.editlist[0].book_Trans_Status;
                        debugger;
                        $scope.maxrenew = promise.circularparamdetails[0].max_No_Renewals;
                        $scope.maxitem = promise.circularparamdetails[0].max_Issue_Items;

                        $scope.BokStud = true;
                        $scope.issuebutton = false;
                        $scope.duedate = true;
                        $scope.returndate = true;
                        $scope.showflag = true;
                    }

                    else if ($scope.issuertype1 == 'gst') {
                        $scope.booktitle = promise.booktitle;
                        $scope.editlist = promise.editlist;
                        $scope.Book_Trans_Id = promise.editlist[0].book_Trans_Id;
                        // $scope.LMB_Id = promise.editlist[0].lmB_Id;
                        //$scope.LMB_Id = promise.editlist[0];
                        $scope.LMBANO_Id = promise.editlist[0];
                        $scope.LBTR_GuestName = promise.editlist[0].lbtR_GuestName;
                        $scope.LBTR_GuestContactNo = promise.editlist[0].lbtR_GuestContactNo;
                        $scope.LBTR_GuestEmailId = promise.editlist[0].lbtR_GuestEmailId;


                        if ($scope.LMBANO_Id.length != 0) {
                            $scope.get_bookdetails();
                            // $scope.get_Studentdetails();
                        }
                        $scope.Issue_Date = new Date(promise.editlist[0].issue_Date);
                        $scope.Due_Date = new Date(promise.editlist[0].due_Date);
                        $scope.Return_Date = new Date(promise.editlist[0].return_Date);
                        $scope.Book_Trans_Status = promise.editlist[0].book_Trans_Status;
                        debugger;
                        $scope.maxrenew = promise.circularparamdetails[0].max_No_Renewals;
                        $scope.maxitem = promise.circularparamdetails[0].max_Issue_Items;

                        $scope.issuedate = true;
                        $scope.BokStud = true;
                        $scope.issuebutton = false;
                        $scope.duedate = true;
                        $scope.returndate = true;
                        $scope.showflag = true;
                    }

                })

        }
        //==================End Edit Record For Return & Renewal-Book==================//


        //==================Renewal-Book==================//
        $scope.renewaldata = function () {
            ////alert($scope.maxrenew);
            debugger;
            var issuedate = $scope.Issue_Date == null ? "" : $filter('date')($scope.Issue_Date, "yyyy-MM-dd");
            var returndate = $scope.Return_Date == null ? "" : $filter('date')($scope.Return_Date, "yyyy-MM-dd");
            var duedate = $scope.Due_Date == null ? "" : $filter('date')($scope.Due_Date, "yyyy-MM-dd");

            var date2 = new Date();

            var date3 = date2 == null ? "" : $filter('date')(date2, "yyyy-MM-dd");

            if (issuedate != date3) {
                debugger;
                swal('Please select Issue date for Renewal!!!!');
            }
            else {
                if ($scope.myForm.$valid) {
                    var issuedate = $scope.Issue_Date == null ? "" : $filter('date')($scope.Issue_Date, "yyyy-MM-dd");
                    var returndate = $scope.Return_Date == null ? "" : $filter('date')($scope.Return_Date, "yyyy-MM-dd");
                    var duedate = $scope.Due_Date == null ? "" : $filter('date')($scope.Due_Date, "yyyy-MM-dd");

                    var data = {
                        "Book_Trans_Id": $scope.Book_Trans_Id,
                        "LMBANO_Id": $scope.LMBANO_Id.lmbanO_Id,
                        // "AMST_Id": $scope.AMST_Id,
                        "LMB_Id": $scope.LMBANO_Id.lmB_Id,
                        "Issue_Date": issuedate,
                        "Due_Date": duedate,
                        "Return_Date": returndate,
                        "Max_No_Renewals": $scope.maxrenew,
                        "issuertype": $scope.issuertype1,
                        "bookcat_type": $scope.bookornonbook,
                    }
                    apiService.create("NonBookTransaction/renewaldata", data)
                        .then(function (promise) {
                            if (promise.returnval != null && promise.duplicate != null) {
                                if (promise.duplicate == false && promise.renew == false) {
                                    if ($scope.Book_Trans_Id > 0) {
                                        swal('Book Renewed Successfully!!');
                                    }
                                    else {
                                        swal('First Issue Book For the Renewal');
                                    }
                                }
                                else if (promise.renew == true) {
                                    swal("Exceeds the Renewal Limits!", 'Hello Return This Non-Book!!!');
                                }
                                else {
                                    swal("Book already Assign");
                                }
                                // $state.reload();
                                $scope.Loaddata();
                                $scope.saveclear();
                                $scope.issuebutton = true;
                                $scope.showflag = false;
                                $scope.clearselecteddetails();
                            }
                            else {
                                swal("Kindly Contact Administrator!!!");
                            }
                        })
                }
                else {
                    $scope.submitted = true;
                }
            }






        }
        //=====================End-Renewal-Book==================//

        $scope.clearselecteddetails = function () {

            $scope.HRME_EmployeeCode = '';
            $scope.HRME_EmployeeFirstName = '';
            $scope.HRMD_DepartmentName = '';
            $scope.HRME_MobileNo = '';
            $scope.HRMDES_DesignationName = '';
            $scope.HRME_Photo = '';
            $scope.maxdays = 0;
            $scope.maxrenew = 0;
            $scope.maxitem = 0;

            $scope.AMST_FirstName = '';
            $scope.AMST_RegistrationNo = '';
            $scope.AMST_AdmNo = '';
            $scope.AMAY_RollNo = '';
            $scope.ASMCL_ClassName = '';
            $scope.ASMC_SectionName = '';
            $scope.AMST_Photoname = '';
            $scope.circularparamdetails = [];
            $scope.LMB_EntryDate = '';
            $scope.LMB_BookTitle = '';
            $scope.LMB_VolNo = '';
            $scope.LMB_Price = '';
            $scope.LMB_ClassNo = '';
            $scope.authorname = '';
            $scope.callno = '';
            $scope.cater = '';

        }


        $scope.issuebutton = true;
        //======================Rerturn Book==================//
        $scope.returndata = function () {
            debugger;
            var issuedate = $scope.Issue_Date == null ? "" : $filter('date')($scope.Issue_Date, "yyyy-MM-dd");
            var returndate = $scope.Return_Date == null ? "" : $filter('date')($scope.Return_Date, "yyyy-MM-dd");
            var duedate = $scope.Due_Date == null ? "" : $filter('date')($scope.Due_Date, "yyyy-MM-dd");



            if (returndate != null && returndate != "" && returndate != undefined) {
                var data = {
                    "Book_Trans_Id": $scope.Book_Trans_Id,
                    "LMBANO_Id": $scope.LMBANO_Id.lmbanO_Id,
                    // "AMST_Id": $scope.AMST_Id,
                    "LMB_Id": $scope.LMBANO_Id.lmB_Id,
                    // "Issue_Date": issuedate,
                    "Due_Date": duedate,
                    "Return_Date": returndate,
                    "booktype": $scope.booktype,
                    "issuertype": $scope.issuertype1,
                    "bookcat_type": $scope.bookornonbook,
                    // "Book_Trans_Status": $scope.Book_Trans_Status,
                }
                if ($scope.Book_Trans_Status != 'Return') {
                    apiService.create("NonBookTransaction/returndata", data)
                        .then(function (promise) {
                            if (promise.returnval != null) {

                                if (promise.returnval == true) {
                                    swal('Book Returned Successfully!!');
                                }
                                else {
                                    swal('First Issue Book For the Return');
                                }
                                // $state.reload();
                                $scope.Loaddata();
                                $scope.saveclear();
                                $scope.issuebutton = true;
                                $scope.showflag = false;
                                $scope.clearselecteddetails();
                            }
                            else {
                                swal("Kindly Contact Administrator!!!");
                            }
                        })
                }
                else {
                    swal('Book Already Return');
                }
            }
            else {
                $scope.submitted = true;
            }
        }

        //=====================End-Rerturn-Book==================//



        // $scope.minDatemf = new Date();


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear-Field
        $scope.cancel = function () {
            $state.reload();
        }
        //-----------------End



        $scope.onSelectclass = function (classId) {
            if ($scope.ASMAY_Id > 0 && $scope.ASMCL_Id > 0 && $scope.asmS_Id > 0) {
                $scope.GetStudentDetails1();
            }
        }
        $scope.onSelectyear = function () {
            if ($scope.ASMAY_Id > 0 && $scope.ASMCL_Id > 0 && $scope.asmS_Id > 0) {
                $scope.GetStudentDetails1();
            }
        }

        $scope.GetStudentDetails1 = function () {
            debugger;
            $scope.clearselecteddetails();
            $scope.saveclear();
            // var j = $scope.amst_Id;
            //  $scope.Amst_Id = [];
            $scope.studlist = [];
            //  $scope.studentlst.amst_Id = '';

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.asmS_Id
            }
            apiService.create("NonBookTransaction/GetStudentDetails1/", data).
                then(function (promise) {

                    if (promise.studentlist != null || promise.studentlist != undefined) {
                       // $scope.count = promise.studentCount;
                        $scope.studlist = promise.studentlist;
                    }
                    //if (promise.studentCount > 0) {
                    //    $scope.count = promise.studentCount;
                    //    $scope.studlist = promise.studentlist;
                    //} 
                    else {
                        swal("No records found for selected academicYear,class and section");
                        $scope.studlist = [];
                        $scope.count = 0;
                    }
                });
        }

    }
})();

