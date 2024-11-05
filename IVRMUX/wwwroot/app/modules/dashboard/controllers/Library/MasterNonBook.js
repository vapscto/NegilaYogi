
(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterNonBookController', MasterNonBookController)

    MasterNonBookController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', '$window', 'Excel', '$timeout']
    function MasterNonBookController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter, $window, Excel, $timeout) {

        $scope.submitted = false;
        $scope.myTabIndex = 0;
        $scope.submitted2 = false;
        $scope.show_bookAccNo = false;
        $scope.show_LMBANO_No = true;
        $scope.edit = false;
        $scope.tab2 = true;
        $scope.tab3 = true;
        $scope.tab1 = false;
        $scope.amount = true;

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";

        $scope.previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
            $scope.tab2 = true;
            $scope.tab1 = false;
        }


        //parents form validation
        $scope.submitted3 = false;
        $scope.previous1 = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
            $scope.tab3 = true;
            $scope.tab2 = false;
            $scope.tab1 = false;
        }


        $scope.next = function () {
            //debugger;
            if ($scope.myForm.$valid) {
                $scope.tab2 = false;
                $scope.tab1 = true;
                //  $scope.maxDate1 = new Date($scope.LMB_EntryDate);
                $scope.myTabIndex = $scope.myTabIndex + 1;
            }
            else {
                $scope.submitted = true;
                $scope.tab2 = true;
                $scope.tab1 = false;
            }

        }

        $scope.submitted2 = false;
        $scope.next1 = function () {
            debugger;
            if ($scope.myForm2.$valid) {
                $scope.tab3 = false;
                $scope.tab1 = true;
                $scope.tab2 = true;
                $scope.myTabIndex = $scope.myTabIndex + 1;
            }
            else {
                $scope.submitted2 = true;
                $scope.tab2 = false;
                $scope.tab1 = true;
            }
        }



        //-----------load data.............
        $scope.Loaddata = function () {
            $scope.search = "";  
            debugger;
            var pageid = 2;
            $scope.master_author = 0;
            apiService.getURI("MasterNonBook/getdetails", pageid).then(function (promise) {
                $scope.sublist = promise.subjectlist;
                $scope.deptlist = promise.deptlist;
                $scope.racklist = promise.racklist;
                $scope.langlist = promise.langlist;

                $scope.vendorlist = promise.vendorlist;
                $scope.publisherlst = promise.publisherlst;

                $scope.categorylist = promise.categorylist;
                $scope.accessorieslist = promise.accessorieslist;
                $scope.librarylist = promise.librarylist;
                $scope.subscriptionist = promise.subscriptionist;
                $scope.periodicitylist = promise.periodicitylist;

                $scope.alldata = promise.alldata;
                $scope.library_id = promise.lmaL_Id;

                $scope.totalgrid = [];
                if ($scope.totalgrid.length == 0) {
                    $scope.totalgrid.push({
                        LMBANO_No: '',
                    });
                }

            })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }


        //----------Savedata..........
        $scope.savedata = function () {
            debugger;
            if ($scope.lmnbK_WithAccessoriesFlg == '1') {
                $scope.lmnbK_WithAccessoriesFlg = true;
            } else {
                $scope.lmnbK_WithAccessoriesFlg = false;
            }

            //if ($scope.master_author == '1') {
            //    $scope.LMBA_Id = $scope.lmbA_Id.lmbA_Id;
            //} else {
            //    $scope.LMBA_Id = 0;
            //}
            if (($scope.lmnbK_Price == undefined) || ($scope.lmnbK_Price == null)) {
                $scope.lmnbK_Price = 0;
                //$scope.lmnbK_NetPrice = 0;
            }
            if (($scope.lmnbK_NetPrice == undefined) || ($scope.lmnbK_NetPrice == null)) {
                //$scope.lmnbK_Price = 0;
                $scope.lmnbK_NetPrice = 0;
            }
            if (($scope.lmnbkF_PageNo == undefined) || ($scope.lmnbkF_PageNo == null)) {
                //$scope.lmnbK_Price = 0;
                $scope.lmnbkF_PageNo = 0;
            }


            if ($scope.myForm3.$valid) {
                //var publishdate = $scope.LMNBK_PublishDate == null ? "" : $filter('date')($scope.LMNBK_PublishDate, "yyyy-MM-dd");
                //var purchasedate = $scope.LMNBK_PurchaseDate == null ? "" : $filter('date')($scope.LMNBK_PurchaseDate, "yyyy-MM-dd");
                //var billdate = $scope.LMNBK_BillDate == null ? "" : $filter('date')($scope.LMNBK_BillDate, "yyyy-MM-dd");

                //angular.forEach($scope.authorlst, function (y) {
                //    if (y.lmbA_Id == $scope.LMBA_Id) {
                //        $scope.author = y.lmbA_AuthorFirstName;
                //    }
                //})


                $scope.test = $scope.totalgrid;
                var cnt = 0;

                for (var m = 0; m < $scope.totalgrid.length; m++) {
                    var stu_id = $scope.totalgrid[m].lmnbkanO_AccnNo;
                    var already_cnt = 0;
                    angular.forEach($scope.test, function (itm1) {
                        if (itm1.lmnbkanO_AccnNo == stu_id) {
                            already_cnt += 1;
                        }
                    })
                    if (already_cnt == 1) {

                    }
                    else {
                        cnt += 1;
                    }
                }
                if ($scope.lmnbK_Discount == undefined || $scope.lmnbK_Discount == null) {
                    $scope.lmnbK_Discount = 0;
                }

                debugger;
                var data = {

                    "LMNBK_Id": $scope.lmnbK_Id,
                    "LMNBK_NonBookTitle": $scope.lmnbK_NonBookTitle,
                    "LMPE_Id": $scope.lmpE_Id,
                    "LMSU_Id": $scope.lmsU_Id,
                    "LMNBK_PeriodicalTypeFlg": $scope.lmnbK_PeriodicalTypeFlg,
                    "LMNBK_ReferenceNo": $scope.lmnbK_ReferenceNo,
                    "LMNBK_BindingType": $scope.lmnbK_BindingType,
                    "LMP_Id": $scope.lmP_Id.lmP_Id,
                    "LMNBK_ISBN": $scope.lmnbK_ISBN,
                    "LMNBK_PublishDate": $scope.lmnbK_PublishDate,
                    "LMD_Id": $scope.lmD_Id,
                    "LMNBK_VolumeNo": $scope.lmnbK_VolumeNo,
                    "LMC_Id": $scope.lmC_Id,
                    "LMNBK_BindStatus": $scope.lmnbK_BindStatus,
                    "LMV_Id": $scope.lmV_Id,
                    "LMNBK_SourceType": $scope.lmnbK_SourceType,
                    "LMNBK_IssueNo": $scope.lmnbK_IssueNo,
                    "LMNBK_PurchaseDate": $scope.lmnbK_PurchaseDate,
                    "LMNBK_VoucherNo": $scope.lmnbK_VoucherNo,
                    //"LMBKF_KeyFactor": $scope.lmbkF_KeyFactor,
                    "LMNBK_NoOfPages": $scope.lmnbK_NoOfPages,
                    "LMNBK_CurrencyType": $scope.lmnbK_CurrencyType,
                    "LMNBK_Discount": $scope.lmnbK_Discount,
                    "LMNBK_Description": $scope.lmnbK_Description,
                    "LML_Id": $scope.lmL_Id,
                    "LMNBK_DonarName": $scope.lmnbK_DonarName,
                    "LMNBK_DonarAddress": $scope.lmnbK_DonarAddress,
                    "LMNBKF_KeyFactor": $scope.lmnbkF_KeyFactor,
                    "LMNBKF_PageNo": $scope.lmnbkF_PageNo,
                    "LMNBK_DiscountTypeFlg": $scope.lmnbK_DiscountTypeFlg,
                    "LMNBK_Keywords": $scope.lmnbK_Keywords,
                    "LMAL_Id": $scope.lmaL_Id,
                    "LMNBK_BillDate": $scope.lmnbK_BillDate,
                    "LMNBK_BillNo": $scope.lmnbK_BillNo,
                    "LMRA_Id": $scope.lmrA_Id,
                    "LMNBKANO_AvailableStatus": $scope.lmnbkanO_AvailableStatus,
                    "LMNBK_Price": $scope.lmnbK_Price,
                    "LMNBK_NetPrice": $scope.lmnbK_NetPrice,
                    "LMAC_Id": $scope.lmaC_Id,
                    "LMNBK_NoOfCopies": $scope.lmnbK_NoOfCopies,
                    "LMNBKANO_Id": $scope.lmnbkanO_Id,
                    "LMNBK_WithAccessoriesFlg": $scope.lmnbK_WithAccessoriesFlg,
                    savetmpdata: $scope.totalgrid,
                    "LMNBKL_Id": $scope.lmnbkL_Id,

                }
                if (cnt < 1) {
                    apiService.create("MasterNonBook/Savedata", data)
                        .then(function (promise) {
                            if (promise.returnval != null && promise.duplicate != null) {
                                if (promise.duplicate == false) {
                                    if (promise.chkaccessionno != true) {
                                        if (promise.returnval == true) {
                                            if ($scope.lmnbK_Id > 0) {
                                                swal('Record Updated Successfully!!!');
                                            }
                                            else {
                                                swal('Record Saved Successfully!!!');
                                            }
                                            $state.reload();
                                        }
                                        else {
                                            if (promise.returnval == false) {
                                                if ($scope.lmnbK_Id > 0) {
                                                    swal('Record Not Update Successfully!!!');
                                                }
                                                else {
                                                    swal('Record Not Saved Successfully!!!');
                                                }
                                            }
                                        }
                                    }
                                    else {
                                        swal("Accession No. Should Not be Duplicate!");
                                    }
                                }
                                else {
                                    swal("Record already exist");
                                }
                                //  $state.reload();
                            }
                            else {
                                swal("Kindly Contact Administrator!!!");
                            }

                        })
                }
                else {
                    swal("Accession No. Should Not be Duplicate!");
                }

            }
            else {
                $scope.submitted3 = true;
            }
        };
        //==================================================================End====================================

        $scope.searchfilter = function (objj, radioobj) {
            if (objj.search.length >= '3') {
                var data = {
                    "LMBA_AuthorFirstName": objj.search,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("MasterNonBook/searchfilter", data).
                    then(function (promise) {
                        $scope.authorlst = promise.authorlst;
                        angular.forEach($scope.authorlst, function (objectt) {
                            if (objectt.lmbA_AuthorFirstName.length > 0) {
                                var string = objectt.lmbA_AuthorFirstName;
                                objectt.lmbA_AuthorFirstName = string.replace(/  +/g, ' ');
                            }
                        })
                    })
            }
        };


        $scope.checkErr = function (LMB_EntryDate, Purchase_Date) {
            $scope.errMessage = '';
            var curDate = new Date();
            if (new Date(LMB_EntryDate) > new Date(Purchase_Date)) {
                swal('To Date should be greater than from date');

                return false;
            }
        }




        $scope.search_flag = false;
        $scope.search123 = "";
        var search_s = "";
        var list_s = [];
        $scope.onselectsearch = function () {
            search_s = $scope.search123;
            //  list_s = $scope.receiptgrid;
            if (search_s == "" || search_s == undefined) {
                swal("Select Any Field For Search");
                $scope.search_flag = false;
            }
            else {
                $scope.search_flag = true;
                $scope.txt = true;
                $scope.searchtxt = "";

            }
        }







        $scope.ShowSearch_Report = function () {
            debugger;
            if ($scope.searchtxt != "") {
                var data = {
                    "Delete_Reason": $scope.search123,
                    "Book_Prefix": $scope.searchtxt,
                    "LMAL_Id": $scope.library_id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("MasterNonBook/searching", data).
                    then(function (promise) {
                        $scope.alldata = promise.alldata;
                        $scope.totcountsearch = promise.alldata.length;

                        if (promise.alldata == null || promise.alldata == "") {
                            swal("Record Does Not Exist For Searched Data !!!!!!")
                            $state.reload();
                        }
                    })
            }
            else {
                swal("Data Is Needed For Search ");
            }

        }

        //=============================================

        $scope.changelibrary = function () {
            debugger;
            if ($scope.library_id == "") {
                $scope.library_id = 0;
            }
            var data = {
                "LMAL_Id": $scope.library_id,

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("MasterNonBook/changelibrary", data).
                then(function (promise) {
                    $scope.alldata = promise.alldata;
                    $scope.totcountsearch = promise.alldata.length;

                    if (promise.alldata == null || promise.alldata == "") {
                        swal("Record Does Not Exist For Searched Library !!!!!!")
                        //$state.reload();
                    }
                })
        }




        //-------check value for %age and Rs
        $scope.checkvalue = function () {
            if ($scope.MB_Disc_Type == '%age') {
                if (Number($scope.MB_Discount) > 100) {
                    swal('If Select %age then Can not Pass more then 100');
                    $scope.MB_Discount = '';
                }
            }
        }
        //-----------------End-----------------//






        $scope.addNew = function (totalgrid) {
            debugger;
            if (($scope.lmnbK_Price == undefined) || ($scope.lmnbK_Price == null)) {
                $scope.lmnbK_Price = 0;
                $scope.lmnbK_NetPrice = 0;
            }
            $scope.totalgrid = [];
            var lmnbkanO_AccnNo = '';
            if ($scope.lmnbK_NoOfCopies != null || $scope.lmnbK_NoOfCopies != '') {

                //if ($scope.lmnbK_Price == undefined || $scope.lmnbK_Price == null || $scope.lmnbK_Price == '') {
                //    swal('Please Enter Price/Copy');
                //}
                //else {
                var a = $scope.lmnbK_NoOfCopies;
                $scope.lmnbK_NetPrice = $scope.lmnbK_Price * a;
                for (var i = 0; i < a; i++) {
                    $scope.totalgrid.push({
                        lmnbkanO_AccnNo: '',
                    });
                }
                // }

            }


        }






        //=============Check for Duplicate data===//
        $scope.Ckeck_ISBNNO = function () {
            debugger;
            var data = {
                "LMB_Id": $scope.LMB_Id,
                "LMB_ISBNNo": $scope.LMB_ISBNNo
            }
            apiService.create("MasterNonBook/Ckeck_ISBNNO", data)
                .then(function (promise) {
                    debugger;
                    if (promise.returnval) {
                        swal("ISBN No. Should Not be Duplicate");

                        $scope.LMB_ISBNNo = "";
                    }
                });
        }


        $scope.chekAccno = function () {
            debugger;
            var data = {
                "LMBANO_Id": $scope.LMBANO_Id,
                "LMBANO_AccessionNo": $scope.LMBANO_AccessionNo,
            }
            apiService.create("MasterNonBook/chekAccno", data)
                .then(function (promise) {

                    if (promise.returnval) {
                        swal("Accession No. Should Not be Duplicate");

                        $scope.LMBANO_AccessionNo = "";
                    }
                });
        };

        //----------End..................//



        //======Import Book Details List of Data.............
        var vm = this;

        vm.gridOptions = {};
        var HostName = location.host;
        $scope.import_func = function () {
            $window.location.href = 'http://' + HostName + '/#/app/LibBookDetailsImport/';
        }
        //=====-------End-Import Book Details List of Data.............



        $scope.Clearform = function () {

            $scope.lmnbkF_KeyFactor = "";
            $scope.lmnbkF_PageNo = "";
            $scope.lmnbK_NonBookTitle = "";
            $scope.lmpE_Id = "";
            $scope.lmsU_Id = "";
            $scope.lmnbK_PeriodicalTypeFlg = "";
            $scope.lmnbK_ReferenceNo = "";
            $scope.lmnbK_BindingType = "";
           
            $scope.lmnbK_ISBN = "";
            $scope.lmnbK_PublishDate = "";
            $scope.lmD_Id = "";
            $scope.lmnbK_VolumeNo = "";
           
            $scope.lmnbK_BindStatus = "";
            $scope.lmV_Id = "";
            $scope.lmnbK_SourceType = "";
            $scope.lmnbK_IssueNo = "";
        }

        $scope.clearfield2 = function () {
            debugger;

            $scope.lmnbK_PurchaseDate = "";
            $scope.lmnbK_VoucherNo = "";
            $scope.lmnbK_NoOfPages = "";
            $scope.lmP_Id = "";
            $scope.lmC_Id = "";
            $scope.lmnbK_CurrencyType = "";
            $scope.lmnbK_Discount = "";
            $scope.lmnbK_Description = "";
            $scope.lmL_Id = "";
            $scope.lmnbK_DonarName = "";
            $scope.lmnbK_DonarAddress = "";           
            $scope.lmnbK_DiscountTypeFlg = "";
            $scope.lmnbK_Keywords = "";
        }

        $scope.clearfield3 = function () {
            debugger;
            $scope.lmaL_Id = "";
            $scope.lmnbK_BillDate = "";
            $scope.lmnbK_BillNo = "";
            $scope.lmrA_Id = "";
            $scope.lmnbkanO_AvailableStatus = "";
            $scope.lmnbK_Price = "";
            $scope.lmnbK_NetPrice = "";
            $scope.lmnbK_WithAccessoriesFlg = "";
            $scope.lmaC_Id = "";
            $scope.lmnbK_NoOfCopies = "";
            $scope.lmnbkanO_AccnNo = "";          
            $scope.addNew();
           

        }



        //-------record active & deactive
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            $scope.lmnbK_Id = user.lmnbK_Id;
            $scope.lmnbkanO_Id = user.lmnbkanO_Id;
            var dystring = "";
            if (user.lmnbkanO_ActiveFlg == 1) {
                dystring = "Deactivate";
            }
            else if (user.lmnbkanO_ActiveFlg == 0) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MasterNonBook/deactiveY", user).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }

                });
        }
        //--------------------End----------//


        //------------form submit
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };
        $scope.interacted3 = function (field) {
            return $scope.submitted3;
        };
        //----------end-------------//



        //------------clear Form
        $scope.Clearid = function () {
            $state.reload();
        }

        $scope.EditData = function (user) {
            $scope.show_vendor = true;
            $scope.show_author = true;
            $scope.show_isbnNo = true;
            debugger;
            var data = {
                "LMNBK_Id": user.lmnbK_Id,
                "LMNBKANO_Id": user.lmnbkanO_Id
            }
            apiService.create("MasterNonBook/Editdata", data)
                .then(function (promise) {
                    debugger;
                    $scope.editlis = promise.editlis;

                    $scope.lmnbK_Id = $scope.editlis[0].lmnbK_Id;
                    $scope.lmV_Id = $scope.editlis[0].lmV_Id;
                    $scope.lmnbkanO_Id = $scope.editlis[0].lmnbkanO_Id;
                    $scope.lmnbK_NonBookTitle = $scope.editlis[0].lmnbK_NonBookTitle;

                    $scope.lmC_Id = $scope.editlis[0].lmC_Id;
                    $scope.lmL_Id = $scope.editlis[0].lmL_Id;
                    $scope.lmD_Id = $scope.editlis[0].lmD_Id;
                    $scope.lmpE_Id = $scope.editlis[0].lmpE_Id;
                    $scope.lmsU_Id = $scope.editlis[0].lmsU_Id;
                    $scope.lmnbK_PeriodicalTypeFlg = $scope.editlis[0].lmnbK_PeriodicalTypeFlg;
                    $scope.lmnbK_SourceType = $scope.editlis[0].lmnbK_SourceType;
                    $scope.lmnbK_NoOfPages = $scope.editlis[0].lmnbK_NoOfPages;
                    $scope.lmnbK_ISBN = $scope.editlis[0].lmnbK_ISBN;


                    $scope.lmP_Id = $scope.editlis[0].lmP_Id;
                    $scope.lmnbK_PublishDate = new Date($scope.editlis[0].lmnbK_PublishDate);
                    $scope.lmnbK_PurchaseDate = new Date($scope.editlis[0].lmnbK_PurchaseDate);
                    $scope.lmnbK_BillDate = new Date($scope.editlis[0].lmnbK_BillDate);
                    $scope.lmnbK_BindingType = $scope.editlis[0].lmnbK_BindingType;
                    $scope.lmnbK_ReferenceNo = $scope.editlis[0].lmnbK_ReferenceNo;
                    $scope.lmnbK_VolumeNo = $scope.editlis[0].lmnbK_VolumeNo;
                    $scope.lmnbK_BindStatus = $scope.editlis[0].lmnbK_BindStatus;
                    $scope.lmnbK_IssueNo = $scope.editlis[0].lmnbK_IssueNo;
                    $scope.lmnbK_VoucherNo = $scope.editlis[0].lmnbK_VoucherNo;
                    $scope.lmnbkF_KeyFactor = $scope.editlis[0].lmnbkF_KeyFactor;
                    $scope.lmnbK_CurrencyType = $scope.editlis[0].lmnbK_CurrencyType;
                    $scope.lmnbK_Discount = $scope.editlis[0].lmnbK_Discount;
                    $scope.lmnbK_Description = $scope.editlis[0].lmnbK_Description;
                    $scope.lmnbK_DonarName = $scope.editlis[0].lmnbK_DonarName;
                    $scope.lmnbK_DonarAddress = $scope.editlis[0].lmnbK_DonarAddress;
                    $scope.lmnbkF_PageNo = $scope.editlis[0].lmnbkF_PageNo;
                    $scope.lmnbK_DiscountTypeFlg = $scope.editlis[0].lmnbK_DiscountTypeFlg;
                    $scope.lmnbK_Keywords = $scope.editlis[0].lmnbK_Keywords;

                    $scope.lmnbK_BillNo = $scope.editlis[0].lmnbK_BillNo;
                    $scope.lmnbkL_Id = $scope.editlis[0].lmnbkL_Id;

                    $scope.lmnbkanO_AvailableStatus = $scope.editlis[0].lmnbkanO_AvailableStatus;
                    $scope.lmnbK_Price = $scope.editlis[0].lmnbK_Price;
                    $scope.lmnbK_NetPrice = $scope.editlis[0].lmnbK_NetPrice;

                    $scope.lmaL_Id = $scope.editlis[0].lmaL_Id;
                    $scope.lmnbK_NoOfCopies = $scope.editlis[0].lmnbK_NoOfCopies;
                    $scope.lmrA_Id = $scope.editlis[0].lmrA_Id;
                    $scope.lmnbkanO_AccnNo = $scope.editlis[0].lmnbkanO_AccnNo;
                    $scope.show_LMBANO_No = true;
                    $scope.show_bookAccNo = true;
                    $scope.edit = true;
                    //$scope.lmnbK_WithAccessoriesFlg = $scope.editlis[0].lmnbK_WithAccessoriesFlg;
                    //$scope.lmaC_Id = $scope.editlis[0].lmaC_Id;

                    $scope.abc = $scope.editlis[0].lmnbK_WithAccessoriesFlg;
                    if ($scope.abc == true) {
                        $scope.lmnbK_WithAccessoriesFlg = 1;
                        $scope.lmaC_Id = $scope.editlis[0].lmaC_Id;
                        $scope.abc = $scope.editlis[0].lmnbK_WithAccessoriesFlg;
                    }
                    else if ($scope.abc == false) {
                        $scope.lmnbK_WithAccessoriesFlg = 0;
                        $scope.lmaC_Id = $scope.editlis[0].lmaC_Id;
                        $scope.abc = $scope.editlis[0].lmnbK_WithAccessoriesFlg;
                    }


                    $scope.totalgrid = $scope.editlis;
                    $scope.totalgrid = [];

                    $scope.totalgrid.push({
                        lmnbkanO_AccnNo: $scope.editlis[0].lmnbkanO_AccnNo
                    });

                    // $('#blah').attr('src', user.book_Image);
                    $('#blah').attr('src', $scope.editlis[0].Book_Image);



                })
        }

        $scope.exportToExcel = function (table) {
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }

        $scope.clearsearch = function () {
            $scope.searchtxt = "";
            $scope.search123="";
        }


    }
})();

