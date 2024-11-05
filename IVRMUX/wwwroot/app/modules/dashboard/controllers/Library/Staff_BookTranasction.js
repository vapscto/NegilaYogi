(function () {
    'use strict';
    angular
        .module('app')
        .controller('Staff_BookTranasctionController', Staff_BookTranasctionController)

    Staff_BookTranasctionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function Staff_BookTranasctionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

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



        //-------------Page Load
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            debugger;
            var pageid = 2;
            apiService.getURI("Staff_BookTranasction/getdetails", pageid).then(function (promise) {

                $scope.stafftlist = promise.stafftlist;

                $scope.booktitle = promise.booktitle;

                $scope.getalldata = promise.getalldata;

                $scope.configdata = promise.configdata;

            })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
        //----------------------END Load




        //======================Issue Book==================//
        $scope.Savedata = function () {

            debugger;
            if ($scope.myForm.$valid) {
                var issuedate = $scope.Issue_Date == null ? "" : $filter('date')($scope.Issue_Date, "yyyy-MM-dd");
                var returndate = $scope.Return_Date == null ? "" : $filter('date')($scope.Return_Date, "yyyy-MM-dd");
                var duedate = $scope.Due_Date == null ? "" : $filter('date')($scope.Due_Date, "yyyy-MM-dd");

                var data = {
                    "Book_Trans_Id": $scope.Book_Trans_Id,
                    "LMBANO_Id": $scope.LMBANO_Id,
                    "Issue_Date": issuedate,
                    "Due_Date": duedate,
                    "Return_Date": returndate,
                    "Book_Trans_Status": $scope.Book_Trans_Status,
                    "HRME_Id": $scope.HRME_Id,

                }
                apiService.create("Staff_BookTranasction/Savedata", data)
                    .then(function (promise) {
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.Book_Trans_Id > 0) {
                                        swal('Book Already Return');
                                    }
                                    else {
                                        swal('Book Issue Successfully!!!');
                                    }

                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.Book_Trans_Id > 0) {
                                            swal('Book Not Return Successfully!!!');
                                        }
                                        else {
                                            swal('Book Not Issue Successfully!!!');
                                        }
                                    }
                                }
                            }
                            else {
                                swal("Book already Assign");
                            }
                            $state.reload();
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





        //=====================Get Staff Record ==================//
        $scope.get_Staffdetails = function () {
            debugger;

            var data = {
                "HRME_Id": $scope.HRME_Id
            }
            apiService.create("Staff_BookTranasction/get_Staffdetails", data).then(function (promise) {

                $scope.selctstaffdata = promise.selctstaffdata;
                if ($scope.selctstaffdata.length != 0 && $scope.selctstaffdata != null) {

                    $scope.HRME_EmployeeCode = promise.selctstaffdata[0].hrmE_EmployeeCode;
                    $scope.HRME_EmployeeFirstName = promise.selctstaffdata[0].hrmE_EmployeeFirstName;
                    $scope.HRMD_DepartmentName = promise.selctstaffdata[0].hrmD_DepartmentName;
                    $scope.HRME_MobileNo = promise.selctstaffdata[0].hrmE_MobileNo;
                    $scope.HRMDES_DesignationName = promise.selctstaffdata[0].hrmdeS_DesignationName;
                    $scope.HRME_Photo = promise.selctstaffdata[0].hrmE_Photo;

                }
                else {
                    swal("Data Not Available!!");
                }

            })
        }
        //=====================End Staff Record ==================//




        //=====================Get Book Record ==================//
        $scope.maxrenew = 0;
        $scope.get_bookdetails = function () {
            debugger;

            var data = {
                "LMB_Id": $scope.LMB_Id,
                "LMBANO_Id": $scope.LMBANO_Id
            }
            apiService.create("Staff_BookTranasction/get_bookdetails", data).then(function (promise) {

                $scope.bookdetails = promise.bookdetails;

                if ($scope.bookdetails.length != 0 && $scope.bookdetails != null) {

                    $scope.LMB_EntryDate = new Date(promise.bookdetails[0].LMB_EntryDate);
                    $scope.LMB_BookTitle = promise.bookdetails[0].LMB_BookTitle;
                    $scope.LMB_VolNo = promise.bookdetails[0].LMB_VolNo;
                    $scope.LMB_Price = promise.bookdetails[0].LMB_Price;
                    $scope.LMB_ClassNo = promise.bookdetails[0].LMB_ClassNo;

                    $scope.maxdays = promise.bookdetails[0].max_Issue_Days - 1;
                    $scope.maxrenew = promise.bookdetails[0].max_No_Renewals;
                    $scope.issuedate = true;
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
            var data = {
                "Book_Trans_Id": user.book_Trans_Id,
               
            }
            apiService.create("Staff_BookTranasction/Editdata", data)
                .then(function (promise) {
                    $scope.booktitle = promise.booktitle;
                    $scope.editlist = promise.editlist;
                    $scope.Book_Trans_Id = promise.editlist[0].book_Trans_Id;
                    $scope.LMB_Id = promise.editlist[0].LMB_Id;
                    $scope.LMBANO_Id = promise.editlist[0].LMBANO_Id;

                    $scope.HRME_Id = promise.editlist[0].hrmE_Id;
                    if ($scope.LMBANO_Id.length != 0 && $scope.HRME_Id.length != 0) {
                        $scope.get_bookdetails();
                        $scope.get_Staffdetails();
                    }
                    $scope.Issue_Date = new Date(promise.editlist[0].issue_Date);
                    $scope.Due_Date = new Date(promise.editlist[0].due_Date);
                    $scope.Return_Date = new Date(promise.editlist[0].return_Date);
                    $scope.Book_Trans_Status = promise.editlist[0].book_Trans_Status;
                    debugger;
                    $scope.maxrenew = promise.booktitle[0].max_No_Renewals;

                    $scope.BokStud = true;
                    $scope.issuebutton = false;
                    $scope.duedate = true;
                    $scope.returndate = true;
                    $scope.showflag = true;
                })

        }
        //==================End Edit Record For Return & Renewal-Book==================//


        //==================Renewal-Book==================//
        $scope.renewaldata = function () {
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
                    var data = {
                        "Book_Trans_Id": $scope.Book_Trans_Id,
                        "LMBANO_Id": $scope.LMBANO_Id,
                        "HRME_Id": $scope.HRME_Id,
                        "LMB_Id": $scope.LMB_Id,
                        "Issue_Date": issuedate,
                        "Due_Date": duedate,
                        "Return_Date": returndate,
                        "Max_No_Renewals": $scope.maxrenew,
                    }
                    apiService.create("Staff_BookTranasction/renewaldata", data)
                        .then(function (promise) {
                            if (promise.returnval != null && promise.duplicate != null) {
                                if (promise.duplicate == false && promise.renew == false) {
                                    if ($scope.Book_Trans_Id > 0) {
                                        swal('Book Renewal/Issue Successfully!!');
                                    }
                                    else {
                                        swal('First Issue Book For the Renewal');
                                    }
                                }
                                else if (promise.renew == true) {
                                    swal("Exceeds the Renewal Limits!", 'Hello Return This Book!!!');
                                }
                                else {
                                    swal("Book already Assign");
                                }
                                $state.reload();
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
                        "LMBANO_Id": $scope.LMBANO_Id,
                        "HRME_Id": $scope.HRME_Id,
                        "LMB_Id": $scope.LMB_Id,
                        "Issue_Date": issuedate,
                        "Due_Date": duedate,
                        "Return_Date": returndate,
                        "Book_Trans_Status": $scope.Book_Trans_Status,
                }

                    if ($scope.Book_Trans_Status != 'Return') {
                        apiService.create("Staff_BookTranasction/returndata", data)
                            .then(function (promise) {
                                if (promise.returnval != null) {

                                    if ($scope.returnval != true) {
                                        swal('Book Return Successfully!!');
                                    }
                                    else {
                                        swal('First Issue Book For the Return');
                                    }
                                    $state.reload();
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




       


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear-Field
        $scope.cancel = function () {
            $state.reload();
        }
        //-----------------End


    }
})();

