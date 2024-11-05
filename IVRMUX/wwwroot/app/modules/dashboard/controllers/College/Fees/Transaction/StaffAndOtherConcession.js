(function () {
    'use strict';
    angular
        .module('app')
        .controller('StaffAndOtherConcessionController', StaffAndOtherConcessionController)

    StaffAndOtherConcessionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function StaffAndOtherConcessionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

        $scope.classacademicdrp = true;
        $scope.stulstdis = true;
        $scope.btndiv = false;
        $scope.search = "";

        $scope.disabletrms = true;

        $scope.selectall = false;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (configsettings.length > 0) {
            var grouporterm = configsettings[0].fmC_GroupOrTermFlg;
        }

        if (grouporterm == "T") {
            $scope.grouportername = "Term Name";
        }
        else if (grouporterm == "G") {
            $scope.grouportername = "Group Name";
        }

        var selcount = 0;
        $scope.onclickofconcession = function (feeheadgrid, userdata) {

            for (var i = 0; i < feeheadgrid.length; i++) {
                var selectedval = feeheadgrid[i].isSelected
                if (selectedval == true) {
                    selcount = Number(selcount) + 1;
                }
            }

            if (Number(selcount) >= 1) {
                $scope.btndiv = true;
            }
            else {
                $scope.btndiv = false;
            }
        }

        $scope.toggleAll = function (allchkdata) {
            var toggleStatus = $scope.selectedAll;
            angular.forEach($scope.feeheadgrid, function (itm) {
                itm.isSelected = toggleStatus;
            });
        }

        $scope.toggleAllstu = function (allchkdatastu, groupcount) {

            if (allchkdatastu == true) {
                var toggleStatusstu = $scope.selectedAllstu;
                angular.forEach($scope.studentsdata, function (itm) {
                    itm.studchecked = toggleStatusstu;
                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "FMCC_Id": $scope.FMCC_Id,
                    "radiobtnvalue": $scope.checkboxval,
                }

                apiService.create("StaffAndOtherConcession/checkpaiddetails", data).
                    then(function (promise) {
                        if (promise.studentdata.length === 0) {
                            $scope.onstudentclick(groupcount, $scope.studentsdata[0].amsT_Id, allchkdatastu);
                   
                        }
                        else {
                            angular.forEach($scope.studentsdata, function (itm) {
                                itm.studchecked = false;
                            });

                            $scope.selectedAllstu = false;
                            swal("Already amount is paid for the selected no of student!!So this feature is not avaliable");
                        }
                    })
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


        $scope.resultData = [];
        $scope.resultData1 = [];


        $scope.page1 = "page1";
        $scope.reverse1 = true;

        $scope.page2 = "page2";
        $scope.reverse2 = true;

        $scope.page3 = "page3";
        $scope.reverse3 = true;

        $scope.page4 = "page4";
        $scope.reverse4 = true;

        $scope.loaddata = function () {
            $scope.checkboxval = "Classwise";
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

            $scope.currentPage4 = 1;
            $scope.itemsPerPage4 = 5

            var data = {
                "configset": grouporterm,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("StaffAndOtherConcession/getalldetails", data).
                then(function (promise) {

           
                    $scope.groupcount = promise.fillgroup;

                    $scope.academiyrcnt = promise.fillyear;
                    $scope.ASMAY_Id = academicyrlst[0].asmaY_Id;

               

            
     

                  

                 //   $scope.FMC_EableStaffTrans = promise.configsetting[0].fmC_EableStaffTrans;
                   // $scope.FMC_EableOtherStudentTrans = promise.configsetting[0].fmC_EableOtherStudentTrans;

                    $scope.staffdata = promise.stafflist;
                    $scope.othersdata = promise.otherlist;

                    $scope.thirdgridstaff = promise.staffdata;

                    $scope.thirdgridothers = promise.othersdata;

                })

            $scope.sort = function (keyname) {
                debugger;
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

            $scope.sort1 = function (keyname1) {

                $scope.sortKey1 = keyname1;   //set the sortKey to the param passed
                $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
            }

            $scope.sort2 = function (keyname2) {
                debugger;
                $scope.sortKey2 = keyname2;   //set the sortKey to the param passed
                $scope.reverse2 = !$scope.reverse2; //if true make it false and vice versa
            }

            $scope.sort3 = function (keyname3) {
                debugger;
                $scope.sortKey3 = keyname3;   //set the sortKey to the param passed
                $scope.reverse3 = !$scope.reverse3; //if true make it false and vice versa
            }

            $scope.sort4 = function (keyname4) {
                debugger;
                $scope.sortKey4 = keyname4;   //set the sortKey to the param passed
                $scope.reverse4 = !$scope.reverse4; //if true make it false and vice versa
            }

        };

        $scope.onselectfeecategory = function (feecatid) {

        }

        $scope.maindiv = false;

        $scope.onclickloaddata = function (groupcount) {

            if (groupcount != undefined) {
                for (var i = 0; i < groupcount.length; i++) {
                    if (groupcount[i].selected == true) {
                        groupcount[i].selected = false
                    }
                }
            }

            if ($scope.checkboxval == "Classwise") {
                $scope.catdiv = false;
                $scope.classdiv = true;
                $scope.groupdiv = false
                $scope.maindiv = true;
                // $scope.btndiv = true;

                $scope.staffgrid = false;
                $scope.Othergrid = false;

                $scope.gridview1 = false;
                $scope.gridview2 = false;

                $scope.feecatdiv = false;

            }
            else if ($scope.checkboxval == "categorywise") {
                $scope.catdiv = true;
                $scope.classdiv = false;
                $scope.groupdiv = false;
                $scope.maindiv = true;
                $scope.staffgrid = false;
                $scope.Othergrid = false;
                // $scope.btndiv = true;

                $scope.gridview1 = false;
                $scope.gridview2 = false;

                $scope.feecatdiv = false;
            }

            else if ($scope.checkboxval == "feecategorywise") {

                $scope.catdiv = false;
                $scope.classdiv = false;
                $scope.groupdiv = false;
                $scope.maindiv = true;
                $scope.staffgrid = false;
                $scope.Othergrid = false;
                // $scope.btndiv = true;

                $scope.gridview1 = false;
                $scope.gridview2 = false;

                $scope.feecatdiv = true;

            }

            else if ($scope.checkboxval == "Staff") {

                $scope.classdiv = false;
                $scope.groupdiv = false;
                $scope.catdiv = false;
                $scope.groupdiv = false
                $scope.maindiv = true;

                $scope.gridview1 = false;
                $scope.groupdiv = true;

                $scope.gridview2 = false;
                $scope.staffgrid = true;
                $scope.Othergrid = false;

                $scope.feecatdiv = false;
            }

            else if ($scope.checkboxval == "Others") {

                $scope.classdiv = false;
                $scope.groupdiv = false;
                $scope.catdiv = false;
                $scope.groupdiv = false
                $scope.maindiv = true;

                $scope.gridview1 = false;
                $scope.groupdiv = true;

                $scope.gridview2 = false;
                $scope.staffgrid = false;

                $scope.Othergrid = true;

                $scope.feecatdiv = false;
            }

        };


        $scope.angularData = {
            'nameList': []
        };
        $scope.vals = [];
        $scope.gridview1 = false;
        $scope.onselectcategory = function (catid, classid) {

            $scope.groupdiv = true;

            if (classid != "") {
                if ($scope.checkboxval == "categorywise") {
                    var data = {
                        "AMC_ID": catid,
                        "radiobtnvalue": $scope.checkboxval,
                        "configset": grouporterm,
                        "ASMAY_Id": $scope.ASMAY_Id
                    }
                }
                else if ($scope.checkboxval == "Classwise") {
                    var data = {
                        "ASMCL_Id": classid,
                        "radiobtnvalue": $scope.checkboxval,
                        "configset": grouporterm,
                        "ASMAY_Id": $scope.ASMAY_Id
                    }
                }

                if ($scope.checkboxval == "feecategorywise") {
                    var data = {
                        "FMCC_Id": catid,
                        "radiobtnvalue": $scope.checkboxval,
                        "configset": grouporterm,
                        "ASMAY_Id": $scope.ASMAY_Id
                    }
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("StaffAndOtherConcession/onselectclassorcat", data).
                    then(function (promise) {

                        if (promise.studentdata.length > 0) {
                            $scope.gridview1 = true;
                            $scope.studentsdata = promise.studentdata;
                            //angular.forEach($scope.studentsdata, function (objj) {
                            //    objj.fullname = objj.amsT_FirstName + " " + objj.amsT_MiddleName + " " + objj.amsT_LastName;
                            //})
                            for (var i = 0; i < promise.studentdata.length; i++) {
                                var name = promise.studentdata[i].amsT_FirstName;
                                if (promise.studentdata[i].amsT_MiddleName !== null) {
                                    name += " " + promise.studentdata[i].amsT_MiddleName;
                                }
                                if (promise.studentdata[i].amsT_LastName != null) {
                                    name += " " + promise.studentdata[i].amsT_LastName;
                                }
                                $scope.vals.push(name);
                            }
                            angular.forEach($scope.vals, function (v, k) {
                                $scope.angularData.nameList.push({
                                    'fullname': v
                                });
                            });

                            var j = 0;
                            angular.forEach($scope.studentsdata, function (obj) {
                                //Using bracket notation
                                obj["fullname"] = $scope.angularData.nameList[j].fullname;
                                j++;
                            });

                        }
                        else {
                            $scope.gridview1 = false;
                            swal("No Records Found!!!!")
                        }

                    });
            } else {
                $scope.gridview1 = false;
                $scope.gridview2 = false;
            }

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
                // if (option.fmG_Id != "")
                $scope.stulstdis = false;

                for (var i = 0; i < studentsdata.length; i++) {
                    if (studentsdata[i].studchecked == true) {
                        amstidstud = studentsdata[i].amsT_Id;
                    }
                }


                if ($scope.checkboxval == "categorywise") {
                    var data = {
                        "AMC_ID": $scope.AMC_ID,
                        "radiobtnvalue": $scope.checkboxval,
                        "FMG_Id": option.fmG_Id,
                        "configset": grouporterm,
                        "AMST_Id": amstidstud,
                        FMG_Ids: FMG_Ids,
                        "ASMAY_Id": $scope.ASMAY_Id
                    }
                }
                else if ($scope.checkboxval == "Classwise") {
                    var data = {
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "radiobtnvalue": $scope.checkboxval,
                        "FMG_Id": option.fmG_Id,
                        "configset": grouporterm,
                        "AMST_Id": amstidstud,
                        FMG_Ids: FMG_Ids,
                        "ASMAY_Id": $scope.ASMAY_Id
                    }
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                var concessionamount, headid, insid, savedheadid, savedinsid, contype, remarks;
                apiService.create("StaffAndOtherConcession/fillhead", data).
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
                                                promise.fillheaddata[j].fscI_ConcessionAmount = concessionamount;
                                                promise.fillheaddata[j].fsC_ConcessionType = contype;
                                                promise.fillheaddata[j].fsC_ConcessionReason = remarks;

                                                promise.fillheaddata[j].isSelected = true;
                                                promise.fillheaddata[j].fmA_Amount += promise.fillheaddata[j].fscI_ConcessionAmount;
                                             
                                                if (promise.fillheaddata[j].fsC_ConcessionType == "Percent") {
                                                  
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
                $scope.stulstdis = true;
                $scope.feeheadgrid = [];
            }

        };

        $scope.Clearid = function () {
            $state.reload();
        }

        $scope.onselectstudent = function (studentid) {

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMST_Id": studentid
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("StaffAndOtherConcession/onselectstudent", data).
                then(function (promise) {
                    $scope.Net_amount = promise.netamount;
                })
        };


        $scope.onselecthead = function (headid) {

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "FMH_Id": headid
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("StaffAndOtherConcession/onselecthead", data).
                then(function (promise) {
                    $scope.Net_amount = promise.netamount;
                })
        };

        var remove_list = [];
        var ins_spe_list = [];
     

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

           

            var data = {
                "AMCST_Id": amstid,
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

            var concessionamount, headid, insid, savedheadid, savedinsid, contype, remarks;

            apiService.create("StaffAndOtherConcession/fillamount", data).
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

                                          
                                            promise.fillheaddata[j].fmA_Amount += promise.fillheaddata[j].fscI_ConcessionAmount;
                                            if (promise.fillheaddata[j].fsC_ConcessionType == "Percent") {
                                            promise.fillheaddata[j].fscI_ConcessionAmount = Number($filter('number')(Number((promise.fillheaddata[j].fscI_ConcessionAmount / promise.fillheaddata[j].fmA_Amount) * 100), 0));

                                               
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
                    
                        $scope.feeheadgrid = nonzerolist;
                        console.log(nonzerolist);
  
                     
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
                                        else if (a3.fsC_ConcessionType == "Percent") {
                                            fscI_ConcessionAmount = a3.fscI_ConcessionAmount;
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
                        //MB for Special
                        if (nonzerolist.length == 0) {
                            swal("Selected Student has paid All Due amount");
                        }
                    }
                    else {
                        swal("Kindly Map Group");
                    }

                });
        }

        $scope.cleardata = function () {
            $scope.Amst_Id = "";
            $scope.FMG_Id = "";
            $scope.REF_DATE = "";
            $scope.REF_REMARKS = "";
            $scope.REF_BANK_CASH = "";
            $scope.REF_CheqDate = "";
            $scope.REF_CheqNo = "";
            $scope.REF_REC_No = "";
            $scope.REF_BANK_NAME = "";
            $scope.L_Code = "";
            $scope.ASMCL_Id = "";
            $scope.ASMAY_Id = "";
            $scope.gridview1 = false;
        }

        $scope.DeletRecord = function (employee, SweetAlert) {
            debugger;
            $scope.editEmployee = employee.fmR_ID;
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
                        apiService.DeleteURI("StaffAndOtherConcession/Deletedetails", feechequebounceid).
                            then(function (promise) {

                                $scope.thirdgrid = promise.fillthirdgriddata;
                                swal(promise.validationvalue);
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });

            //})
        }


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

            apiService.create("StaffAndOtherConcession/EditconcessionDetails", data).
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

        $scope.onselectmodeofpayment = function () {

            var data = {
                "REF_BANK_CASH": $scope.REF_BANK_CASH,
                "filterinitialdata": $scope.filterdata,
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("StaffAndOtherConcession/modeofpayment", data).
                then(function (promise) {

                    if ($scope.REF_BANK_CASH == 'C' || $scope.REF_BANK_CASH == 'B') {
                        $scope.accountlst = promise.fillacclst;
                    }

                })
        };


        $scope.reF_AMOUNT = true;
        $scope.balanceamt = function (students, index) {
        
            {
                $scope.Bal_AMOUNT = Number(students[index].reF_AMOUNT) - Number(students[index].enteredamt)
            }

        }

        $scope.edit = function (employee) {
            $scope.editEmployee = employee.fmR_ID;
            var templId = $scope.editEmployee;

            apiService.getURI("StaffAndOtherConcession/editdetails", templId).
                then(function (promise) {

                    $scope.gridview1 = true;

                    $scope.ASMAY_Id = promise.fillthirdgriddata[0].asmaY_Id;

                    $scope.students = promise.fillthirdgriddata;

                    $scope.enteredamt = promise.fillthirdgriddata[0].reF_AMOUNT;

                    $scope.Bal_AMOUNT = true;
                    $scope.Bal_AMOUNT = Number(promise.fillthirdgriddata[0].reF_AMOUNT) - Number($scope.enteredamt)

                    $scope.ASMCL_Id = promise.fillthirdgriddata[0].asmcL_ID;
                    $scope.Amst_Id = promise.fillthirdgriddata[0].amsT_ID;
                    $scope.FMG_Id = promise.fillthirdgriddata[0].fmG_ID;

                    $scope.REF_BANK_CASH = promise.fillthirdgriddata[0].reF_BANK_CASH;
                    $scope.reF_AMOUNT = promise.fillthirdgriddata[0].reF_AMOUNT;
                    $scope.REF_BANK_NAME = promise.fillthirdgriddata[0].reF_BANK_NAME;
                    $scope.REF_CheqDate = promise.fillthirdgriddata[0].reF_CheqDate;
                    $scope.REF_CheqNo = promise.fillthirdgriddata[0].reF_CheqNo;
                    $scope.REF_DATE = promise.fillthirdgriddata[0].reF_DATE;
                    $scope.REF_REC_No = promise.fillthirdgriddata[0].reF_REC_No;
                    $scope.REF_REMARKS = promise.fillthirdgriddata[0].reF_REMARKS;
                })
        }

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.submitted = false;

        $scope.savedata = function (studentsdata, feeheadgrid, staffdata, othersdata) {

            if ($scope.checkboxval != "Staff" && $scope.checkboxval != "Others") {
                $scope.resultData = [];
                angular.forEach(studentsdata, function (student) {
                    if (student.studchecked == true) {
                        $scope.resultData.push(student);
                    }
                });
            }
            else if ($scope.checkboxval == "Staff") {
                $scope.resultData = [];
                angular.forEach(staffdata, function (student) {
                    if (student.staffchecked == true) {
                        $scope.resultData.push(student);
                    }
                });
            }

            else if ($scope.checkboxval == "Others") {
                $scope.resultData = [];
                angular.forEach(othersdata, function (student) {
                    if (student.otherschecked == true) {
                        $scope.resultData.push(student);
                    }
                });
            }


            $scope.resultData1 = [];
           
            var count = 0;
            var temp_concessionamount = 0;
            if (ins_spe_list.length == 0 && remove_list.length == 0) {
                angular.forEach(feeheadgrid, function (opq) {
                    if (opq.isSelected) {
                        count += 1;
                    
                        $scope.resultData1.push(opq);
                    }
                })
            }
            else if (ins_spe_list.length > 0 && remove_list.length > 0) {
                angular.forEach(feeheadgrid, function (opq) {
                    if (opq.isSelected) {
                        if (opq.Head_Flag == 'H') {
                            count += 1;
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

            var amount = 0, saveflag = "Accept";
            for (var i = 0; i < feeheadgrid.length; i++) {
                var selectedval = feeheadgrid[i].isSelected

                if (selectedval == true) {
                    if (Number(feeheadgrid[i].fscI_ConcessionAmount) == 0 || feeheadgrid[i].fsC_ConcessionType == null) {
                        saveflag = "save";
                    }
                }

            
            }





            if (saveflag != "save" && $scope.resultData.length > 0 && $scope.resultData1.length > 0) {
                if ($scope.resultData1.length > 0 && saveflag != "save") {
                    angular.forEach($scope.resultData1, function (der) {
                        if (der.fsC_ConcessionType == "Percent") {
                            der.fscI_ConcessionAmount = (Number(der.fscI_ConcessionAmount) / 100) * der.fmA_Amount;
                        }
                    })
                }
                if ($scope.checkboxval == "categorywise") {
                    var data = {
                        "AMC_ID": $scope.AMC_ID,
                        "radiobtnvalue": $scope.checkboxval,
                        "FMG_Id": $scope.FMG_Id,
                        savetmpdata: $scope.resultData,
                        savetmpdata1: $scope.resultData1,
                        "ASMAY_Id": $scope.ASMAY_Id
                    }
                }
                else if ($scope.checkboxval == "Classwise") {
                    var data = {
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "radiobtnvalue": $scope.checkboxval,
                        "FMG_Id": $scope.FMG_Id,
                        savetmpdata: $scope.resultData,
                        savetmpdata1: $scope.resultData1,
                        "ASMAY_Id": $scope.ASMAY_Id
                    }
                }

                else if ($scope.checkboxval == "Staff" || $scope.checkboxval == "Others") {
                    var data = {
                        "radiobtnvalue": $scope.checkboxval,
                        "FMG_Id": $scope.FMG_Id,
                        savetmpdata: $scope.resultData,
                        savetmpdata1: $scope.resultData1,
                        "ASMAY_Id": $scope.ASMAY_Id
                    }
                }

                if ($scope.checkboxval == "feecategorywise") {
                    var data = {
                        "FMCC_Id": $scope.AMC_ID,
                        "radiobtnvalue": $scope.checkboxval,
                        "FMG_Id": $scope.FMG_Id,
                        savetmpdata: $scope.resultData,
                        savetmpdata1: $scope.resultData1,
                        "ASMAY_Id": $scope.ASMAY_Id
                    }
                }

                if ($scope.checkboxval == "Classwise") {
                    if (studentsdata[0].fscI_ID > 0) {
                        var data = {
                            "FMCC_Id": $scope.AMC_ID,
                            "radiobtnvalue": $scope.checkboxval,
                            "FMG_Id": $scope.FMG_Id,
                            savetmpdata: $scope.resultData,
                            savetmpdata1: $scope.resultData1,
                            "ASMAY_Id": $scope.ASMAY_Id,
                            "FSCI_ID": studentsdata[0].fscI_ID,
                            "FSC_ID": studentsdata[0].fsC_Id
                        }
                    }
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("StaffAndOtherConcession/savedata", data).
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


        $scope.concessionamount = function (feeheadgrid, index) {
            debugger;
            if (feeheadgrid[index].fsC_ConcessionType == "Amount") {
                if (Number(feeheadgrid[index].fscI_ConcessionAmount) > Number(feeheadgrid[index].fmA_Amount)) {
                    swal("Enter Amount cannot be greater than Net Amount");
                    feeheadgrid[index].fscI_ConcessionAmount = 0;
                }
            }
            else if (feeheadgrid[index].fsC_ConcessionType == "Percent") {
               
                if (Number(feeheadgrid[index].fscI_ConcessionAmount) > 100) {
                    swal("Percentage Value Not More Than 100");
                    feeheadgrid[index].fscI_ConcessionAmount = 0;
                }
            
            }


        };
        $scope.clear_amount = function (feeheadgrid, index) {
            feeheadgrid[index].fscI_ConcessionAmount = 0;
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
                        apiService.create("StaffAndOtherConcession/Deletedetails", data).
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

        }

        $scope.onselectacademic = function (yearlst) {

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

         

            apiService.create("StaffAndOtherConcession/getacademicyear", data).
                then(function (promise) {

                    $scope.thirdgrid = promise.studentdata;

                    $scope.groupdiv = false;
                    angular.forEach($scope.groupcount, function (ty) {
                        ty.selected = false;
                    })

                 
                    $scope.ASMCL_Id = "";
                    $scope.gridview1 = false;
                    $scope.gridview2 = false;

                })
        };

    }


})();