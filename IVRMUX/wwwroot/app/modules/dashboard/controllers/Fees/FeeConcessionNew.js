(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeConcessionNewController', FeeConcessionNewController)

    FeeConcessionNewController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function FeeConcessionNewController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

        $scope.classacademicdrp = true;
        $scope.stulstdis = true;
        $scope.btndiv = false;
        $scope.search = "";

        $scope.disabletrms = true; 

        $scope.selectall = false;
        $scope.studdetails = false;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (configsettings.length > 0) {
            var grouporterm = configsettings[0].fmC_GroupOrTermFlg;
        }


        var selcount = 0;
       


      
        $scope.loaddata = function () {
           

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            var pageid = 1;
            apiService.getURI("FeeConcessionNew/getalldetails", pageid).
                then(function (promise) {
            //apiService.create("FeeConcessionNew/getalldetails", pageid).
            //    then(function (promise) {

     
                    $scope.academiyrcnt = promise.fillyear;
                    $scope.ASMAY_Id = academicyrlst[0].asmaY_Id;
                    $scope.classlist = promise.fillclass;
                    $scope.feegroup = promise.fillgroup
                    $scope.thirdgrid = promise.studentdata;
                    $scope.totcountfirst = promise.studentdata.length;
         

                 //   $scope.onclickloaddata();
                    
                })

            

        };
        
        $scope.studentdetails = [];
        var remove_list = [];
        var ins_spe_list = [];
   
     
        $scope.onselectedfeegroup = function () {

            var data = {
                "FMG_Id": $scope.FMG_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeConcessionNew/getfeehead", data).
                then(function (promise) {


                    $scope.feeterm = promise.fillheaddata;


                })
        };

        $scope.onclickterm = function () {
            if ($scope.checkboxval == true) {
                $scope.checkboxval1 = false;
                $scope.studdetails = false;
            }
            var data = {
                "FMG_Id": $scope.FMG_Id,
                "FMH_Id": $scope.FMH_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeConcessionNew/getterm", data).
                then(function (promise) {


                    $scope.termcount = promise.fillterm;


                })
        };

        $scope.onclickinstallment = function () {

            if ($scope.checkboxval1 == true) {
                $scope.checkboxval = false;
                $scope.studdetails = false;
            }
            var data = {
                "FMG_Id": $scope.FMG_Id,
                "FMH_Id": $scope.FMH_Id,
                "FMCC_Id": $scope.fmcC_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeConcessionNew/getinstallment", data).
                then(function (promise) {


                    $scope.installmentcount = promise.filinstallment;


                })
        };
        $scope.studentsdata = function (fmT_ID) {
           
            var data = {
                "FMG_Id": $scope.FMG_Id,
                "FMH_Id": $scope.FMH_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                //ASMCL_Ids: class_Ids,
                "FMT_ID": fmT_ID,
                "FMCC_Id": $scope.fmcC_Id,
                //FMT_Ids: term_Ids

            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeConcessionNew/studentdata", data).
                then(function (promise) {
                    if (promise.studentdata.length > 0 && promise.studentdata.length != null) {
                        $scope.studdetails = true;
                        //termcount
                        $scope.student = promise.studentdata;
                        $scope.amount = promise.fmA_Amount;
                        $scope.FMC_Id = promise.fmC_Id;
                    }
                    else {
                        swal("Record Not Found ");
                        $scope.studdetails = false;
                    }


                })
        };

        $scope.studentsdatatwo = function (ftI_Id) {
          
            var data = {
                "FMG_Id": $scope.FMG_Id,
                "FMH_Id": $scope.FMH_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                //ASMCL_Ids: class_Ids,
                //FMI_Ids: installment
                "FMCC_Id": $scope.fmcC_Id,
                "FTI_Id": ftI_Id

            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeConcessionNew/studentdata1", data).
                then(function (promise) {

                    if (promise.studentdata.length > 0 && promise.studentdata.length != null) {
                        $scope.studdetails = true;
                
                        $scope.student = promise.studentdata;
                        $scope.amount = promise.fmA_Amount;
                        $scope.FMC_Id = promise.fmC_Id;
                    }
                    else {
                        swal("Record Not Found ");
                        $scope.studdetails = false;
                    }
                })
        };
        $scope.toggleAllstu = function (allchkdatastu) {

            if (allchkdatastu == true) {
                var toggleStatusstu = $scope.selectedAllstu;
                angular.forEach($scope.student, function (itm) {
                    itm.studchecked = toggleStatusstu;
                });

          

     
            }
            else {

                angular.forEach($scope.student, function (itm) {
                    itm.studchecked = false;
                });

                $scope.selectedAllstu = false;

                $scope.gridview2 = false;
                $scope.feeheadgrid.length = 0;
            }

        }

        $scope.toggleAllstudent = function (allchkdatastu) {

            if (allchkdatastu == true) {
                var toggleStatusstu = $scope.selectedAllstu;
                angular.forEach($scope.studentsdata, function (itm) {
                    itm.studchecked = toggleStatusstu;
                });




            }
            else {

                angular.forEach($scope.studentsdata, function (itm) {
                    itm.studchecked = false;
                });

                $scope.selectedAllstu = false;

                $scope.gridview2 = false;
                $scope.feeheadgrid.length = 0;
            }

        }

        $scope.Save = function () {
           
            angular.forEach($scope.student, function (itm) {
                if (itm.studchecked == true) {

                    $scope.studentdetails.push(itm);
                   
                }
            });

            var data = {
                savetmpdata: $scope.studentdetails,
                "ASMAY_Id": $scope.ASMAY_Id,
                "FMG_Id": $scope.FMG_Id,
                "FMH_Id": $scope.FMH_Id,
                "FSCI_ConcessionAmount":$scope.concessionAmount
               // "FTI_Id": $scope.ftI_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeConcessionNew/Save", data).
                then(function (promise) {

                    //termcount
                    if (promise.message == "true") {
                        swal("Record Saved Successfully");
                        $scope.studentdetails = [];
                        $scope.thirdgrid = promise.studentdata;
                    }
                    else {
                        swal("Request Failed");
                        $scope.studentdetails = [];
                 
                    }
                   // $scope.student = promise.studentdata;


                })

        };




        $scope.edittransaction = function (employee, employee1) {

            var data = {
                "FSCI_ID": employee1,
                "configset": grouporterm
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeConcessionNew/EditconcessionDetails", data).
                then(function (promise) {

                    $scope.btndiv = true;
                    $scope.gridview1 = true;
                    $scope.gridview2 = true;
                    $scope.disableconcessionamount = false;
                    $scope.disableconcessionamountstu = false;
                   

                    $scope.academiyrcnt = promise.fillyear;
                    $scope.ASMAY_Id = academicyrlst[0].asmaY_Id;
                    $scope.studentsdata = promise.editfeeDetails;
                    $scope.academiyrcnt = promise.fillyear;
                    $scope.ASMAY_Id = academicyrlst[0].asmaY_Id;
                    $scope.feeheadgrid = promise.editfeeDetails;
                    $scope.classcount = promise.fillclass;
                    $scope.ASMCL_Id = promise.editfeeDetails[0].asmcL_ID;
                    $scope.groupcount = promise.fillgroup;
                    $scope.FMG_Id = promise.editfeeDetails[0].fmt_id;

       
                })
        }

        $scope.DeletRecord = function (fscidpri, fsci_id) {
            debugger;

            if ($scope.checkboxval != "Staff" && $scope.checkboxval != "Others") {
                var data = {
                    "FSC_Id": fscidpri,
                    "FSCI_Id": fsci_id,
                    "radiobtnvalue": $scope.checkboxval,
                    "ASMAY_Id": $scope.ASMAY_Id
                }
            }
            else if ($scope.checkboxval == "Staff") {
                var data = {
                    "FEC_Id": fscidpri,
                    "FECI_Id": fsci_id,
                    "radiobtnvalue": $scope.checkboxval,
                    "ASMAY_Id": $scope.ASMAY_Id
                }
            }
            else if ($scope.checkboxval == "Others") {
                var data = {
                    "FOC_Id": fscidpri,
                    "FOCI_Id": fsci_id,
                    "radiobtnvalue": $scope.checkboxval,
                    "ASMAY_Id": $scope.ASMAY_Id
                }
            }


            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

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
                        apiService.create("FeeConcessionNew/Deletedetails", data).
                            then(function (promise) {

                                if (promise.returnval == "true") {
                                    swal('Record Deleted Successfully');
                                    $state.reload();
                                }
                                else if (promise.returnval == "paid") {
                                    swal("Transaction Done,So Record Can't Delete");
                                    $state.reload();
                                }
                                else {
                                    swal('Record Not Deleted Successfully');
                                    $state.reload();
                                }
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                        $state.reload();
                    }
                });


            //})
        }

        $scope.savedata = function (student, feeheadgrid) {

       
                $scope.resultData = [];
                angular.forEach($scope.studentsdata, function (student) {
                    if (student.studchecked == true) {
                        $scope.resultData.push(student);
                    }
                });
           


            $scope.resultData1 = [];
            
            var count = 0;
            var temp_concessionamount = 0;
            if (ins_spe_list.length == 0 && remove_list.length == 0) {
                angular.forEach($scope.feeheadgrid, function (opq) {
                    if (opq.isSelected) {
                        count += 1;
                        //  opq.netAmount = Number(opq.netAmount);
                        $scope.resultData1.push(opq);
                    }
                })
            }
            else if (ins_spe_list.length > 0 && remove_list.length > 0) {
                angular.forEach(feeheadgrid, function (opq) {
                    opq.fsC_ConcessionType = "Amount";
                    if (opq.isSelected) {
                        if (opq.Head_Flag == 'H') {
                            count += 1;
                            //  opq.netAmount = Number(opq.netAmount);
                            $scope.resultData1.push(opq);
                        }
                        else if (opq.Head_Flag == 'SH') {
                            angular.forEach(ins_spe_list, function (s) {
                                if (s.ftI_Id == opq.ftI_Id) {
                                    angular.forEach(s.sp_list, function (s1) {
                                        if (s1.sp_id == opq.fmH_Id) {
                                            if (opq.fsC_ConcessionType == 'Amount') {
                                                var toBePaid = 0;
                                                temp_concessionamount = Number(opq.fscI_ConcessionAmount);
                                                angular.forEach(s1.sp_ind_list, function (s2) {
                                                    toBePaid += Number(s2.fmA_Amount);
                                                })
                                                if (toBePaid == Number(opq.fscI_ConcessionAmount)) {
                                                    angular.forEach(s1.sp_ind_list, function (s2) {
                                                        count += 1;
                                                        s2.fscI_ConcessionAmount = s2.fmA_Amount;
                                                        s2.fsC_ConcessionType = opq.fsC_ConcessionType;
                                                        $scope.resultData1.push(s2);
                                                    })
                                                }
                                                else if (toBePaid > Number(opq.fscI_ConcessionAmount)) {

                                                    var keepGoing = true;
                                                    angular.forEach(s1.sp_ind_list, function (s2) {
                                                        if (keepGoing) {
                                                            if (Number(opq.fscI_ConcessionAmount) >= Number(s2.fmA_Amount)) {
                                                                count += 1;
                                                                s2.fscI_ConcessionAmount = s2.fmA_Amount;
                                                                s2.fsC_ConcessionType = opq.fsC_ConcessionType;
                                                                $scope.resultData1.push(s2);
                                                                opq.fscI_ConcessionAmount = (Number(opq.fscI_ConcessionAmount) - Number(s2.fmA_Amount));
                                                            }
                                                            else if (Number(opq.fscI_ConcessionAmount) < Number(s2.fmA_Amount)) {
                                                                // s2.fsS_ToBePaid = Number(opq.fscI_ConcessionAmount);
                                                                s2.fscI_ConcessionAmount = Number(opq.fscI_ConcessionAmount);
                                                                s2.fsC_ConcessionType = opq.fsC_ConcessionType;
                                                                count += 1;
                                                                $scope.resultData1.push(s2);
                                                                opq.fscI_ConcessionAmount = (Number(opq.fscI_ConcessionAmount) - Number(s2.fscI_ConcessionAmount));
                                                            }
                                                            if (Number(opq.fscI_ConcessionAmount) == 0) {
                                                                keepGoing = false;
                                                                opq.fscI_ConcessionAmount = temp_concessionamount;
                                                            }
                                                        }

                                                    })
                                                }
                                                angular.forEach(s1.sp_ind_list, function (s2) {
                                                    var al_cnt = 0;
                                                    angular.forEach($scope.resultData1, function (del_m) {
                                                        if (del_m.fmG_Id == s2.fmG_Id && del_m.fmH_Id == s2.fmH_Id && del_m.ftI_Id == s2.ftI_Id && del_m.fmA_Id == s2.fmA_Id) {
                                                            al_cnt += 1;
                                                        }
                                                    })
                                                    if (al_cnt == 0 && Number(s2.fscI_ConcessionAmount) > 0) {
                                                        count += 1;
                                                        s2.fscI_ConcessionAmount = 0;
                                                        s2.fsC_ConcessionType = opq.fsC_ConcessionType;
                                                        $scope.resultData1.push(s2);
                                                    }
                                                })

                                            }
                                            else if (opq.fsC_ConcessionType == 'Percent') {
                                                angular.forEach(s1.sp_ind_list, function (s2) {
                                                    count += 1;
                                                    s2.fscI_ConcessionAmount = opq.fscI_ConcessionAmount;
                                                    s2.fsC_ConcessionType = opq.fsC_ConcessionType;
                                                    $scope.resultData1.push(s2);
                                                })
                                            }
                                        }

                                    })
                                }

                            })
                        }
                    }
                })
            }
            //MB For Special

            var amount = 0, saveflag = "Accept";
            for (var i = 0; i < $scope.feeheadgrid.length; i++) {
                var selectedval = feeheadgrid[i].isSelected

                if (selectedval == true) {
                    if (Number($scope.feeheadgrid[i].fscI_ConcessionAmount) == 0 || $scope.feeheadgrid[i].fsC_ConcessionType == null) {
                        saveflag = "save";
                    }
                }

            }





            if (saveflag != "save" && $scope.resultData.length > 0 && $scope.resultData1.length > 0) {

                    var data = {
                        "AMC_ID": $scope.AMC_ID,
                        "FMCC_Id": $scope.FMCC_Id,
                        "FMG_Id": $scope.FMG_Id,
                        savetmpdata: $scope.resultData,
                        savetmpdata1: $scope.resultData1,
                        "ASMAY_Id": $scope.ASMAY_Id
                    }
              

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("FeeConcessionNew/savedata", data).
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
             
                swal("Kindly Select Type and Enter Concession For Selected Records");
            }
            

        };

        $scope.Clearid = function () {
            $state.reload();
        }

        $scope.onstudentclick = function (groupcount, amstid, selectall) {
            

            if (selectall == undefined) {
                selectall = false;
            }

            $scope.disableconcessionamount = false;
            var gouportermcount = "0";
            for (var i = 0; i < groupcount.length; i++) {
                if (groupcount[i].selected == true) {
                    gouportermcount = gouportermcount + ',' + groupcount[i].fmG_Id;
                }
            }

            if (selectall == false) {
                angular.forEach($scope.studentsdata, function (yu) {
                    if (yu.amsT_Id == amstid && yu.studchecked == false) {
                        yu.studchecked = true;
                    }
                })
                angular.forEach($scope.studentsdata, function (yu) {
                    if (yu.amsT_Id != amstid) {
                        yu.studchecked = false;
                    }
                })
            }

            var data = {
                "AMST_Id": amstid,
                "multiplegroups": gouportermcount,
                "configset": grouporterm,
                "radiobtnvalue": $scope.checkboxval,
                "ASMAY_Id": $scope.ASMAY_Id

            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            $scope.fsC_ConcessionType = "Amout";
            var concessionamount, headid, insid, savedheadid, savedinsid, contype, remarks;

            apiService.create("FeeConcessionNew/fillamount", data).
                then(function (promise) {

                    $scope.saved_data_len = promise.savedcondatalist.length;

                    if (promise.fillheaddata.length > 0) {
                        if (promise.savedcondatalist != null) {
                            if (promise.savedcondatalist.length > 0) {
                                for (var i = 0; i < promise.savedcondatalist.length; i++) {

                                    savedheadid = promise.savedcondatalist[i].fmH_Id;
                                    savedinsid = promise.savedcondatalist[i].ftI_Id;
                                    concessionamount = promise.savedcondatalist[i].fscI_ConcessionAmount;
                                    contype = promise.savedcondatalist[i].fsC_ConcessionType;
                                    remarks = promise.savedcondatalist[i].fsC_ConcessionReason;

                                    for (var j = 0; j < promise.fillheaddata.length; j++) {

                                        headid = promise.fillheaddata[j].fmH_Id;
                                        insid = promise.fillheaddata[j].ftI_Id;

                                        if (savedheadid == headid && savedinsid == insid) {
                                            promise.fillheaddata[j].fscI_ConcessionAmount = concessionamount
                                            promise.fillheaddata[j].fsC_ConcessionType = contype
                                            promise.fillheaddata[j].fsC_ConcessionReason = remarks

                                            //promise.fillheaddata[j].isSelected = true;
                                            promise.fillheaddata[j].fmA_Amount += promise.fillheaddata[j].fscI_ConcessionAmount;
                                            if (promise.fillheaddata[j].fsC_ConcessionType == "Percent") {

                                                //promise.fillheaddata[j].fscI_ConcessionAmount = (promise.fillheaddata[j].fscI_ConcessionAmount / promise.fillheaddata[j].fmA_Amount) * 100;
                                                promise.fillheaddata[j].fscI_ConcessionAmount = Number($filter('number')(Number((promise.fillheaddata[j].fscI_ConcessionAmount / promise.fillheaddata[j].fmA_Amount) * 100), 0));

                                                //der.fscI_ConcessionAmount = (Number(der.fscI_ConcessionAmount) / 100) * der.fmA_Amount;
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        var nonzerolist = [];

                        angular.forEach(promise.fillheaddata, function (ie) {
                            if (ie.fmA_Amount >= 0) {
                                nonzerolist.push(ie);
                            }
                        })

                        angular.forEach(promise.fillheaddata, function (iee) {
                            if (iee.fmA_Amount > 0) {
                                iee.isSelected = true;
                            }
                        })


                        $scope.gridview2 = true;
                        $scope.btndiv = true;
                        //$scope.feeheadgrid = promise.fillheaddata;
                        $scope.feeheadgrid = nonzerolist;
                        console.log(nonzerolist);
                        //MB for Special
                        debugger;
                        $scope.temp_Head_Instl_list = [];
                        angular.forEach(nonzerolist, function (uy) {
                            uy.Head_Flag = 'H';
                            $scope.temp_Head_Instl_list.push(uy);
                        })
                        remove_list = [];
                        ins_spe_list = [];
                        angular.forEach(promise.instalspecial, function (ins) {
                            var special_list = [];
                            angular.forEach($scope.special_head_list, function (op1) {
                                var spe_ind_list = [];
                                angular.forEach($scope.special_head_details, function (op2) {
                                    if (op1.fmsfH_Id == op2.fmsfH_Id) {
                                        angular.forEach(nonzerolist, function (op_m) {
                                            if (op_m.fmH_Id == op2.fmH_ID && op_m.ftI_Id == ins.ftI_Id) {
                                                spe_ind_list.push(op_m);
                                                remove_list.push(op_m);
                                            }
                                        })
                                    }

                                })
                                if (spe_ind_list.length > 0) {
                                    special_list.push({ sp_id: op1.fmsfH_Id, sp_name: op1.fmsfH_Name, sp_ind_list: spe_ind_list });
                                }
                            })
                            if (special_list.length > 0) {
                                ins_spe_list.push({ ftI_Id: ins.ftI_Id, ftI_Name: ins.ftI_Name, sp_list: special_list });
                            }
                        })

                        if (ins_spe_list.length > 0) {
                            angular.forEach(remove_list, function (ma1) {
                                $scope.temp_Head_Instl_list.splice($scope.temp_Head_Instl_list.indexOf(ma1), 1);
                            })
                            angular.forEach(ins_spe_list, function (a1) {

                                angular.forEach(a1.sp_list, function (a2) {
                                    var isSelected = false;
                                    var fsC_ConcessionType = "";
                                    var fscI_ConcessionAmount = 0;
                                    var fsC_ConcessionReason = "";
                                    var fmA_Amount = 0;
                                    var fmG_Id = 0;
                                    // var fmG_GroupName = '';
                                    var not_cnt = 0;
                                    var totamtt = 0;
                                    angular.forEach(a2.sp_ind_list, function (a3) {
                                        if (fmG_Id == 0) {
                                            fmG_Id = a3.fmG_Id;
                                            // fmG_GroupName = a3.fmG_GroupName;
                                        }
                                        else {
                                            if (fmG_Id != a3.fmG_Id) {
                                                not_cnt += 1;
                                            }
                                        }

                                        fmA_Amount += a3.fmA_Amount;
                                        if (a3.fsC_ConcessionType == "Amount") {
                                            fscI_ConcessionAmount += a3.fscI_ConcessionAmount;
                                        }
                                       

                                        if (fsC_ConcessionType == "") {
                                            fsC_ConcessionType = a3.fsC_ConcessionType;
                                        }
                                        if (fsC_ConcessionReason == "") {
                                            fsC_ConcessionReason = a3.fsC_ConcessionReason;
                                        }

                                    })
                                    if (not_cnt == 0) {
                                        if (Number(fscI_ConcessionAmount) > 0) {
                                            isSelected = true;
                                        }
                                        $scope.temp_Head_Instl_list.push({ fmG_Id: fmG_Id, fmH_Id: a2.sp_id, fmH_FeeName: a2.sp_name, ftI_Id: a1.ftI_Id, ftI_Name: a1.ftI_Name, fmA_Amount: fmA_Amount, fscI_ConcessionAmount: fscI_ConcessionAmount, fsC_ConcessionType: fsC_ConcessionType, fsC_ConcessionReason: fsC_ConcessionReason, Head_Flag: 'SH', isSelected: isSelected });//fmG_GroupName: 'SH_' + fmG_GroupName,
                                    }
                                    else if (not_cnt > 0) {
                                        if (Number(fscI_ConcessionAmount) > 0) {
                                            isSelected = true;
                                        }
                                        $scope.temp_Head_Instl_list.push({ fmG_Id: 0, fmH_Id: a2.sp_id, fmH_FeeName: a2.sp_name, ftI_Id: a1.ftI_Id, ftI_Name: a1.ftI_Name, fmA_Amount: fmA_Amount, fscI_ConcessionAmount: fscI_ConcessionAmount, fsC_ConcessionType: fsC_ConcessionType, fsC_ConcessionReason: fsC_ConcessionReason, Head_Flag: 'SH', isSelected: isSelected });// fmG_GroupName: 'Special_Head',
                                    }

                                })
                            })
                            nonzerolist = $scope.temp_Head_Instl_list;
                            $scope.feeheadgrid = nonzerolist;
                            console.log(nonzerolist);
                        }
                      
                        if (nonzerolist.length == 0) {
                            swal("Selected Student has paid All Due amount");
                        }
                    }
                    else {
                        swal("Kindly Map Group");
                    }


                });
        }
    }


})();