(function () {
    'use strict';
    angular
.module('app')
        .controller('FeeMasterGroupwiseAutoReceipt', FeeMasterGroupwiseAutoReceipt)

    FeeMasterGroupwiseAutoReceipt.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$stateParams', '$compile']
    function FeeMasterGroupwiseAutoReceipt($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $stateParams,$compile) {

        $scope.searchValue = "";
        $scope.showdet = true;
        $scope.showbuttons = true;

        $scope.prefixshow = true;
        $scope.suffixshow = true;

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        $scope.savedisable = true;

        var pageid = $stateParams.pageId;
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.disabled = true;
                if (privlgs[i].ivrmirP_AddFlag == true) {
                    $scope.saveflg = true;
                    $scope.savebtn = true;

                }
                else {
                    $scope.saveflg = false;
                    $scope.savebtn = false;
                }
                if (privlgs[i].ivrmirP_UpdateFlag == true) {
                    $scope.editflag = true;
                    $scope.editbtn = true;

                }
                else {
                    $scope.editflag = false;
                    $scope.editbtn = false;
                }
                if (privlgs[i].ivrmirP_DeleteFlag == true) {
                    $scope.deactiveflag = true;
                    $scope.deletebtn = true;
                }
                else {
                    $scope.deactiveflag = false;
                    $scope.deletebtn = false;
                }


            }
        }
        $scope.onload = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            var data = {
                "MI_Id": $scope.Id,
            }
            apiService.create("FeeMasterGroupwiseAutoReceipt/getdetails", data).
            then(function (promise) {
                $scope.arrlist6 = promise.acayear;

                $scope.ASMAY_Id = academicyrlst[0].asmaY_Id;

                $scope.arrlistchk = promise.fillgroup;

                if (promise.filldata.length > 0) {
                    $scope.students = promise.filldata;
                    $scope.presentCountgrid = promise.filldata.length;
                }
                else {
                    swal("No Records Found");
                    $scope.presentCountgrid = 0;
                }
            })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        };


        $scope.selectacademicyear = function () {

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeMasterGroupwiseAutoReceipt/getacademicyear", data).
                then(function (promise) {

                    $scope.arrlistchk = promise.fillgroup;

                    if (promise.filldata.length > 0) {
                        $scope.students = promise.filldata;
                        $scope.presentCountgrid = promise.filldata.length;
                    }
                    else {
                        swal("No Records Found");
                        $scope.presentCountgrid = 0;
                    }
                })
        };


        $scope.delete = function (det, SweetAlert) {

            var data = {
                //"FGARG_Id": det.fgarG_Id
                "FGAR_Id": det.fgaR_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            }

            swal({
                title: "Are you sure?",
                text: "All groups related to this receipt format will be deleted!!Do you want to Proceed?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.create("FeeMasterGroupwiseAutoReceipt/delete/", data).
                    then(function (promise) {
                        
                        if (promise.returnvalue == "2") {
                            swal("Sorry.....You can not delete this record.Because it is already mapped");
                            return;
                        }
                        else if (promise.returnvalue === "1") {
                            swal('Record Deleted Successfully');
                            $state.reload();
                        }
                        else if (promise.returnvalue === "3") {
                            swal('Receipt is already generated for this Format!!So Record cant be deleted.');
                        }
                    })
                }
                else {
                    swal("Cancelled");
                }
            });
        }
        $scope.edit = function (det) {

            var data={
                "FGAR_Id":det.fgaR_Id
            }

            $scope.prefixnme = true;
            $scope.suffixnme = true;

            $scope.academicnme = true;

            $scope.Id = det.fgarG_Id;
           // fgaR_Id
            apiService.create("FeeMasterGroupwiseAutoReceipt/edit/", data).
            then(function (promise) {
                $scope.ASMAY_Id = promise.filldata[0].asmaY_Id;

                if (promise.filldata[0].fgaR_PrefixFlag == 1)
                {
                    $scope.FGAR_PrefixFlag = true;
                    $scope.FGAR_PrefixName = promise.filldata[0].fgaR_PrefixName;
                    $scope.FGAR_Template_Name = promise.filldata[0].fgaR_Template_Name;

                    $scope.predisable = false;
                    $scope.prefixnme = false;
                }
                else
                {
                    $scope.FGAR_PrefixFlag = false;
                    $scope.FGAR_PrefixName = "";
                    $scope.predisable = false;
                    $scope.prefixnme = false;
                }

                if (promise.filldata[0].fgaR_SuffixFlag == 1) {
                    $scope.FGAR_SuffixFlag = true;
                    $scope.FGAR_SuffixName = promise.filldata[0].fgaR_SuffixName;
                    $scope.sufdisable = false;
                    $scope.suffixnme = false;
                }
                else
                {
                    $scope.FGAR_SuffixFlag = false;
                    $scope.FGAR_SuffixName = "";
                    $scope.sufdisable = false;
                    $scope.suffixnme = false;
                }

                $scope.FGAR_Starting_No = promise.filldata[0].fgaR_Starting_No;
                $scope.FGAR_Name = promise.filldata[0].fgaR_Name;
                $scope.FGAR_Address = promise.filldata[0].fgaR_Address;

                $scope.FMG_Id = promise.filldata[0].fmG_Id;
                $scope.fgaR_Id = promise.filldata[0].fgaR_Id;
                $scope.arrlistchk = [];
                $scope.arrlistchk=promise.fillgroup;
                angular.forEach($scope.arrlistchk, function (group) {
                    angular.forEach(promise.filldata, function (group1) {
                        if (group.fmG_Id === group1.fmG_Id) {
                            group.selected = true;
                        }
                    })
                })
            })
        }

        $scope.submitted = false;
        $scope.save = function (selected, arrlistchk) {

            if ($scope.myForm.$valid) {

                if (arrlistchk.length > 0)
                {
                    $scope.tempararyArrayList = [];
                    angular.forEach($scope.arrlistchk, function (comp) {
                        if (comp.selected == true) {
                            $scope.tempararyArrayList.push({ FMG_Id: comp.fmG_Id, FMGG_GroupName: comp.fmG_GroupName });
                        }
                    });
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "FGAR_PrefixFlag": $scope.FGAR_PrefixFlag,
                        "FGAR_PrefixName": $scope.FGAR_PrefixName,
                        "FGAR_SuffixFlag": $scope.FGAR_SuffixFlag,
                        "FGAR_SuffixName": $scope.FGAR_SuffixName,
                        "FGAR_Name": $scope.FGAR_Name,
                        "FGAR_Address": $scope.FGAR_Address,
                        "savegroup": $scope.tempararyArrayList,
                        //"FGAR_Id": $scope.Id,
                        "FGAR_Id": $scope.fgaR_Id,
                        "FGAR_Starting_No": $scope.FGAR_Starting_No,
                        "FGAR_Template_Name": $scope.FGAR_Template_Name
                    }

                    apiService.create("FeeMasterGroupwiseAutoReceipt/savedata/", data)

                    .then(function (promise) {
                        if (promise.returnvaluetwo != null && promise.returnvaluetwo != "") {
                            swal('Some Record Already Exist', "These Group name" + promise.returnvalue + " Already Group name Exist Record Updated");
                        }
                        if (promise.returnvalue == "true") {
                            swal('Record Saved Successfully');
                            
                        }
                        else if (promise.returnvalue == "false") {
                            swal('Record Saving Failed');
                          
                        }
                        else if (promise.returnvalue == "updated") {
                            swal('Record Updated Successfully');
                           
                        }
                        else if (promise.returnvalue == "updatefailed") {
                            swal('Record Update Failed');
                           
                        }
                        else if (promise.returnvalue == "duplicate") {
                            swal('Record Already Exists');
                        }
                        else if (promise.returnvalue != null && promise.returnvalue != "")
                        {
                            swal('Some Record Already Exist', "These Group name " + promise.returnvalue + " Duplicate");
                        }
                        else {
                            swal('Operation Failed');
                        }
                        $state.reload();
                    })
                }
                else
                {
                    swal("Kindly select any one group!!!")
                }
              
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.clearid = function () {
            $state.reload();

        }
        $scope.interacted = function (field) {
            return $scope.submitted
        };

        $scope.sufdisable = true;
        $scope.predisable = true;

        $scope.changeprefix = function (FGAR_PrefixFlag) {
            if(FGAR_PrefixFlag==false)
            {
                $scope.predisable = true;
                $scope.FGAR_PrefixName = "";
            }
            else if (FGAR_PrefixFlag == true) {
                $scope.predisable = false;
                $scope.FGAR_PrefixName = "";
            }
        };      
        $scope.changesuffix = function (FGAR_SuffixFlag) {
            if (FGAR_SuffixFlag == false) {
                $scope.sufdisable = true;
                $scope.FGAR_SuffixName = "";
            }
            else if (FGAR_SuffixFlag == true) {
                $scope.sufdisable = false;
                $scope.FGAR_SuffixName = "";
            }
        };
        //extra
        $scope.all_checkCCCC = function () {
            var fmG_Id = $scope.fmgg_Id;
            var count = 0;
            angular.forEach($scope.arrlistchk, function (itm) {
                itm.selected = fmG_Id;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        };
        $scope.togchkbxCCCC = function () {
            $scope.fmG_Id = $scope.arrlistchk.every(function (options) {
                return options.selected;
            });
        };
        $scope.isOptionsRequiredtwo = function () {
            return !$scope.arrlistchk.some(function (options) {
                return options.selected;
            });
        };

        $scope.groupvalidate = function () {
            $scope.tempararyArrayList = [];
            angular.forEach($scope.arrlistchk, function (comp) {
                if (comp.selected == true) {
                    $scope.tempararyArrayList.push({ FMG_Id: comp.fmG_Id, FMGG_GroupName: comp.fmG_GroupName });
                }
            });

        }
        $scope.showmodalgroup = function (fgaR_Id) {

            var data = {
                "FGAR_Id": fgaR_Id,
                "ASMAY_Id": $scope.ASMAY_Id

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeMasterGroupwiseAutoReceipt/get_groupdetails", data).
                then(function (promise) {
                    $scope.get_grpDetail = promise.get_grpDetail;


                })
        }

        $scope.showmodaldetails = function (id) {
      
            var data = {
                "FGAR_Id": id,
                
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeMasterGroupwiseAutoReceipt/printreceipt", data).
                then(function (promise) {
                    $scope.htmldata = promise.htmldata;
                    var e1 = angular.element(document.getElementById("test"));
                    $compile(e1.html($scope.htmldata))(($scope));
                })
                    
        }



    }
})();
