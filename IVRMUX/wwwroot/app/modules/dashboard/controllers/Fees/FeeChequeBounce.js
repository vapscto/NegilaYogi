(function () {
    'use strict';
    angular
.module('app')
.controller('FeeChequeBounceController', FeeChequeBounceController)

    FeeChequeBounceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','superCache']
    function FeeChequeBounceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.totcountsearch = 0;

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        $scope.cfg = {};

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("FeeChequeBounce/getalldetails", pageid).
            then(function (promise) {
                $scope.yearlst = promise.fillyear;

                $scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;

                $scope.class_list = promise.classlist;
                //$scope.studentlst = promise.fillstudent;
                $scope.receiptlst = promise.fillreceipt;
                $scope.students = promise.alldata;
                $scope.totcountfirst = promise.alldata.length;
                $scope.FCB_DATE = new Date();


               // $scope.ASMAY_Id = academicyrlst[0].asmaY_Id;

            })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
           
        };
       
        $scope.onselectacademic = function (yearlst) {
            var academicyearid = $scope.cfg.ASMAY_Id;
            apiService.getURI("FeeChequeBounce/getacademicyear", academicyearid).
       then(function (promise) {
           $scope.class_list = promise.classlist;

           //$scope.studentlst = promise.fillstudent;

           $scope.students = promise.alldata;

           $scope.getdates($scope.ASMAY_Id);
           $scope.ASMCL_Id = "";
           $scope.Amst_Id = "";
           $scope.FYP_ID = "";
           $scope.FCB_DATE = new Date();
           $scope.FCB_Remarks = "";
           $scope.studentlst = [];
           $scope.receiptlst = [];

           $scope.search123 = "";
           $scope.searchtxt = "";
           $scope.searchdat = "";

       })
        };

        $scope.savebuttn = false;
        $scope.onselectstudent = function (studentlst) {
            $scope.savebuttn = true;
            var studid = $scope.Amst_Id;
            apiService.getURI("FeeChequeBounce/getstudlistgroup", studid).
       then(function (promise) {
           $scope.receiptlst = promise.fillreceipt;
          // $scope.ASMCL_Id = "";
          // $scope.Amst_Id = "";
           $scope.FYP_ID = "";
           $scope.FCB_DATE = new Date();
           $scope.FCB_Remarks = "";
        
       })
        };


        $scope.cleardata = function () {
            //$scope.ASMAY_Id = "";
            //$scope.Amst_Id = "";
            //$scope.FYP_ID = "";
            //$scope.FCB_DATE = "";
            //$scope.FCB_Remarks = "";
            $state.reload();
        }


        $scope.DeletRecord = function (employee, SweetAlert) {
            $scope.editEmployee = employee.fcB_Id;
            var feechequebounceid = $scope.editEmployee
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("FeeChequeBounce/Deletedetails", feechequebounceid).
                   then(function (promise) {

                     //  $scope.students = promise.fillstudent;

                       if (promise.returnval == true) {

                          // $scope.masterse = promise.masterSectionData;

                           swal('Record Deleted Successfully');
                           $state.reload();
                           $scope.loaddata();
                       }
                       else {
                           swal('Record Not Deleted Successfully');
                       }
                   })
               }
               else {
                   swal("Record Deletion Cancelled");
               }
           });


            //})
        }

        $scope.edit = function (employee) {
            $scope.editEmployee = employee.fcB_Id;
            var templId = $scope.editEmployee;

            apiService.getURI("FeeChequeBounce/getSchoolTypedetails", templId).
            then(function (promise) {
                $scope.savebuttn = true;
                $scope.ASMAY_Id = promise.fillstudent[0].asmaY_ID;
                $scope.FYP_ID = promise.fillstudent[0].fyP_ID;
                $scope.Amst_Id = promise.fillstudent[0].amsT_Id;
                $scope.FCB_DATE = promise.fillstudent[0].fcB_DATE;
                $scope.FCB_Remarks = promise.fillstudent[0].fcB_Remarks;

            })
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.submitted = false;
        $scope.savedata = function (studentlst) {
            
            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_ID": $scope.cfg.ASMAY_Id,
                    "AMST_Id": $scope.Amst_Id,
                    "FYP_ID": $scope.FYP_ID,
                    "FCB_DATE": new Date($scope.FCB_DATE).toDateString(),
                    "FCB_Remarks": $scope.FCB_Remarks
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("FeeChequeBounce/", data).
           then(function (promise) {             
               if (promise.validationvalue === "Saved" || promise.validationvalue === "Updated") {
                   // $scope.thirdgrid = promise.filldata;
                   swal('Record ' + promise.validationvalue + ' Successfully');
                   $state.reload();
                   $scope.loaddata();
               }
               else {
                   if (promise.validationvalue === "not Saved" || promise.validationvalue === "not Updated")
                       swal('Record ' + promise.validationvalue + ' Successfully');
                   else {
                       swal(promise.validationvalue);
                   }
               }
               //$scope.students = promise.fillstudent;

               //swal("Record Saved Successfully")
               //$state.reload();

           })

            }
            else
            {
                $scope.submitted = true;
            }
            
        };
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

                //if ($scope.search123 == "3") {
                //    $scope.txt = false;
                //    $scope.numbr = true;
                //    $scope.dat = false;

                //}
                //else
                    if ($scope.search123 == "1") {

                    $scope.txt = false;
                   // $scope.numbr = false;
                    $scope.dat = true;

                }
                else {
                    $scope.txt = true;
                   // $scope.numbr = false;
                    $scope.dat = false;

                }
                $scope.searchtxt = "";
                $scope.searchdat = "";
               // $scope.searchnumbr = "";

            }
        }
        $scope.ShowSearch_Report = function () {
            
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            if ($scope.searchtxt != "" || $scope.searchnumbr != "" || $scope.searchdat != "") {
                //if ($scope.search123 == "3") {
                //    var data = {
                //        "searchType": $scope.search123,
                //        "searchnumber": $scope.searchnumbr
                //    }
                //}
            //else 
                if ($scope.search123 == "1") {
                    

                    var date = new Date($scope.searchdat).toDateString("dd/MM/yyyy");
                    var data = {
                        "searchType": $scope.search123,
                        "searchdate": date,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                    }
                }
                else {

                    var data = {
                        "searchType": $scope.search123,
                        "searchtext": $scope.searchtxt,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeChequeBounce/searching", data).
            then(function (promise) {
                $scope.students = promise.alldata;
                $scope.totcountsearch = promise.alldata.length;
                if (promise.alldata == null || promise.alldata == "") {
                    swal("Record Does Not Exist For Searched Data !!!!!!")
                }
            })
            }
            else {
                swal("Data Is Needed For Search ");
            }
        }
        $scope.clearsearch = function () {
            //$scope.search123 = "";
            //$scope.search_flag = false;
            //$scope.searchtxt = "";
            //$scope.searchnumbr = "";
            //$scope.searchdat = "";
            $state.reload();
            $scope.loaddata();
        }
        //search end

        //Get Section

        $scope.get_Section = function (cls) {
          
            var data = {
                "ASMAY_ID": $scope.cfg.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
            }
            apiService.create("FeeChequeBounce/get_section", data).
                then(function (promise) {
                    if (promise.fillsection.length > 0) {
                        $scope.sectionlst = promise.fillsection;
                    }
                })
        }

        //Get Section


        //MB
        $scope.get_students=function(cls)
        {
           // var class_id = $scope.ASMCL_Id;
            var data = {
                "ASMAY_ID": $scope.cfg.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            }
            apiService.create("FeeChequeBounce/get_students", data).
                then(function (promise) {
                    if (promise.fillstudent.length > 0) {
                        $scope.studentlst = promise.fillstudent;
                    }
                    else {
                        $scope.studentlst = [];
                    }
           
           $scope.Amst_Id = "";
           $scope.FYP_ID = "";
           $scope.FCB_DATE = new Date();
           $scope.FCB_Remarks = "";
           $scope.receiptlst = [];
       })
        }
        $scope.get_receipts = function (stu) {
            // var class_id = $scope.ASMCL_Id;
            var data = {
                "ASMAY_ID": $scope.cfg.ASMAY_Id,
                "AMST_Id": $scope.Amst_Id,
            }
            apiService.create("FeeChequeBounce/get_receipts", data).
       then(function (promise) {
           // $scope.class_list = promise.classlist;
           $scope.savebuttn = true;
           $scope.receiptlst = promise.fillreceipt;
           $scope.FYP_ID = "";
           $scope.FCB_DATE = new Date();
           $scope.FCB_Remarks = "";
       })
        }
        $scope.getdates = function (yr_id) {


            var iddata = yr_id;
            
            for (var k = 0; k < $scope.yearlst.length; k++) {

                if ($scope.yearlst[k].asmaY_Id == iddata) {

                    var data = $scope.yearlst[k].asmaY_Year;

                }

            }

            if (data != null) {
                
                console.log(data);
                var name, name1;
                for (var i = 0; i < data.length; i++) {
                    if (i < 4) {
                        if (i == 0) {
                            name = data[i];
                        } else {
                            name += data[i];
                        }
                    }
                    if (i > 4) {
                        if (i == 5) {
                            name1 = data[5];
                        } else {
                            name1 += data[i];
                        }
                    }
                }
                $scope.fromDate = name;
                $scope.toDate = name1;
                $scope.frommon = "";
                $scope.tomon = "";
                $scope.fromDay = "";
                $scope.toDay = "";
                // For Academic From Date
                $scope.minDatemf = new Date(
                      $scope.fromDate,
                       $scope.frommon,
                        $scope.fromDay + 1);

                $scope.maxDatemf = new Date(
                      $scope.toDate,
                       $scope.tomon,
                        $scope.toDay + 365);
                $scope.today = new Date();
            }
        }
        //MB

    }

})();