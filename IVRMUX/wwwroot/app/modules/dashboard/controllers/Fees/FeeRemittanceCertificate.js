(function () {
    'use strict';
    angular
.module('app')
.controller('FeeRemittanceCertificateController', FeeRemittanceCertificateController)
    FeeRemittanceCertificateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','superCache']
    function FeeRemittanceCertificateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.exportsheet = false;
        $scope.IsHiddenup = true;
        $scope.IsHiddendown = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.radio_button = 1;
       // $scope.pagination = true;
        $scope.file_disable = true;
        $scope.total = 0;
        $scope.validate = false;
       // $scope.tot_flag = false;
        $scope.FeeIT = true;
       
        $scope.FeeReceipt = false;
        $scope.print_flag = true;
        

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.ShowHideup = function () { 

            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }


        $scope.ShowHidedown = function () {


            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        }

        $scope.loadbasicdata = function () {
            
            apiService.get("FeeRemittanceCertificate/getinitialfeedata/").then(function (promise) {
               
                if (promise !== "") {
                    $scope.yearlist = promise.yearList;
                    $scope.date_List = promise.dateList;
                    $scope.feehead_name_list = promise.feehead_Name_list;
                }
            });
        }
        $scope.radio_fee = "1";
        $scope.loadbasicdata_fee = function () {
            
            $scope.loadIt()
        }

        $scope.loadIt = function () {

            
            if ($scope.radio_fee == "1")
            {
                $scope.FeeIT = true;
                $scope.FeeReceipt = false;
                $scope.loadbasicdata();
            }

             if ($scope.radio_fee == "2") {
                $scope.FeeIT = false;
                $scope.FeeReceipt = true;
                $scope.loaddata();
               
            }
             $scope.submitted = false;
             $scope.myForm.$setPristine();
             $scope.myForm.$setUntouched();
        }
      
       
        $scope.load_fee_receipt = function () {
            
            if($scope.rndind=="ReceiptNoWise")
            {
                $scope.receipt_wise = true;
                $scope.date_wise = false;
                $scope.dis123 = false;
                $scope.print_flag = true;
                //$scope.From_Date = Convert.ToDateTime("01 / 01 / 2016");
               // $scope.To_Date = Convert.ToDateTime("01 / 01 / 2017");
                //$scope.asmaY_Id = "null";
                
            }
            if ($scope.rndind == "dateWise") {
                $scope.receipt_wise = false;
                $scope.date_wise = true;
                $scope.dis = false;
                $scope.print_flag = true;
                //$scope.From_Date = false;
               // $scope.To_Date = false;
              //  $scope.asmaY_Id = 1;
            }
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }

        $scope.clear_feeit = function () {
           // $scope.loadbasicdata();
            $state.reload();
            //$scope.asmaY_Id = "";
            //$scope.radio_button = "1";
            //$scope.amsT_Id = "";
            //$scope.fmH_Id = "";
        }
        $scope.clear_fee_reciept = function () {
            // $state.reload();
            
            
           if ($scope.rndind == "ReceiptNoWise") {
                $scope.rndind = "ReceiptNoWise";
                $scope.fee_asmaY_Id = "";
                $scope.dis = false;

           }
           else if ($scope.rndind == "dateWise") {
                $scope.rndind = "dateWise";
                $scope.From_Date = null;
                $scope.To_Date = null;
                $scope.dis123 = false;
            }
           // $scope.Thirdparty = "";
            // $scope.Header = "";
           // $scope.To_Date = "";
            //$scope.From_Date = "";
           // $scope.fee_asmaY_Id = "";
            $scope.rcp_model = "";
            $scope.ins_model = "";
            $scope.print_flag = true;
            //$scope.dis = false;

        }
         
        $scope.IsHiddenup_Receipt = true;
        $scope.ShowHideup_Receipt = function () {

            $scope.IsHiddenup_Receipt = $scope.IsHiddenup_Receipt ? false : true;
        }



        $scope.loadData = function () {
            
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "Adm_no_name": $scope.radio_button,
            }
            apiService.create("FeeRemittanceCertificate/getadm_no_name/", data).then(function (promise) {
                
                if (promise.student_Name_List!=null) {
                   
                    $scope.student_name_List = promise.student_Name_List;
                   $scope.amsT_Id = "";
                }
                else {
                    
                    if ($scope.asmaY_Id == null  || $scope.asmaY_Id == "")
                    {
                        
                        swal("First Select Acadamic Year");
                    }
                    else {
                        swal("No Students Found Kindly Select Another Year");
                        //$scope.ASMAY_Id = "";
                    }
                    $scope.student_name_List = promise.student_Name_List;
                    $scope.amsT_Id = "";
                }
               
                   
            });
        }

        $scope.Grid_view = false;
        $scope.submitted = false;

        $scope.showreport = function () {

            
            $scope.submitted = false;

            if ($scope.myForm.$valid) {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMST_Id": $scope.amsT_Id,
            }
            
                
            apiService.create("FeeRemittanceCertificate/searchdata", data).then(function (promise) {
                
                if (promise.fee_rem_cer_list != null && promise.fee_rem_cer_list != "") {
                    $scope.Grid_view = true;
                $scope.searchdatalist = promise.fee_rem_cer_list;
                   
                        $scope.total = 0;
                        for (var i = 0; i < promise.fee_rem_cer_list.length; i++) {
                            $scope.total = (promise.fee_rem_cer_list[i].tuition_Fee_Amount + $scope.total);
                                    
                        }
                        angular.forEach($scope.searchdatalist, function (e) { $scope.searchdatalistamstT_AdmNo = e.amsT_AdmNo; })
                        angular.forEach($scope.searchdatalist, function (e) { $scope.searchdatalistasmcL_ClassName = e.asmcL_ClassName; })
                        angular.forEach($scope.searchdatalist, function (e) { $scope.searchdatalistStudentName = e.studentName; })
                        angular.forEach($scope.searchdatalist, function (e) { $scope.searchdatalistfather_Name = e.father_Name; })
                        angular.forEach($scope.searchdatalist, function (e) { $scope.searchdatalistmother_Name = e.mother_Name; })
                        $scope.file_disable = false;
                        $scope.pagination = true;
                        $scope.lbl_issue_date = $scope.Issue_Date;
                        $scope.lbl_ref_no = $scope.ref_no;
                        // $scope.tot_flag = true;
                    }

                    else {
                    swal("No Records Found");
                    $scope.Grid_view = false;
                        $scope.file_disable = true;
                        $scope.doc_sel = false;
                        $scope.pagination = false;
                        $scope.loadbasicdata();
                        $state.reload();
                                
                    }
             
                       
              

            })
        }
    else {
                $scope.submitted = true;

    }
        }


        $scope.pageChanged = function (newPage) {
            if (newPage > 0) {
                $scope.newPage = newPage;
                $scope.searchtrust();   //calling Search functionality
            }
        };

        $scope.propertyName = 'fee_Group';

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };



        //for printing in outer HTML window
        $scope.printdisble = true;
        $scope.printData_It = function () {
            
            var divToPrint = document.getElementById("Table");
            var newWin = window.open("");
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();

        }      //




        $scope.sortBydropdown = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.loaddata = function () {
            $scope.receipt_wise = true;
            $scope.date_wise = false;
           
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("FeeReceiptReport/getalldetails123", pageid).
        then(function (promise) {
            
            $scope.year_list = promise.yearlist;
            $scope.categoryarray = promise.categoryarray;
            $scope.receiptlistarray = promise.newreplist;
            
            $scope.dis = false; $scope.dis123 = false;
           // $scope.onclickloaddata();
        })
        }
        $scope.rndind = "ReceiptNoWise";
        //$scope.onclickloaddata = function () {
        //    if ($scope.rndind == "ReceiptNoWise") {
        //       // $scope.rcp_model = true;
        //      //  $scope.asmaY_Id = false;
        //        $scope.rcp_model = "";
        //        $scope.ins_model = "";
        //        $scope.recpdrp = false;
        //        $scope.acdfdrp = true;
               
                
        //    }
        //   // else if($scope.rndind =="dateWise")
        //    else {
        //       // $scope.asmaY_Id = true;
        //       // $scope.rcp_model = false;
        //        $scope.acdfdrp = false;
        //        $scope.recpdrp = true;
        //        $scope.rcp_model = "";
        //        $scope.ins_model = "";
               
        //    }
        //    if ($scope.Header == "1") {
        //        $scope.recatogorydrp = true;
        //    }
        //    else {
        //        $scope.recatogorydrp = false;
        //    }
       
        //{

        //}
        //};



        $scope.interacted_fee_reciept = function (field) {
            return $scope.submitted || field.$dirty;
        };
        


        $scope.submitted = false;
        $scope.ShowReportdata = function () {
           
          
            
            if ($scope.fee_receipt.$valid) {
                var data = {
                    "recpno": $scope.rcp_model,
                    "radioval": $scope.rndind,
                    
                }
                apiService.create("FeeReceiptReport/getreport", data).
            then(function (promise) {
                
                if (promise.reportdatelist != null && promise.reportdatelist.length > 0) {
                    $scope.students = promise.reportdatelist;
                    $scope.stunameM = $scope.naem(promise.reportdatelist);
                    $scope.admnoM = $scope.stuadmno(promise.reportdatelist);
                    $scope.clsM = $scope.classnaem(promise.reportdatelist);
                    $scope.rcpnoDM = $scope.repno(promise.reportdatelist);
                    $scope.acyrDM = $scope.acayyername(promise.reportdatelist);
                    $scope.cheqdateDM = $scope.dateofcheck(promise.reportdatelist);
                    $scope.feerecvdM = $scope.paidAmt(promise.reportdatelist);
                    $scope.payflgM = $scope.typeonmode(promise.reportdatelist);
                    $scope.rmksM = "";
                    $scope.totpaidM = $scope.getTotal(promise.reportdatelist);
                    $scope.totconsessionM = $scope.getTotal1(promise.reportdatelist);
                    $scope.totfineM = $scope.getTotal2(promise.reportdatelist);
                    $scope.totnetM = $scope.getTotal(promise.reportdatelist);
                    $scope.totbalM = $scope.getTotal(promise.reportdatelist);
                    $scope.TextBoxChanged();
                    $scope.dis = true;
                    $scope.print_flag = false;
                }
                else {
                    
                    swal("No Record Found");

                    $scope.print_flag = true;
                   // $scope.loaddata();
                   // $scope.rcp_model = "";
                    $scope.load_fee_receipt();
                  // $scope.clear_fee_reciept();
                }
            })
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.naem = function (int) {
            var total;
            angular.forEach($scope.students, function (e) { total = e.stuname; });
            return total;
        };
        $scope.stuadmno = function (int) {
            var total;
            angular.forEach($scope.students, function (e) { total = e.stuadmno; });
            return total;
        };
        $scope.classnaem = function (int) {
            var total;
            angular.forEach($scope.students, function (e) { total = e.classnaem; });
            return total;
        };
        $scope.repno = function (int) {
            var total;
            angular.forEach($scope.students, function (e) { total = e.repno; });
            return total;
        };
        $scope.acayyername = function (int) {
            var total;
            angular.forEach($scope.students, function (e) { total = e.acayyername; });
            return total;
        };
        $scope.dateofcheck = function (int) {
            var total;
            angular.forEach($scope.students, function (e) { total = e.dateofcheck; });
            return total;
        };
        $scope.paidAmt = function (int) {
            var total;
            angular.forEach($scope.students, function (e) { total = e.paidAmt; });
            return total;
        };
        $scope.typeonmode = function (int) {
            var total;
            angular.forEach($scope.students, function (e) { total = e.typeonmode; });
            return total;
        };
        $scope.getTotal = function (int) {
            var total = 0;
            angular.forEach($scope.students, function (e) {
                total += e.paidAmt;
            });
            return total;
        };
        $scope.getTotal1 = function (int) {
            var total = 0;
            angular.forEach($scope.students, function (e) {
                total += e.concessionAmt;
            });
            return total;
        };
        $scope.getTotal2 = function (int) {
            var total = 0;
            angular.forEach($scope.students, function (e) {
                total += e.fineAmt;
            });
            return total;
        };


        $scope.printData_Receipt = function () {
            
            var divToPrint = document.getElementById("printrcp123");
            var newWin = window.open("");
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();

        }
        $scope.TextBoxChanged = function () {
            var valu = $scope.ins_model;
            var pageid = 2;
            apiService.getURI("FeeReceiptReport/getinsdetils", pageid).
        then(function (promise) {
            //$scope.categoryarray = promise.insdata;
            angular.forEach(promise.insdata, function (e) {
                $scope.insnamebind = e.insname;
                $scope.insaddrebind = e.insaddress
            });
        })
        }

     


    }

})();


////by mahaboob fee receipt


//(function () {
//    'use strict';
//    angular
//.module('app')
//.controller('FeeReceiptReportController', FeeReceiptReportController123)

//    FeeReceiptReportController123.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
//    function FeeReceiptReportController123($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
//        $scope.loaddata = function () {
//            $scope.currentPage = 1;
//            $scope.itemsPerPage = 5;
//            var pageid = 2;
//            apiService.getURI("FeeReceiptReport/getalldetails123", pageid).
//        then(function (promise) {
//            $scope.categoryarray = promise.categoryarray;
//            $scope.receiptlistarray = promise.newreplist;
//            $scope.dis = false; $scope.dis123 = false;
//            $scope.onclickloaddata();
//        })
//        }
//        $scope.onclickloaddata = function () {
//            if ($scope.rndind == "ReceiptNoWise") {
//                $scope.recpdrp = false;
//            }
//            else {
//                $scope.recpdrp = true;
//            }
//            if ($scope.Header == "1") {
//                $scope.recatogorydrp = true;
//            }
//            else {
//                $scope.recatogorydrp = false;
//            }
//        };
//        $scope.ShowReportdata = function () {
//            alert("hi");

//            var data = {
//                "recpno": $scope.rcp_model,
//            }
//            apiService.create("FeeReceiptReport/getreport", data).
//        then(function (promise) {
//            $scope.students = promise.reportdatelist;
//            $scope.stunameM = $scope.naem(promise.reportdatelist);
//            $scope.admnoM = $scope.stuadmno(promise.reportdatelist);
//            $scope.clsM = $scope.classnaem(promise.reportdatelist);
//            $scope.rcpnoDM = $scope.repno(promise.reportdatelist);
//            $scope.acyrDM = $scope.acayyername(promise.reportdatelist);
//            $scope.cheqdateDM = $scope.dateofcheck(promise.reportdatelist);
//            $scope.feerecvdM = $scope.paidAmt(promise.reportdatelist);
//            $scope.payflgM = $scope.typeonmode(promise.reportdatelist);
//            $scope.rmksM = "";
//            $scope.totpaidM = $scope.getTotal(promise.reportdatelist);
//            $scope.totconsessionM = $scope.getTotal1(promise.reportdatelist);
//            $scope.totfineM = $scope.getTotal2(promise.reportdatelist);
//            $scope.totnetM = $scope.getTotal(promise.reportdatelist);
//            $scope.totbalM = $scope.getTotal(promise.reportdatelist);
//            $scope.TextBoxChanged();
//            $scope.dis = true;
//        })
//        }
//        $scope.naem = function (int) {
//            var total;
//            angular.forEach($scope.students, function (e) { total = e.stuname; });
//            return total;
//        };
//        $scope.stuadmno = function (int) {
//            var total;
//            angular.forEach($scope.students, function (e) { total = e.stuadmno; });
//            return total;
//        };
//        $scope.classnaem = function (int) {
//            var total;
//            angular.forEach($scope.students, function (e) { total = e.classnaem; });
//            return total;
//        };
//        $scope.repno = function (int) {
//            var total;
//            angular.forEach($scope.students, function (e) { total = e.repno; });
//            return total;
//        };
//        $scope.acayyername = function (int) {
//            var total;
//            angular.forEach($scope.students, function (e) { total = e.acayyername; });
//            return total;
//        };
//        $scope.dateofcheck = function (int) {
//            var total;
//            angular.forEach($scope.students, function (e) { total = e.dateofcheck; });
//            return total;
//        };
//        $scope.paidAmt = function (int) {
//            var total;
//            angular.forEach($scope.students, function (e) { total = e.paidAmt; });
//            return total;
//        };
//        $scope.typeonmode = function (int) {
//            var total;
//            angular.forEach($scope.students, function (e) { total = e.typeonmode; });
//            return total;
//        };
//        $scope.getTotal = function (int) {
//            var total = 0;
//            angular.forEach($scope.students, function (e) {
//                total += e.paidAmt;
//            });
//            return total;
//        };
//        $scope.getTotal1 = function (int) {
//            var total = 0;
//            angular.forEach($scope.students, function (e) {
//                total += e.concessionAmt;
//            });
//            return total;
//        };
//        $scope.getTotal2 = function (int) {
//            var total = 0;
//            angular.forEach($scope.students, function (e) {
//                total += e.fineAmt;
//            });
//            return total;
//        };


//        $scope.printData = function () {
//            
//            var divToPrint = document.getElementById("printrcp123");
//            var newWin = window.open("");
//            newWin.document.write(divToPrint.outerHTML);
//            newWin.print();
//            newWin.close();

//        }
//        $scope.TextBoxChanged = function () {
//            var valu = $scope.ins_model;
//            var pageid = 2;
//            apiService.getURI("FeeReceiptReport/getinsdetils", pageid).
//        then(function (promise) {
//            //$scope.categoryarray = promise.insdata;
//            angular.forEach(promise.insdata, function (e) {
//                $scope.insnamebind = e.insname;
//                $scope.insaddrebind = e.insaddress
//            });
//        })
//        }

//    }
//})();






