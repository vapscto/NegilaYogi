
(function () {
    'use strict';
    angular
        .module('app')
        .controller('BookRegisterController', BookRegisterController)

    BookRegisterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', '$window', 'Excel', '$timeout']
    function BookRegisterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter, $window, Excel, $timeout) {

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

        $scope.previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
            $scope.tab2 = true;
            $scope.tab1 = false;
        }


        //parents form validation
        $scope.submitted3 = false;
        $scope.submitted4 = false;
        $scope.previous1 = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
            $scope.tab3 = true;
            $scope.tab2 = false;
            $scope.tab1 = false;
        }
     


        $scope.next = function () {
      
            if ($scope.myForm.$valid) {
                $scope.tab2 = false;
                $scope.tab1 = true;
                $scope.maxDate1 = new Date($scope.LMB_EntryDate);
                $scope.myTabIndex = $scope.myTabIndex + 1;
                var entrydate = $scope.LMB_EntryDate == null ? "" : $filter('date')($scope.LMB_EntryDate, "yyyy-MM-dd");
                var data = {
                    "LMB_Id": $scope.LMB_Id,
                    "LMB_BookTitle": $scope.LMB_BookTitle,
                    "MB_Subtitle": $scope.MB_Subtitle,
                    "LMC_Id": $scope.lmC_Id,
                    "LMB_BookType": $scope.LMB_BookType,
                    "LMS_Id": $scope.lmS_Id,
                    "LMD_Id": $scope.lmD_Id,
                    "Binding_Type": $scope.Binding_Type,
                    "LMB_Edition": $scope.LMB_Edition,
                    "LMB_PublishedYear": $scope.LMB_PublishedYear,
                    "LMB_EntryDate": entrydate,
                    "MB_Call_No": $scope.MB_Call_No,
                    "LMB_ISBNNo": $scope.LMB_ISBNNo,
                    "LMB_ClassNo": $scope.LMB_ClassNo,
                    "LMB_VolNo": $scope.LMB_VolNo,
                    "MB_Pages": $scope.MB_Pages,
                    "Invoice_No": $scope.Invoice_No,
                    "Book_Image": $scope.obj.image,
                    "LMBKF_KeyFactor": $scope.lmbkF_KeyFactor,
                    "LMBKF_PageNo": $scope.lmbkF_PageNo,
                    "LMB_BookNo": $scope.lmB_BookNo,
                    "LMAL_Id": $scope.lmaL_Id,

                }

                apiService.create("BookRegister/Tab1Savedata", data)
                    .then(function (promise) {
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                        
                                    if (promise.returnval == true) {
                                        $scope.LMB_Id = promise.lmB_Id;
                                        $scope.alldata = promise.alldata;
                                        //if ($scope.LMB_Id > 0) {
                                        //    swal('Record Updated Successfully!!!');
                                        //}
                                        //else {
                                        //    swal('Record Saved Successfully!!!');
                                        //}
                                        //$state.reload();
                                    }
                                    else {
                                        if (promise.returnval == false) {
                                            //if ($scope.LMB_Id > 0) {
                                            //    swal('Record Not Update Successfully!!!');
                                            //}
                                            //else {
                                            //    swal('Record Not Saved Successfully!!!');
                                            //}

                                            swal('Record Not Saved/Updated Successfully!!!');
                                        }
                                    }
                                

                                //else {
                                //    swal("Accession No. Should Not be Duplicate!");
                                //}

                            }
                        //    else {
                        //        swal("Record already exist");
                        //    }
                        //    //  $state.reload();
                        }
                        else {
                            swal("Kindly Contact Administrator!!!");
                        }

                    })


            }
            else {
                $scope.submitted = true;
                $scope.tab2 = true;
                $scope.tab1 = false;
            }

        }
        $scope.LMP_Id = "";
        $scope.submitted2 = false;
        $scope.next1 = function () {
            debugger;
            if ($scope.myForm2.$valid) {
                $scope.tab3 = false;
                $scope.tab1 = true;
                $scope.tab2 = true;
                $scope.myTabIndex = $scope.myTabIndex + 1;
                var purcdate = $scope.Purchase_Date == null ? "" : $filter('date')($scope.Purchase_Date, "yyyy-MM-dd");
                var entrydate = $scope.LMB_EntryDate == null ? "" : $filter('date')($scope.LMB_EntryDate, "yyyy-MM-dd");
                var vendorid = 0;

                if ($scope.LMV_Id == ""|| $scope.LMV_Id == undefined || $scope.LMV_Id == null) {
                   vendorid = 0;
                }
                else {
                    vendorid = $scope.LMV_Id.lmV_Id;
                }


                if (vendorid == '' || vendorid==null) {
                    vendorid = 0;
                }
                var data = {

                    "LMB_Id": $scope.LMB_Id,
                    
                   
                    "LMB_PublishedYear": $scope.LMB_PublishedYear,
                    "With_Accessories": $scope.with_Accessories,
                   
                  
                    "Purchase_Date":purcdate,
                    "LML_Id": $scope.lmL_Id,
                    "LMB_BillNo": $scope.LMB_BillNo,
                    "Voucher_No": $scope.Voucher_No,
                    "Donor_Id": $scope.Donor_Id,
                    "CurrencyType": $scope.CurrencyType,
                    "Source_Type": $scope.Source_Type,
                    "LMB_NetPrice": $scope.LMB_NetPrice,
                    "LMV_Id": vendorid,
                                
                    "MB_Keywords": $scope.MB_Keywords,
                    "LMP_Id": $scope.LMP_Id.lmP_Id,
                    "MB_Remarks": $scope.MB_Remarks,
                    "Bibliography_Page": $scope.Bibliography_Page,
                    "Index_Page": $scope.Index_Page,
                    "LMB_PurOrDonated": $scope.lmB_PurOrDonated,
                    "LMB_DonorAddress": $scope.lmB_DonorAddress,
                    "LMAL_Id": $scope.lmaL_Id,
                }

                apiService.create("BookRegister/Tab2Savedata", data)
                    .then(function (promise) {
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {

                                if (promise.returnval == true) {
                                    $scope.LMB_Id = promise.lmB_Id;
                                    $scope.alldata = promise.alldata;
                                    //if ($scope.LMB_Id > 0) {
                                    //    swal('Record Updated Successfully!!!');
                                    //}
                                    //else {
                                    //    swal('Record Saved Successfully!!!');
                                    //}
                                    //$state.reload();
                                }
                                else {
                                    if (promise.returnval == false) {
                                        //if ($scope.LMB_Id > 0) {
                                        //    swal('Record Not Update Successfully!!!');
                                        //}
                                        //else {
                                        //    swal('Record Not Saved Successfully!!!');
                                        //}

                                        swal('Record Not Saved/Updated Successfully!!!');
                                    }
                                }


                                //else {
                                //    swal("Accession No. Should Not be Duplicate!");
                                //}

                            }
                            //    else {
                            //        swal("Record already exist");
                            //    }
                            //    //  $state.reload();
                        }
                        else {
                            swal("Kindly Contact Administrator!!!");
                        }

                    })

            }
            else {
                $scope.submitted2 = true;
                $scope.tab2 = false;
                $scope.tab1 = true;
            }
        }




        $scope.searchfilter = function (objj, radioobj) {
            if (objj.search.length >= '1') {
                var data = {
                    "LMBA_AuthorFirstName": objj.search,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("BookRegister/searchfilter", data).
                    then(function (promise) {
                        $scope.authorlst = promise.authorlst;
                        angular.forEach($scope.authorlst, function (objectt) {
                            if (objectt.lmaU_AuthorFirstName.length > 0) {
                                var string = objectt.lmaU_AuthorFirstName;
                                objectt.lmaU_AuthorFirstName = string.replace(/  +/g, ' ');
                            }
                        })
                    })
            }
        };



        //---upload Picture
        $scope.UploadStudentProfilePic = [];
        $scope.obj = {};
        $scope.uploadStudentProfilePic = function (input, document) {

            $scope.UploadStudentProfilePic = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {

                    var reader = new FileReader();

                    reader.onload = function (e) {

                        $('#blah')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();

                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }
        function Uploadprofile() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.UploadStudentProfilePic.length; i++) {
                formData.append("File", $scope.UploadStudentProfilePic[i]);
            }

            //We can send more data to server using append         
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Librarybooks", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);

                    $scope.obj.image = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }
        //---End-----------------//
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
            if ($scope.searchtxt != "") {
                var data = {
                    "Delete_Reason": $scope.search123,
                    "Book_Prefix": $scope.searchtxt,
                  //  "LMAL_Id": $scope.library_id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("BookRegister/searching", data).
                    then(function (promise) {
                        $scope.alldata = promise.alldata;
                        $scope.totcountsearch = promise.alldata.length;
                        console.log($scope.alldata);

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




        $scope.pricechange = function () {
            if ($scope.LMB_Price == null || $scope.LMB_Price == undefined || $scope.LMB_Price == '') {
                $scope.LMB_Price = 0;
            }
            if ($scope.LMB_No == null || $scope.LMB_No == undefined || $scope.LMB_No == '') {
                $scope.LMB_No = 1;
            }
            if ($scope.MB_Discount == null || $scope.MB_Discount == undefined || $scope.MB_Discount == '') {
                $scope.MB_Discount = 0;
            }
            if ($scope.MB_Disc_Type == 'Rs') {
              


                $scope.LMB_NetPrice = ($scope.LMB_Price * $scope.LMB_No) - $scope.MB_Discount;
                $scope.LMB_NetPrice = parseFloat($scope.LMB_NetPrice).toFixed(2);
            }
            else if ($scope.MB_Disc_Type == '%age') {
                if ($scope.MB_Discountper == null || $scope.MB_Discountper == undefined || $scope.MB_Discountper == '') {
                    $scope.MB_Discountper = 0;
                }

                $scope.MB_Discount = ($scope.MB_Discountper / 100) * ($scope.LMB_Price * $scope.LMB_No);
                $scope.MB_Discount = parseFloat($scope.MB_Discount).toFixed(2);;
                $scope.LMB_NetPrice = ($scope.LMB_Price * $scope.LMB_No) - $scope.MB_Discount;
                $scope.LMB_NetPrice = parseFloat($scope.LMB_NetPrice).toFixed(2);
            }

        }


        //-------check value for %age and Rs
        $scope.checkvalue = function () {
            if ($scope.MB_Disc_Type == '%age') {
                if (Number($scope.MB_Discountper) > 100) {
                    swal('If Select %age then Can not Pass more then 100');
                    $scope.MB_Discountper = '';
                }

                else {
                    if ($scope.LMB_Price == null || $scope.LMB_Price == undefined || $scope.LMB_Price == '') {
                        $scope.LMB_Price = 0;
                    }
                    if ($scope.LMB_No == null || $scope.LMB_No == undefined || $scope.LMB_No == '') {
                        $scope.LMB_No = 1;
                    }
                    if ($scope.MB_Discount == null || $scope.MB_Discount == undefined || $scope.MB_Discount == '') {
                        $scope.MB_Discount = 0;
                    }
                    if ($scope.MB_Discountper == null || $scope.MB_Discountper == undefined || $scope.MB_Discountper == '') {
                        $scope.MB_Discountper = 0;
                    }
                    $scope.MB_Discount = ($scope.MB_Discountper / 100) * ($scope.LMB_Price * $scope.LMB_No);
                    $scope.MB_Discount = parseFloat($scope.MB_Discount).toFixed(2);
                    $scope.LMB_NetPrice = ($scope.LMB_Price * $scope.LMB_No) - $scope.MB_Discount;
                    $scope.LMB_NetPrice = parseFloat($scope.LMB_NetPrice).toFixed(2);
                }
                
            }
        }
        //-----------------End-----------------//



        //-----------load data.............
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";         
            var pageid = 2;
            $scope.master_author = 0;
            apiService.getURI("BookRegister/getdetails", pageid).then(function (promise) {
                $scope.sublist = promise.subjectlist;
                $scope.deptlist = promise.deptlist;
                $scope.racklist = promise.racklist;
                $scope.langlist = promise.langlist;
                //$scope.donorlist = promise.donorlist;
                $scope.vendorlist = promise.vendorlist;
                $scope.publisherlst = promise.publisherlst;
                $scope.authorlst = promise.authorlst;
                //$scope.classlist = promise.classlist;
                $scope.categorylist = promise.categorylist;
                $scope.accessorieslist = promise.accessorieslist;
                $scope.librarylist = promise.librarylist;

                $scope.alldata = promise.alldata;
                console.log($scope.alldata);
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


        $scope.bktl = '';
        $scope.LMB_IDACN = '';
        $scope.addnewaccnno = function (bk) {
            debugger;
            $scope.bktl = bk.lmB_BookTitle;
            $scope.LMB_IDACN = bk.lmB_Id;
            $scope.lmrA_Id1 = bk.lmrA_Id;
            $scope.LMB_No1 = 1;
            $scope.totalgrid1 = [];
            if ($scope.totalgrid1.length == 0) {
                $scope.totalgrid1.push({
                    LMBANO_No: '',
                });
            }
        }

        $scope.lmrA_Id1 = '';

        $scope.Addaccnno = function () {
            debugger;

            if ($scope.myForm13.$valid) {

                $scope.test = $scope.totalgrid1;
                var cnt = 0;

                for (var m = 0; m < $scope.totalgrid1.length; m++) {
                    var stu_id = $scope.totalgrid1[m].LMBANO_No;
                    var already_cnt = 0;
                    angular.forEach($scope.test, function (itm1) {
                        if (itm1.LMBANO_No == stu_id) {
                            already_cnt += 1;
                        }
                    })
                    if (already_cnt == 1) {

                    }
                    else {
                        cnt += 1;
                    }
                }

                var data = {
                    "LMB_NoOfCopies": $scope.LMB_No1,
                    "LMRA_Id": $scope.lmrA_Id1,
                    "LMB_Id": $scope.LMB_IDACN,
                    savetmpdata: $scope.totalgrid1,

                }
                if (cnt < 1) {
                    apiService.create("BookRegister/Addaccnno", data)
                        .then(function (promise) {
                            if (promise.returnval) {
                                swal("New Book Copy Added Successfully");

                                $state.reload();
                            }
                        });
                }
                else {
                    swal('Duplicate Accession Number..')
                }
            } else {
                $scope.submitted4 = true;
            }
        }


        $scope.getacndpl = function (acn, ur) {
            debugger;
            var data = {
                "LMBANO_AccessionNo": acn,
            }

            apiService.create("BookRegister/Ckeck_LMBANO_AccessionNo", data)
                .then(function (promise) {
                    debugger;
                    if (promise.returnval) {
                        swal("Accession No. Should Not be Duplicate");

                        angular.forEach($scope.totalgrid1, function (ll) {
                            
                            if (ll.LMBANO_No == acn) {
                                ll.LMBANO_No = '';
                            }

                        })

                        
                    }
                });

        }
        $scope.addNew1 = function (totalgrid1) {
            
            $scope.totalgrid1 = [];
            
            var LMBANO_No = '';
            if ($scope.LMB_No1 != null || $scope.LMB_No1 != '') {
                var a = $scope.LMB_No1;
                for (var i = 0; i < a; i++) {
                    $scope.totalgrid1.push({
                        LMBANO_No: '',
                    });
                }

            }
           
        };



        $scope.addNew = function (totalgrid) {
            if (($scope.LMB_Price == undefined) || ($scope.LMB_Price == null)) {
                $scope.LMB_Price = 0;
                $scope.LMB_NetPrice = 0;
            } 
            $scope.totalgrid = [];
            if (($scope.LMB_Price == undefined) || ($scope.LMB_Price == null)) {
                $scope.LMB_Price = 0;
                $scope.LMB_NetPrice = 0;
            }

            if (($scope.MB_Discount == undefined) || ($scope.MB_Discount == null)) {
                $scope.MB_Discount = 0;
            }
            var LMBANO_No = '';
            if ($scope.LMB_No != null || $scope.LMB_No != '') {
                var a = $scope.LMB_No;
                if (a == undefined || a == null || a=="") {
                    a = 0;
                }

                a = Number(a);
                $scope.LMB_NetPrice = $scope.LMB_Price * a;

                $scope.LMB_NetPrice = parseFloat($scope.LMB_NetPrice - $scope.MB_Discount).toFixed(2);
                for (var i = 0; i < a; i++) {
                    $scope.totalgrid.push({
                        LMBANO_No: '',
                    });
                }

            }
            //else {
            //    $scope.submitted2 = true;
            //}


        };



        //----------End..load data.................//

        //===Check for Duplicate data===//
        $scope.Ckeck_ISBNNO = function () {
            debugger;
            var data = {
                "LMB_Id": $scope.LMB_Id,
                "LMB_ISBNNo": $scope.LMB_ISBNNo
            }
            apiService.create("BookRegister/Ckeck_ISBNNO", data)
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
            apiService.create("BookRegister/chekAccno", data)
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

     

        $scope.LMBA_Id = "";
        //----------Savedata..........
        $scope.savedata = function (bookAccNo) {
            debugger;
            if ($scope.with_Accessories == '1') {
                $scope.with_Accessories = true;
            } else {
                $scope.with_Accessories = false;
            }

            if ($scope.master_author == '1') {

            } else {
                $scope.LMAU_Id = 0;
                $scope.LMBA_Id = 0;
            }
            if ($scope.lmbkF_PageNo == undefined || $scope.lmbkF_PageNo == null) {
                $scope.lmbkF_PageNo = 0;
            }
            if (($scope.LMB_Price == undefined) || ($scope.LMB_Price == null)) {
                $scope.LMB_Price = 0;

                //$scope.LMB_NetPrice = 0;
            } 
            if (($scope.LMB_NetPrice == undefined) || ($scope.LMB_NetPrice == null)) {
                //$scope.LMB_Price = 0;
                $scope.LMB_NetPrice = 0;
            } 
            


            if ($scope.myForm3.$valid) {
               

                $scope.test = $scope.totalgrid;
                var cnt = 0;

                for (var m = 0; m < $scope.totalgrid.length; m++) {
                    var stu_id = $scope.totalgrid[m].LMBANO_No;
                    var already_cnt = 0;
                    angular.forEach($scope.test, function (itm1) {
                        if (itm1.LMBANO_No == stu_id) {
                            already_cnt += 1;
                        }
                    })
                    if (already_cnt == 1) {

                    }
                    else {
                        cnt += 1;
                    }
                }

                //if ($scope.lmbA_AuthorMiddleName == undefined || $scope.lmbA_AuthorMiddleName == null) {
                //    $scope.lmbA_AuthorMiddleName = '';
                //}
                //if ($scope.lmbA_AuthorLastName == undefined || $scope.lmbA_AuthorLastName == null) {
                //    $scope.lmbA_AuthorLastName = '';
                //}


                //var data = {

                //    "LMB_Id": $scope.LMB_Id,
                //    "LMB_BookTitle": $scope.LMB_BookTitle,
                //    "MB_Subtitle": $scope.MB_Subtitle,
                //    "LMC_Id": $scope.lmC_Id,
                //    "LMB_BookType": $scope.LMB_BookType,
                //    "LMS_Id": $scope.lmS_Id,
                //    "LMD_Id": $scope.lmD_Id,
                //    "Binding_Type": $scope.Binding_Type,
                //    "LMB_Edition": $scope.LMB_Edition,
                //    "LMB_PublishedYear": $scope.LMB_PublishedYear,
                //    "LMB_EntryDate": entrydate,
                //    "MB_Call_No": $scope.MB_Call_No,
                //    "LMB_ISBNNo": $scope.LMB_ISBNNo,
                //    "LMB_ClassNo": $scope.LMB_ClassNo,
                //    "LMB_VolNo": $scope.LMB_VolNo,
                //    "MB_Pages": $scope.MB_Pages,
                //    "Invoice_No": $scope.Invoice_No,
                //    "With_Accessories": $scope.with_Accessories,
                //    "Book_Image": $scope.obj.image,
                //    "LMB_NoOfCopies": $scope.LMB_No,
                //    "LMRA_Id": $scope.lmrA_Id,
                //    "LMBANO_AccessionNo": $scope.LMBANO_No,
                //    "Purchase_Date": purcdate,
                //    "LML_Id": $scope.lmL_Id,
                //    "LMB_BillNo": $scope.LMB_BillNo,
                //    "Voucher_No": $scope.Voucher_No,
                //    "LMB_Price": $scope.LMB_Price,
                //    "Donor_Id": $scope.Donor_Id,
                //    "CurrencyType": $scope.CurrencyType,
                //    "Source_Type": $scope.Source_Type,
                //    "LMB_NetPrice": $scope.LMB_NetPrice,
                //    "LMV_Id": $scope.lmV_Id,
                //    //   "ForTheClass": $scope.ForTheClass, //here we pass asmcL_Id and get student class Name                     
                //    "MB_Keywords": $scope.MB_Keywords,
                //    "LMP_Id": $scope.lmP_Id,
                //    "MB_Remarks": $scope.MB_Remarks,
                //    "MB_Disc_Type": $scope.MB_Disc_Type, //here we save value of radio
                //    "LMB_Discount": $scope.MB_Discount,
                //    "Bibliography_Page": $scope.Bibliography_Page,
                //    "Index_Page": $scope.Index_Page,
                //    "LMBA_Id": $scope.LMAU_Id.lmaU_Id,
                //    savetmpdata: $scope.totalgrid,
                //    "LMBA_AuthorFirstName": $scope.author,
                //    "LMBANO_AvialableStatus": $scope.lmbanO_AvialableStatus,
                //    "LMB_PurOrDonated": $scope.lmB_PurOrDonated,
                //    "LMB_DonorAddress": $scope.lmB_DonorAddress,
                //    "LMBA_AuthorFirstName": $scope.lmbA_AuthorFirstName,
                //    "LMBA_AuthorMiddleName": $scope.lmbA_AuthorMiddleName,
                //    "LMBA_AuthorLastName": $scope.lmbA_AuthorLastName,

                //    //"LMV_Id": $scope.LMV_Id,
                //    //"LMBA_Id": $scope.LMBA_Id ,
                //    "LMBANO_Id": $scope.LMBANO_Id,
                //    "LMAC_Id": $scope.lmaC_Id,
                //    "LMAL_Id": $scope.lmaL_Id,
                //    "LMB_BookNo": $scope.lmB_BookNo,
                //    "LMBKF_KeyFactor": $scope.lmbkF_KeyFactor,
                //    "LMBKF_PageNo": $scope.lmbkF_PageNo,

                //}
                $scope.FileUploda = [];
                if ($scope.materaldocuupload != null && $scope.materaldocuupload.length > 0) {
                    angular.forEach($scope.materaldocuupload, function (itm1) {
                        if (itm1.ismclT_FilePath != null && itm1.ismclT_FilePath != "") {
                            $scope.FileUploda.push({                               
                                LMBFILE_FilePath: itm1.ismclT_FilePath,
                                LMBFILE_FileName: itm1.vtadaaF_FileName,
                            })
                        }
                    })
                }
                if ($scope.LMBANO_Id == null || $scope.LMBANO_Id=='') {
                    $scope.LMBANO_Id = 0;
                }
                var data = {
                    "LMB_Id": $scope.LMB_Id,
                    "LMRA_Id": $scope.lmrA_Id,
                    "LMBANO_AccessionNo": $scope.LMBANO_No,
                    "LMB_Price": $scope.LMB_Price,
                    "Source_Type": $scope.Source_Type,
                    "LMB_NetPrice": $scope.LMB_NetPrice,
                    "MB_Disc_Type": $scope.MB_Disc_Type, //here we save value of radio
                    "LMB_Discount": $scope.MB_Discount,
                    "LMBA_Id": $scope.LMAU_Id.lmaU_Id,
                    savetmpdata: $scope.totalgrid,
                    "LMBA_AuthorFirstName": $scope.author,
                    "LMBANO_AvialableStatus": $scope.lmbanO_AvialableStatus,
                    "LMAC_Id": $scope.lmaC_Id,
                    "LMBA_AuthorFirstName": $scope.lmbA_AuthorFirstName,
                    "LMBA_AuthorMiddleName": $scope.lmbA_AuthorMiddleName,
                    "LMBA_AuthorLastName": $scope.lmbA_AuthorLastName,
                    "LMB_NoOfCopies": $scope.LMB_No,
                    //"LMV_Id": $scope.LMV_Id,
                    //"LMBA_Id": $scope.LMBA_Id ,
                    "LMBANO_Id": $scope.LMBANO_Id,
                    "With_Accessories": $scope.with_Accessories,
                    "BookFilesPdf": $scope.FileUploda,

                }
                if (cnt < 1) {
                    apiService.create("BookRegister/Savedata", data)
                        .then(function (promise) {
                            if (promise.returnval != null && promise.duplicate != null) {
                                if (promise.duplicate == false) {
                                    if (promise.chkaccessionno != true) {
                                        if (promise.returnval == true) {
                                            if ($scope.LMB_Id > 0) {
                                                swal('Record Saved/Updated Successfully!!!');
                                            }
                                            else {
                                                swal('Record Saved/Updated Successfully!!!');
                                            }
                                            $state.reload();
                                        }
                                        else {
                                            if (promise.returnval == false) {
                                                if ($scope.LMB_Id > 0) {
                                                    swal('Record Not Saved/Updated Successfully!!!');
                                                }
                                                else {
                                                    swal('Record Not Saved/Updated Successfully!!!');
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
        //-----------End-----------//


        $scope.Clearform = function () {
            $scope.LMB_BookTitle = "";
            $scope.lmB_BookNo = "";
            $scope.LMB_BookType = "";
            $scope.Binding_Type = "";
            $scope.LMB_ISBNNo = "";
            $scope.MB_Call_No = "";
            $scope.Invoice_No = "";
            $scope.LMB_ClassNo = "";
            $scope.lmD_Id = "";
            $scope.lmC_Id = "";
            $scope.lmS_Id = "";
            $scope.LMB_EntryDate = "";
            $scope.LMB_PublishedYear = "";
            $scope.MB_Subtitle = "";
            $scope.LMB_ISBNNo = "";
            $scope.MB_Pages = "";
            $scope.LMB_Edition = "";
            $scope.lmbkF_KeyFactor = "";
            $scope.lmbkF_PageNo = "";
            $scope.LMB_VolNo = "";
            $scope.lmaL_Id = "";
        }
        $scope.LMV_Id = "";
        $scope.clearfield2 = function () {
            debugger;
            $scope.Purchase_Date = "";
            $scope.lmL_Id = "";
            $scope.LMB_NetPrice = "";
            $scope.LMV_Id = "";
            $scope.LMB_Price = "";
            $scope.MB_Keywords = "";
            $scope.lmB_PurOrDonated = "";
            $scope.LMB_BillNo = "";
            $scope.Donor_Id = "";
            $scope.lmB_DonorAddress = "";
            $scope.Voucher_No = "";
            $scope.CurrencyType = "";
            $scope.Source_Type = "";
            $scope.LMP_Id = "";
            $scope.MB_Remarks = "";
            $scope.MB_Disc_Type = "";
            $scope.MB_Discount = "";
            $scope.Bibliography_Page = "";
            $scope.MB_Keywords = "";
            $scope.Index_Page = "";
        }
        $scope.clearfield3 = function () {
            debugger;
            $scope.lmbA_AuthorFirstName = "";
            $scope.with_Accessories = "";
            $scope.lmbA_AuthorMiddleName = "";
            $scope.lmbA_AuthorLastName = "";
            $scope.lmaL_Id = "";
            $scope.lmbanO_AvialableStatus = "";
            $scope.lmrA_Id = "";
            $scope.LMB_Id = "";
            $scope.lmaC_Id = "";
            $scope.LMB_No = "";
            $scope.addNew();

        }



        //-------record active & deactive
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            $scope.LMB_Id = user.LMB_Id;
            $scope.LMBANO_No = user.LMBANO_No;
            var dystring = "";
            if (user.mB_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.mB_ActiveFlag == 0) {
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
                        apiService.create("BookRegister/deactiveY", user).
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
        $scope.interacted4 = function (field) {
            return $scope.submitted4;
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

            if (user.lmbanO_Id ==null) {
                user.lmbanO_Id = 0;
            }
            
            var data = {
                "LMB_Id": user.lmB_Id,
                "LMBANO_Id": user.lmbanO_Id
            }
            apiService.create("BookRegister/Editdata", data)
                .then(function (promise) {
                    $scope.master_author = 0;
                   
                    $scope.editlis = promise.editlis;
                    $scope.authorlst = promise.authorlst;
                    //$scope.LMB_Id = user.lmB_Id;
                    //$scope.LMV_Id = user.lmV_Id;
                    //$scope.LMBA_Id = user.lmbA_Id;
                    //$scope.LMBANO_Id = user.lmbanO_Id;
                    //$scope.LMBANO_Id = user.lmbanO_Id;
                    //$scope.LMB_BookTitle = user.lmB_BookTitle;
                    //$scope.MB_Subtitle = user.mB_Subtitle;
                    //$scope.lmC_Id = user.lmC_Id;
                    //$scope.lmS_Id = user.lmS_Id;
                    //$scope.lmD_Id = user.lmD_Id;
                    //$scope.Binding_Type = user.binding_Type;
                    //$scope.LMB_Edition = user.lmB_Edition;
                    //$scope.LMB_PublishedYear = user.lmB_PublishedYear;
                    //$scope.LMB_EntryDate = new Date(user.lmB_EntryDate);
                    //$scope.MB_Call_No = user.mB_Call_No;
                    //$scope.LMB_ISBNNo = user.lmB_ISBNNo;
                    //$scope.LMB_ClassNo = user.lmB_ClassNo;
                    //$scope.LMB_VolNo = user.lmB_VolNo;
                    //$scope.MB_Pages = user.mB_Pages;
                    //$scope.obj.image = user.book_Image;
                    //$scope.LMB_No = user.lmB_NoOfCopies;
                    //$scope.Purchase_Date = new Date(user.purchase_Date);
                    //$scope.lmL_Id = user.lmL_Id;
                    //$scope.LMB_BillNo = user.lmB_BillNo;
                    //$scope.Voucher_No = user.voucher_No;
                    //$scope.LMB_Price = user.lmB_Price;
                    //$scope.CurrencyType = user.currencyType;
                    //$scope.Source_Type = user.source_Type;
                    //$scope.LMB_NetPrice = user.lmB_NetPrice;
                    //$scope.lmV_Id = user.lmV_Id;
                    //$scope.MB_Keywords = user.mB_Keywords;
                    //$scope.lmP_Id = user.lmP_Id;
                    //$scope.MB_Remarks = user.mB_Remarks;
                    //$scope.MB_Disc_Type = user.mB_Disc_Type;
                    //$scope.MB_Discount = user.lmB_Discount;
                    //$scope.Bibliography_Page = user.bibliography_Page;
                    //$scope.Index_Page = user.index_Page;
                    //$scope.LMB_NetPrice = user.lmB_NetPrice;
                    //$scope.LMBA_Id = user.lmbA_Id;
                    //$scope.LMB_BookType = user.lmB_BookType;
                    //$scope.Invoice_No = user.invoice_No;
                    //$scope.lmbanO_AvialableStatus = user.lmbanO_AvialableStatus;
                    //$scope.lmB_PurOrDonated = user.lmB_PurOrDonated;
                    //$scope.Donor_Id = user.lmB_DonorName;
                    //$scope.lmB_DonorAddress = user.lmB_DonorAddress;
                    //$scope.lmbA_AuthorFirstName = user.lmbA_AuthorFirstName,
                    //$scope.lmbA_AuthorMiddleName = user.lmbA_AuthorMiddleName,
                    //$scope.lmbA_AuthorLastName = user.lmbA_AuthorLastName,
                    //$scope.abc = user.with_Accessories;
                    $scope.LMB_Id = $scope.editlis[0].LMB_Id;
                    $scope.LMV_Id = $scope.editlis[0].LMV_Id;
                    //$scope.LMBA_Id = $scope.editlis[0].LMBA_Id;
                    $scope.LMBANO_Id = $scope.editlis[0].LMBANO_Id;
                    $scope.LMB_BookTitle = $scope.editlis[0].LMB_BookTitle;
                    $scope.MB_Subtitle = $scope.editlis[0].MB_Subtitle;
                    $scope.lmC_Id = $scope.editlis[0].LMC_Id;
                    $scope.lmS_Id = $scope.editlis[0].LMS_Id;
                    $scope.lmD_Id = $scope.editlis[0].LMD_Id;
                    $scope.Binding_Type = $scope.editlis[0].Binding_Type;
                    $scope.LMB_Edition = $scope.editlis[0].LMB_Edition;
                    $scope.LMB_PublishedYear = $scope.editlis[0].LMB_PublishedYear;
                    $scope.LMB_EntryDate = new Date($scope.editlis[0].LMB_EntryDate);
                    $scope.MB_Call_No = $scope.editlis[0].MB_Call_No;
                    $scope.LMB_ISBNNo = $scope.editlis[0].LMB_ISBNNo;
                    $scope.LMB_ClassNo = $scope.editlis[0].LMB_ClassNo;
                    $scope.LMB_VolNo = $scope.editlis[0].LMB_VolNo;
                    $scope.MB_Pages = $scope.editlis[0].MB_Pages;
                    $scope.obj.image = $scope.editlis[0].book_Image;
                    $scope.lmbkF_KeyFactor = $scope.editlis[0].LMBKF_KeyFactor;
                    $scope.lmbkF_PageNo = $scope.editlis[0].LMBKF_PageNo;
                   
                    $scope.LMB_No = $scope.editlis[0].LMB_NoOfCopies;

                    if ($scope.editlis[0].Purchase_Date != null && $scope.editlis[0].Purchase_Date != undefined && $scope.editlis[0].Purchase_Date!="") {
                        $scope.Purchase_Date = new Date($scope.editlis[0].Purchase_Date);
                    }

                  
                    $scope.lmL_Id = $scope.editlis[0].LML_Id;
                    $scope.LMB_BillNo = $scope.editlis[0].LMB_BillNo;
                    $scope.Voucher_No = $scope.editlis[0].Voucher_No;
                    $scope.LMB_Price = $scope.editlis[0].LMB_Price;
                    $scope.CurrencyType = $scope.editlis[0].CurrencyType;
                    $scope.Source_Type = $scope.editlis[0].Source_Type;
                    $scope.LMB_NetPrice = $scope.editlis[0].LMB_NetPrice;

                    $scope.LMV_Id = $scope.editlis[0];
                    $scope.LMV_Id.lmV_Id = $scope.editlis[0].LMV_Id;


                    $scope.MB_Keywords = $scope.editlis[0].MB_Keywords;


                   // $scope.lmP_Id = $scope.editlis[0].LMP_Id;
                    $scope.LMP_Id = $scope.editlis[0];
                    $scope.LMP_Id.lmP_Id = $scope.editlis[0].LMP_Id;
                    $scope.MB_Remarks = $scope.editlis[0].MB_Remarks;
                   // $scope.MB_Disc_Type = $scope.editlis[0].MB_Disc_Type;
                    $scope.MB_Discount = $scope.editlis[0].LMB_Discount;
                    $scope.Bibliography_Page = $scope.editlis[0].Bibliography_Page;
                    $scope.Index_Page = $scope.editlis[0].Index_Page;
                    $scope.LMB_NetPrice = $scope.editlis[0].LMB_NetPrice;
                 



                    $scope.LMB_BookType = $scope.editlis[0].LMB_BookType;
                    $scope.Invoice_No = $scope.editlis[0].Invoice_No;
                    $scope.lmbanO_AvialableStatus = $scope.editlis[0].LMBANO_AvialableStatus;
                    $scope.lmB_PurOrDonated = $scope.editlis[0].LMB_PurOrDonated;
                    $scope.Donor_Id = $scope.editlis[0].LMB_DonorName;
                    $scope.lmB_DonorAddress = $scope.editlis[0].LMB_DonorAddress;

                    $scope.lmbL_Id = $scope.editlis[0].LMBL_Id;

                    $scope.totalgrid = $scope.editlis;
                    $scope.totalgrid = [];


                    $scope.totalgrid.push({
                        LMBANO_No: $scope.editlis[0].LMBANO_AccessionNo
                    });

                    // $scope.lmbA_Id = $scope.editlis[0];
                   
                    $scope.LMAU_Id = $scope.editlis[0];
                    $scope.LMAU_Id.lmaU_Id = $scope.editlis[0].lmaU_Id;

                    if ($scope.LMAU_Id.lmaU_Id > 0) {
                        $scope.master_author = '1';
                        $scope.master_author = 1;
                    }
                    else {
                        $scope.lmbA_AuthorFirstName = $scope.editlis[0].LMBA_AuthorFirstName;
                        $scope.lmbA_AuthorMiddleName = $scope.editlis[0].lmbA_AuthorMiddleName;
                        $scope.lmbA_AuthorLastName = $scope.editlis[0].lmbA_AuthorLastName;
                           
                    }

                    $scope.abc = $scope.editlis[0].With_Accessories;

                    if ($scope.abc == true) {
                        $scope.with_Accessories = 1;
                        $scope.lmaC_Id = $scope.editlis[0].LMAC_Id;
                        $scope.abc = $scope.editlis[0].With_Accessories;
                    }
                    else if ($scope.abc == false) {
                        $scope.with_Accessories = 0;
                        $scope.lmaC_Id = $scope.editlis[0].LMAC_Id;
                        $scope.abc = $scope.editlis[0].With_Accessories;
                    }

                    $scope.lmaL_Id = $scope.editlis[0].LMAL_Id;
                    $scope.lmB_BookNo = $scope.editlis[0].LMB_BookNo;
                    $scope.lmrA_Id = $scope.editlis[0].LMRA_Id;
                    //  $scope.bookAccNo = $scope.editlis[0].LMBANO_AccessionNo;
                    $scope.show_LMBANO_No = false;
                    $scope.show_bookAccNo = true;
                    if ($scope.LMBANO_Id != null && $scope.LMBANO_Id != '' && $scope.LMBANO_Id !=undefined) {
                        $scope.edit = true;
                    }
                  
                    // $('#blah').attr('src', user.book_Image);
                    $scope.lmbkF_KeyFactor = $scope.editlis[0].LMBKF_KeyFactor;
                    $scope.lmbkF_PageNo = $scope.editlis[0].LMBKF_PageNo;

                    $('#blah').attr('src', $scope.editlis[0].Book_Image);

                    if (promise.bookFilesPdfEdit != null && promise.bookFilesPdfEdit.length > 0) {
                        $scope.materaldocuupload = promise.bookFilesPdfEdit;
                        angular.forEach($scope.materaldocuupload, function (dd) {                          
                            dd.ismclT_FilePath = dd.lmbfilE_FilePath;
                            dd.ismclT_FileName = dd.lmbfilE_FileName;
                        });
                    }

                    //  $scope.LMAU_Id.lmbA_Id = $scope.editlis[0].lmbA_Id;
                })

        }

        $scope.exportToExcel = function (table) {


            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }


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

            apiService.create("BookRegister/changelibrary", data).
                then(function (promise) {
                    $scope.alldata = promise.alldata;
                    $scope.totcountsearch = promise.alldata.length;

                    if (promise.alldata == null || promise.alldata == "") {
                        swal("Record Does Not Exist For Searched Library !!!!!!")
                        //$state.reload();
                    }
                })
        }

        $scope.clearsearch = function () {
            $state.reload();
        }
        $scope.previewimg_new = function (img) {
            $scope.ismclT_FilePath = img;
            var img = $scope.ismclT_FilePath;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.ismclT_FilePath);
                    $('#myimagePreview').modal('show');
                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.ismclT_FilePath);
                }
                else if (lastelement == 'pdf') {
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.ismclT_FilePath;
                    $http.get(imagedownload1, { responseType: 'arraybuffer' })
                        .success(function (response) {
                            var fileURL = "";
                            var file = "";
                            var embed = "";
                            var pdfId = "";
                            file = new Blob([(response)], { type: 'application/pdf' });
                            fileURL = URL.createObjectURL(file);
                            pdfId = document.getElementById("pdfIdzz");
                            pdfId.removeChild(pdfId.childNodes[0]);
                            embed = document.createElement('embed');
                            embed.setAttribute('src', fileURL);
                            embed.setAttribute('type', 'application/pdf');
                            embed.setAttribute('width', '100%');
                            embed.setAttribute('height', '1000');
                            pdfId.appendChild(embed);
                            $('#showpdf').modal('show');
                        });
                }
            }
            else {
                $window.open($scope.ismclT_FilePath)
            }
        };
        //added by chethan

        //add
        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input, document) {
            $scope.SelectedFileForUploadzd = input.files;
            $scope.vtadaA_FileName = input.files[0].name;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 5242880)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/png" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/jpg" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/JPG" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/pdf" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/msword" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                //5242880
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].size > 5242880) {
                    swal("File size should be less than 5 MB");
                    return;
                }
            }
        };
        function UploaddianmateralPhoto(data) {
            console.log("TADA  Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= 1; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadtrnsportdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.ismclT_FilePath = d;
                    data.vtadaaF_FileName = $scope.vtadaA_FileName;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        };
        $scope.materaldocuupload = [{ itrS_Id: 'trans1' }];
        if ($scope.materaldocuupload.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addgrnrows = function () {
            if ($scope.materaldocuupload.length > 1) {
                for (var i = 0; i === $scope.materaldocuupload.length; i++) {
                    var id = $scope.materaldocuupload[i].itrS_Id;
                    var lastChar = id.substr(id.length - 1);
                    $scope.cnt = parseInt(lastChar);
                }
            }
            $scope.cnt = $scope.cnt + 1;
            $scope.tet = 'trans' + $scope.cnt;
            var newItemNo = $scope.cnt;
            $scope.materaldocuupload.push({ 'itrS_Id': 'trans' + newItemNo });


        };
        $scope.removegrnrows = function (index, data) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

        };
        //c;ose


    }
})();

