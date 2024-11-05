
(function () {
    'use strict';
    angular
.module('app')
.controller('CollegemasterstudentconcessionController', CollegemasterstudentconcessionController)

    CollegemasterstudentconcessionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function CollegemasterstudentconcessionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        
   //     $scope.stulstdis = true;
        $scope.btndiv = false;
        $scope.search = "";
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        if (configsettings.length > 0) {
            var grouporterm = configsettings[0].fmC_GroupOrTermFlg;
        }

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (grouporterm == "T") {
            $scope.grouportername = "Term Name";
        }
        else if (grouporterm == "G") {
            $scope.grouportername = "Group Name";
        }

        //var selcount = 0;
        //$scope.onclickofconcession = function (feeheadgrid, userdata) {

        //    for (var i = 0; i < feeheadgrid.length; i++) {
        //        var selectedval = feeheadgrid[i].isSelected
        //        if (selectedval == true) {
        //            selcount = Number(selcount) + 1;
        //        }
        //    }

        //    if (Number(selcount) >= 1) {
        //        $scope.btndiv = true;
        //    }
        //    else {
        //        $scope.btndiv = false;
        //    }
        //}

        $scope.toggleAll = function (allchkdata) {
            var toggleStatus = $scope.selectedAll;
            angular.forEach($scope.feeheadgrid, function (itm) {
                itm.isSelected = toggleStatus;
            });
        }

        $scope.resultData = [];
        $scope.resultData1 = [];


        $scope.page1 = "page1";
        $scope.reverse1 = true;

        $scope.page2 = "page2";
        $scope.reverse2 = true;

        $scope.page3 = "page3";
        $scope.reverse3 = true;

        $scope.cfg = {};

        $scope.loaddata = function () {
            $scope.totcountfirst = 0;
            $scope.disableconcessionamount = true;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5

            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 5

            $scope.currentPage3 = 1;
            $scope.itemsPerPage3 = 5

            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = 5
            var pageid = 20;

            var data = {
                "configset": grouporterm,
            }

         

            apiService.create("Collegemasterstudentconcession/getdata", data).then(function (promise) {

                //$scope.arrlist6 = academicyrlst;
                //$scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;

                $scope.arrlist6 = promise.yearlst;
                $scope.groupcount = promise.grouplist;
                $scope.thirdgrid = promise.savedrecord;
                $scope.totcountfirst = $scope.thirdgrid.length;
                $scope.getyear();
            })
        };

        $scope.getyear = function () {
            $scope.totcountfirst = 0;
            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
            }
            apiService.create("Collegemasterstudentconcession/get_courses", data).
                then(function (promise) {
                    $scope.coursecount = promise.courselist;
                    $scope.thirdgrid = promise.savedrecord;
                    $scope.totcountfirst = $scope.thirdgrid.length;
                    $scope.groupcount = promise.grouplist;
            })
        };

        $scope.get_branches = function () {
            
            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                "AMCO_Id": $scope.amcO_Id,
            }
            apiService.create("Collegemasterstudentconcession/get_branches", data).
                then(function (promise) {
                    $scope.branchcount = promise.branchlist;

                })

        };

        $scope.get_semisters = function () {
            
            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": $scope.amB_Id,
            }


            apiService.create("Collegemasterstudentconcession/get_semisters", data).
                then(function (promise) {

                    $scope.semestercount = promise.semisterlist;


                })

        };

        $scope.maindiv = false;

        $scope.gridview1 = false;
        $scope.get_student = function () {
            
            var AMS_Id = [];
            angular.forEach($scope.semestercount, function (ty) {
                if (ty.selected) {
                    AMS_Id.push(ty.amsE_Id);
                }
            })

            $scope.groupdiv = true;
                    var data = {
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMCO_Id": $scope.amcO_Id,
                        "AMB_Id": $scope.amB_Id,
                        AMS_Id: AMS_Id,
                    }
            apiService.create("Collegemasterstudentconcession/get_student", data).
           then(function (promise) {
               $scope.gridview1 = true;
               $scope.studentsdata = promise.studentlist;

               $scope.sort2 = function (keyname) {
                   $scope.sortKey = keyname;   //set the sortKey to the param passed
                   $scope.reverse = !$scope.reverse; //if true make it false and vice versa
               }
           });
        }
        $scope.temptermarray = [];
        $scope.gridview2 = false;
        var amstidstud;
        $scope.fillheads = function (option, studentsdata) {
            
            var FMG_Ids = [];
            $scope.temptermarray = [];
            angular.forEach($scope.groupcount, function (ty) {
                if (ty.selected) {
                    FMG_Ids.push(ty.fmG_Id);
                }
            })
            if (FMG_Ids.length > 0) {

                for (var i = 0; i < studentsdata.length; i++) {
                    if (studentsdata[i].studchecked == true) {
                        amstidstud = studentsdata[i].amcsT_Id;
                    }
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                var concessionamount, headid, insid, savedheadid, savedinsid, contype, remarks;
                apiService.create("Collegemasterstudentconcession/fillhead", data).
           then(function (promise) {

               $scope.saved_data_len = promise.savedcondatalist.length;

               if (promise.fillheaddata.length > 0) {
                   if (promise.savedcondatalist != null) {
                       if (promise.savedcondatalist.length > 0) {
                           for (var i = 0; i < promise.savedcondatalist.length; i++) {

                               savedheadid = promise.savedcondatalist[i].fmH_Id;
                               savedinsid = promise.savedcondatalist[i].ftI_Id;
                               concessionamount = promise.savedcondatalist[i].fscI_ConcessionAmount;
                               contype = promise.savedcondatalist[i].fcsC_ConcessionType;
                               remarks = promise.savedcondatalist[i].fcsC_ConcessionReason;

                               for (var j = 0; j < promise.fillheaddata.length; j++) {

                                   headid = promise.fillheaddata[j].fmH_Id;
                                   insid = promise.fillheaddata[j].ftI_Id;

                                   if (savedheadid == headid && savedinsid == insid) {
                                       promise.fillheaddata[j].fscI_ConcessionAmount = concessionamount;
                                       promise.fillheaddata[j].fcsC_ConcessionType = contype;
                                       promise.fillheaddata[j].fcsC_ConcessionReason = remarks;

                                       promise.fillheaddata[j].isSelected = true;
                                       promise.fillheaddata[j].fmA_Amount += promise.fillheaddata[j].fscI_ConcessionAmount;
                                      
                                       if (promise.fillheaddata[j].fcsC_ConcessionType == "Percent") {
                                         
                                           promise.fillheaddata[j].fscI_ConcessionAmount = (promise.fillheaddata[j].fscI_ConcessionAmount / promise.fillheaddata[j].fmA_Amount) * 100;

                                       }


                                   }

                               }
                           }
                       }
                   }

                   $scope.gridview2 = true;
              
               }

             
               $scope.feeheadgrid = promise.fillheaddata;
           })
            }
            else {
          
                $scope.feeheadgrid = [];
            }

        };

        $scope.Clearid = function () {
            $state.reload();
        }

        $scope.onselectstudent = function (studentid) {

            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                "AMCST_Id": studentid
            }

            apiService.create("Collegemasterstudentconcession/onselectstudent", data).
       then(function (promise) {
           $scope.Net_amount = promise.netamount;
       })
        };


        $scope.onselecthead = function (headid) {

            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                "FMH_Id": headid
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("Collegemasterstudentconcession/onselecthead", data).
       then(function (promise) {
           $scope.Net_amount = promise.netamount;
       })
        };


        $scope.onstudentclick = function (groupcount, amcsT_Id) {
            
          
            $scope.disableconcessionamount = false;
         
            var AMS_Id = [];
            angular.forEach($scope.semestercount, function (ty) {
                if (ty.selected) {
                    AMS_Id.push(ty.amsE_Id);
                }
            })

            angular.forEach($scope.studentsdata, function (yu) {
                if (yu.amcsT_Id == amcsT_Id && yu.studchecked == false) {
                    yu.studchecked = true;
                }
            })
            angular.forEach($scope.studentsdata, function (yu) {
                if (yu.amcsT_Id != amcsT_Id) {
                    yu.studchecked = false;
                }
            })

            var data = {

                "AMCST_Id": amcsT_Id,
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": $scope.amB_Id,
                AMS_Id: AMS_Id,
                "FMG_Id": $scope.fmG_Id
               // "multiplegroups": gouportermcount,
             //   "configset": grouporterm,
            }

            var concessionamount, headid, insid, savedheadid, savedinsid, contype, remarks;

            apiService.create("Collegemasterstudentconcession/fillamount", data).
       then(function (promise) {

        //   $scope.saved_data_len = promise.savedcondatalist.length;

           if (promise.fillheaddata.length > 0) {
               if (promise.savedcondatalist != null) {
                   if (promise.savedcondatalist.length > 0) {
                       for (var i = 0; i < promise.savedcondatalist.length; i++) {

                           savedheadid = promise.savedcondatalist[i].fmH_Id;
                           savedinsid = promise.savedcondatalist[i].ftI_Id;
                           concessionamount = promise.savedcondatalist[i].fscI_ConcessionAmount;
                           contype = promise.savedcondatalist[i].fcsC_ConcessionType;
                           remarks = promise.savedcondatalist[i].fcsC_ConcessionReason;

                           for (var j = 0; j < promise.fillheaddata.length; j++) {

                               headid = promise.fillheaddata[j].fmH_Id;
                               insid = promise.fillheaddata[j].ftI_Id;

                               if (savedheadid == headid && savedinsid == insid) {
                                   promise.fillheaddata[j].fscI_ConcessionAmount = concessionamount
                                   promise.fillheaddata[j].fcsC_ConcessionType = contype
                                   promise.fillheaddata[j].fcsC_ConcessionReason = remarks

                                   promise.fillheaddata[j].isSelected = true;
                                   promise.fillheaddata[j].fmA_Amount += promise.fillheaddata[j].fscI_ConcessionAmount;
                                   if (promise.fillheaddata[j].fcsC_ConcessionType == "Percent") {

                                       promise.fillheaddata[j].fscI_ConcessionAmount = (promise.fillheaddata[j].fscI_ConcessionAmount / promise.fillheaddata[j].fmA_Amount) * 100;

                                       //der.fcscI_ConcessionAmount = (Number(der.fcscI_ConcessionAmount) / 100) * der.fmA_Amount;
                                   }
                               }

                           }
                       }
                   }
               }
               var nonzerolist = [];

               angular.forEach(promise.fillheaddata, function (ie) {
                   if (ie.fmA_Amount > 0) {
                       nonzerolist.push(ie);
                   }
               })


               $scope.gridview2 = true;
               $scope.btndiv = true;
               $scope.feeheadgrid = nonzerolist;
               if (nonzerolist.length == 0) {
                   swal("Selected Student has paid All Due amount");
               }
           }
           else {
               swal("Kindly Map Group");
           }

         

       });
        }



       // $scope.cleardata = function () {

        //     $scope.AMCST_Id = "";
       //     $scope.FMG_Id = "";
       //     $scope.REF_DATE = "";
       //     $scope.REF_REMARKS = "";
       //     $scope.REF_BANK_CASH = "";
       //     $scope.REF_CheqDate = "";
       //     $scope.REF_CheqNo = "";
       //     $scope.REF_REC_No = "";
       //     $scope.REF_BANK_NAME = "";
       //     $scope.L_Code = "";
       //     $scope.ASMCL_Id = "";
       //     $scope.ASMAY_Id = "";
       //     $scope.gridview1 = false;
       // }


        $scope.DeletRecord = function (employee, SweetAlert) {
            
            $scope.editEmployee = employee;
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
                    apiService.getURI("Collegemasterstudentconcession/DeletRecord", feechequebounceid).
                   then(function (promise) {

                     //  $scope.thirdgrid = promise.fillthirdgriddata;
             //          swal(promise.validationvalue);
                       if (promise.returnval == "true") {
                           swal("Record Deleted Successfully");
                           $state.reload();
                       }
                       else {
                           swal("Record Not Deleted");
                       }
                   })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });

        }


      


       // $scope.reF_AMOUNT = true;
       // $scope.balanceamt = function (students, index) {

       //     {
       //         $scope.Bal_AMOUNT = Number(students[index].reF_AMOUNT) - Number(students[index].enteredamt)
       //     }

       // }

       // $scope.edit = function (employee) {
       //     $scope.editEmployee = employee.fmR_ID;
       //     var templId = $scope.editEmployee;

       //     apiService.getURI("Collegemasterstudentconcession/editdetails", templId).
       //     then(function (promise) {

       //         $scope.gridview1 = true;

       //         $scope.ASMAY_Id = promise.fillthirdgriddata[0].asmaY_ID;

       //         $scope.students = promise.fillthirdgriddata;

       //         $scope.enteredamt = promise.fillthirdgriddata[0].reF_AMOUNT;

       //         $scope.Bal_AMOUNT = true;
       //         $scope.Bal_AMOUNT = Number(promise.fillthirdgriddata[0].reF_AMOUNT) - Number($scope.enteredamt)

       //         $scope.ASMCL_Id = promise.fillthirdgriddata[0].asmcL_ID;
        //         $scope.amcsT_Id = promise.fillthirdgriddata[0].amcsT_Id;
       //         $scope.FMG_Id = promise.fillthirdgriddata[0].fmG_ID;

       //         $scope.REF_BANK_CASH = promise.fillthirdgriddata[0].reF_BANK_CASH;
       //         $scope.reF_AMOUNT = promise.fillthirdgriddata[0].reF_AMOUNT;
       //         $scope.REF_BANK_NAME = promise.fillthirdgriddata[0].reF_BANK_NAME;
       //         $scope.REF_CheqDate = promise.fillthirdgriddata[0].reF_CheqDate;
       //         $scope.REF_CheqNo = promise.fillthirdgriddata[0].reF_CheqNo;
       //         $scope.REF_DATE = promise.fillthirdgriddata[0].reF_DATE;
       //         $scope.REF_REC_No = promise.fillthirdgriddata[0].reF_REC_No;
       //         $scope.REF_REMARKS = promise.fillthirdgriddata[0].reF_REMARKS;
       //     })
       // }

       // $scope.interacted = function (field) {

       //     return $scope.submitted;
       // };

       // $scope.submitted = false;

        $scope.savedata = function (studentsdata, feeheadgrid, staffdata) {

                $scope.resultData = [];
                angular.forEach(studentsdata, function (student) {
                    if (student.studchecked == true) {
                        $scope.resultData.push(student);
                    }
                });
            
           


            $scope.resultData1 = [];
            angular.forEach(feeheadgrid, function (student1) {
                if (student1.isSelected == true) {
                    $scope.resultData1.push(student1);

                }
            });

            var amount = 0, saveflag = "Accept";
            for (var i = 0; i < feeheadgrid.length; i++) {
                var selectedval = feeheadgrid[i].isSelected

                if (selectedval == true) {
                    if (Number(feeheadgrid[i].fscI_ConcessionAmount) == 0 || feeheadgrid[i].fcsC_ConcessionType == null) {
                        saveflag = "save";
                    }
                }
            }





            if (saveflag != "save" && $scope.resultData.length > 0 && $scope.resultData1.length > 0) {
                if ($scope.resultData1.length > 0 && saveflag != "save") {
                    angular.forEach($scope.resultData1, function (der) {
                        if (der.fcsC_ConcessionType == "Percent") {
                            der.fscI_ConcessionAmount = (Number(der.fscI_ConcessionAmount) / 100) * der.fmA_Amount;
                        }
                    })
                }
             
                    var data = {
                        "AMC_ID": $scope.AMC_ID,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                       // "radiobtnvalue": $scope.checkboxval,
                        "FMG_Id": $scope.fmG_Id,
                        savetmpdata: $scope.resultData,
                        savetmpdata1: $scope.resultData1,
                    }
                
         

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("Collegemasterstudentconcession/savedata", data).
           then(function (promise) {

               if (promise.returnval == "false") {
                   swal("Kindly contact Administrator")
               }
               else {
                   swal("Data Saved/Updated Successfully!!!!!")
               }
               $state.reload();
           })

            }
            else {
                //swal("Kindly select minimum paramater value to save the record");
                swal("Kindly Select Type and Enter Concession For Selected Records");
            }
           

        };


       // $scope.concessionamount = function (feeheadgrid, index) {
       //     
       //     if (feeheadgrid[index].fcsC_ConcessionType == "Amount") {
       //         if (Number(feeheadgrid[index].fscI_ConcessionAmount) > Number(feeheadgrid[index].fmA_Amount)) {
       //             swal("Enter Amount cannot be greater than Net Amount");
       //             feeheadgrid[index].fscI_ConcessionAmount = 0;
       //         }
       //     }
       //     else if (feeheadgrid[index].fcsC_ConcessionType == "Percent") {
               
       //         if (Number(feeheadgrid[index].fscI_ConcessionAmount) > 100) {
       //             swal("Percentage Value Not More Than 100");
       //             feeheadgrid[index].fscI_ConcessionAmount = 0;
       //         }
               
       //     }


       // };
       // $scope.clear_amount = function (feeheadgrid, index) {
       //     feeheadgrid[index].fscI_ConcessionAmount = 0;
       // }

       // $scope.DeletRecord = function (fcscidpri, fcsci_id) {
       //     

       //     if ($scope.checkboxval != "Staff") {
       //         var data = {
       //             "FCSC_Id": fcscidpri,
       //             "FcSCI_Id": fcsci_id,
       //             "radiobtnvalue": $scope.checkboxval,
       //         }
       //     }
       //     else if ($scope.checkboxval == "Staff") {
       //         var data = {
       //             "feC_Id": fcscidpri,
       //             "fecI_Id": fcsci_id,
       //             "radiobtnvalue": $scope.checkboxval,
       //         }
       //     }


       //     var config = {
       //         headers: {
       //             'Content-Type': 'application/json;'
       //         }
       //     }

       //     swal({
       //         title: "Are you sure?",
       //         text: "Do You Want To Delete Record?",
       //         type: "warning",
       //         showCancelButton: true,
       //         confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
       //         cancelButtonText: "Cancel",
       //         closeOnConfirm: false,
       //         closeOnCancel: false
       //     },
       //    function (isConfirm) {
       //        if (isConfirm) {
       //            apiService.create("Collegemasterstudentconcession/Deletedetails", data).
       //            then(function (promise) {

       //                if (promise.returnval == "true") {
       //                    swal('Record Deleted Successfully');
       //                    $state.reload();
       //                }
       //                else if (promise.returnval == "paid") {
       //                    swal("Transaction Done,So Record Can't Delete");
       //                    $state.reload();
       //                }
       //                else {
       //                    swal('Record Not Deleted Successfully');
       //                    $state.reload();
       //                }
       //            })
       //        }
       //        else {
       //            swal("Record Deletion Cancelled");
       //            $state.reload();
       //        }
       //    });


       //     //})
       // }



    }


})();
