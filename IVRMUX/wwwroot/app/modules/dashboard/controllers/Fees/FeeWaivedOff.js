(function () {
    'use strict';
    angular
.module('app')
.controller('FeeWaivedOffController', FeeWaivedOffController)
    FeeWaivedOffController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce']
    function FeeWaivedOffController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce) {
        //$rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce
        $scope.filterdata1 = "Refunable";
        $scope.sortKey = "fswO_Id";
        $scope.reverse = true;
        $scope.checkvalue = false;
        $scope.finewav = false;
        $scope.totcountsearch = 0;
        $scope.FSWO_Date = new Date();
        var paginationformasters;

        $scope.completedisablefinewaivedoff = true;

        $scope.UploadFile = [];

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        
          var totwaivedoffamount = 0;
        $scope.toggleAll = function (allchkdata) {
            var toggleStatus = $scope.selectedAll;
            angular.forEach($scope.headlst, function (itm) {
                itm.studchecked = toggleStatus;
            });
             if (allchkdata == true) {
                $scope.fswO_WaivedOffRemarks = "Total : " + totwaivedoffamount;
            }
            else if (allchkdata == false) {
                $scope.fswO_WaivedOffRemarks = "";
            }
        }

        $scope.cfg = {};
        /* loading start*/
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("FeeWaivedOff/getalldetails", pageid).
            then(function (promise) {
                                              
               // $scope.FSWO_Date = new Date();
                $scope.asmayiddisable = false;
                $scope.grigview1 = false;
                $scope.yearlst = promise.fillyear;

                $scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;

                $scope.studentlst = promise.fillstudent;
                $scope.grouplst = promise.fillgroup;
                $scope.classlst = promise.fillclass;
                $scope.sectionlst = promise.fillsection;
                $scope.thirdgrid = promise.filldata;
                $scope.totcountfirst = promise.filldata.length;
              //  $scope.AMST_Id = promise.fillstudent[0].amsT_Id;
               // $scope.searchByColumn(1);
                //$scope.thirdgrid = promise.filldata;
                //for (var i = 0; i < $scope.studentlst.length; i++) {
                //    name = $scope.studentlst[i].amsT_Id;
                //    if (name == promise.amsT_Id) {
                //        $scope.studentlst[i].Selected = true;
                //        $scope.AMST_Id = promise.amsT_Id;
                //    }
                //}
            })
        }
        /* loading end*/


        $scope.completeonselectfine = function (chkfine) {
            if (chkfine == true) {
                for (var i = 0; i < $scope.gridview1; i++) {
                    $scope.disablefinecompete = true
                }
            }
            else {
                for (var i = 0; i < $scope.gridview1; i++) {
                    $scope.disablefinecompete = false
                }
            }
            
        }

        $scope.onselectfine = function () {
            if ($scope.finewav == true) {
                $scope.completedisablefinewaivedoff = false;
            }
            else if ($scope.finewav == false) {
                $scope.completedisablefinewaivedoff = true;
            }
        }


        /* Acamic start*/
        $scope.onselectyear = function () {
            
            if ($scope.ASMAY_Id !== "") {
                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "ASMCL_Id": $scope.clsdrp,
                    "ASMS_Id": $scope.sectiondrp,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeWaivedOff/getstudent", data).
                    then(function (promise) {
                        $scope.gridview1 = false;
                        $scope.AMST_Id = "";
                        $scope.studentlst = promise.fillstudent; 

                        if (promise.filldata.length > 0) {
                              
                            $scope.thirdgrid = promise.filldata;
                            $scope.totcountfirst = promise.filldata.length;
                        }
                        else {
                            //swal("No Records Found");
                        }
                               
               });
            }
                       
        }
        //classs end 
        //start student
        $scope.onselectstudent = function () {
            if ($scope.ASMAY_Id !== "" && $scope.AMST_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "AMST_Id": $scope.AMST_Id,
                    "filterrefund": $scope.filterdata1,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeWaivedOff/getgroup", data).
               then(function (promise) {
                   $scope.gridview1 = false;
                   $scope.grouplst = promise.fillgroup;
               });
            }
           
        }
        //end student
        //start group  
        $scope.optionToggledGF = function () {


            var fineflagval = 0;
            if ($scope.finewav == true) {
                fineflagval = 1;
                $scope.checkvalue = true;
            }
            else if ($scope.finewav == false) {
                fineflagval = 0;
                $scope.checkvalue = false;
            }

            var groupidss;
            for (var i = 0; i < $scope.grouplst.length; i++) {
                
                if ($scope.grouplst[i].selected == true) {

                    if (groupidss == undefined)
                        groupidss = $scope.grouplst[i].fmG_Id;
                    else
                        groupidss = groupidss + "," + $scope.grouplst[i].fmG_Id;
                }


            }
            if (groupidss != undefined) {
                
                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "AMST_Id": $scope.AMST_Id,
                    "filterrefund": $scope.filterdata1,
                    "multiplegroup": groupidss,
                    "finewaiveoff": fineflagval
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("FeeWaivedOff/gethead", data).
                then(function (promise) {
                    
                    if (promise.fillhead.length > 0)
                    {
                        $scope.gridview1 = true;
                        $scope.headlst = promise.fillhead;
                    }
                    else
                    { $scope.gridview1 = false; }
                   
                });
            } else { $scope.gridview1 = false; }
            
        }
        //end group  
        //validation start 
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //validation  
        /* post data start*/

        $scope.submitted = false;
        $scope.savedata = function (headlst) {
                       
            if ($scope.myForm.$valid) {
                var fineflagval = 0,completefinewai=0;
                if ($scope.finewav == true) {
                    fineflagval = 1;
                }
                else if ($scope.finewav == false) {
                    fineflagval = 0;
                }

                if ($scope.completefinewav == true) {
                    completefinewai = 1;
                }
                else if ($scope.completefinewav == false) {
                    completefinewai = 0;
                }


                    $scope.albumNameArray2 = [];
                    angular.forEach($scope.grouplst, function (role) {
                        if (!!role.selected) $scope.albumNameArray2.push(role);
                    })               
                    $scope.albumNameArray1 = [];
                    angular.forEach($scope.headlst, function (role) {
                        if (!!role.studchecked) $scope.albumNameArray1.push(role);
                    })
                    if ($scope.albumNameArray2.length === 0) {
                        swal('Please Select atleast one Group')
                    }
                    else if ($scope.albumNameArray1.length === 0) {
                        swal('Please Select atleast one Head')
                    }
                    else
                    {
                        $scope.albumNameArraynewone = [];
                        var a = "";
                       
                        for (var j = 0; j < $scope.headlst.length; j++) {
                            

                            if ($scope.headlst[j].studchecked == true) {

                                //if ((headlst[j].fswO_WaivedOffAmount == 0 && completefinewai != 1) || headlst[j].fswO_WaivedOffAmount == null || headlst[j].fswO_WaivedOffAmount == "") {
                                if ((headlst[j].fswO_WaivedOffAmount == 0 && completefinewai != 1) || (headlst[j].fswO_WaivedOffAmount == 0 && fineflagval != 1)) {
                                        a = "s";
                                        break;
                                    }
                                    //else if (headlst[j].fswO_WaivedOffAmount > headlst[j].tobepaid) {
                                    //    a = "b";

                                    //    break;
                                    //}
                             
                                    else {
                                        $scope.albumNameArraynewone.push(headlst[j]);
                                    }
                            }
                            else
                            {
                                if (headlst[j].fswO_WaivedOffAmount > 0)
                                { a = "c";
                                    break;}
                               
                            }
                         
                        }
                        if (a == "s") {
                            swal('Please Enter the Waived Off Amount for the selected checkbox')
                        }
                        //else if (a == "b") {
                        //    swal('Waived Off Should be lesser than or equal to Balance')
                        //}
                        else if (a == "c") {
                            swal('Please select the check box for the entered amount')
                        }
                        else
                        {
                            $scope.FSWO_Date = new Date($scope.FSWO_Date).toDateString();
                            var data = {                                
                                "FSWO_Id":$scope.fswO_Id,
                                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                                "AMST_Id": $scope.AMST_Id,
                                "FSWO_Date": $scope.FSWO_Date,
                                "headlist": $scope.albumNameArraynewone,
                                "finewaiveoff": fineflagval,
                                "completefinewaiveoff": completefinewai,
                                "FSWO_WaivedOffRemarks": $scope.fswO_WaivedOffRemarks,
                                "filepath": $scope.filepath,
                                "filename": $scope.file_detail
                            }

                            var config = {
                                headers: {
                                    'Content-Type': 'application/json;'
                                }
                            }
                            if ($scope.fswO_Id > 0)
                            {
                                var disfun = "Update";
                            }
                            else
                            {
                                var disfun = "Save";
                            }
                            
                            swal({
                                title: "Are you sure?",
                                text: "Do You Want To " + disfun + " Record? ",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes," + disfun + " it",
                                cancelButtonText: "Cancel",
                                closeOnConfirm: false,
                                closeOnCancel: false,
                                showLoaderOnConfirm: true,

                            },
      function (isConfirm) {
          if (isConfirm) {


              apiService.create("FeeWaivedOff/savedata", data).
                            then(function (promise) {
                                
                                if (promise.returnduplicatestatus === "Saved" || promise.returnduplicatestatus === "Updated") {
                                    // $scope.thirdgrid = promise.filldata;
                                    swal('Record ' + promise.returnduplicatestatus + ' Successfully');
                                    $state.reload();
                                    $scope.loaddata();
                                }
                                else if (promise.returnduplicatestatus === "adjusted") {

                                    swal('Record already adjusted/Refunded');
                                    $state.reload();
                                    $scope.loaddata();
                                }
                                else {
                                    swal('Record ' + promise.returnduplicatestatus + ' Successfully');
                                }
                            })
              //$scope.asmayiddisable = false;
              //$scope.amstiddisable = false;
              //$scope.groupdisable = false;
              //$scope.headdisable = false;
              //  $scope.loaddata();
          }
          else {
              swal("Record saved Failed", "Failed");
          }


      });
                           
                        }
                    }
            }
            else { $scope.submitted = true; }
        };
        /* post data end*/

        $scope.cance = function () {
            
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();
            //$scope.submitted = false;
            //$scope.ASMAY_Id = "";
            //$scope.AMST_Id = "";
            //$scope.grouplst = "";
            //$scope.gridview1 = false;
            $state.reload();
        };
        $scope.DeletRecord = function (employee, SweetAlert) {
            
            $scope.editEmployee = employee.fswO_Id;
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
                    apiService.DeleteURI("FeeWaivedOff/Deletedetails", feechequebounceid).
                    then(function (promise) {
                        
                        if (promise.returnduplicatestatus == "success") {

                            //$scope.thirdgrid = promise.filldata;
                           // $scope.totcountfirst = promise.filldata.length;
                            swal('Record Deleted Successfully');
                            $state.reload();
                            $scope.loaddata();
                        }
                        else if (promise.returnduplicatestatus === "adjusted") {
                           
                            swal('Record already adjusted/Refunded');
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
               // $scope.cance();

            });

            //})
        }
        //edit start
        $scope.disablefinewaivedoff = false;
        $scope.editdata = function (employee) {
            
            $scope.editEmployee = employee.fswO_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("FeeWaivedOff/getdetails", pageid).
            then(function (promise) {

                $scope.editdata = promise.editamount.length;

                $scope.studentlst = promise.fillstudent;

                $scope.fswO_Id = promise.fswO_Id;
                $scope.ASMAY_Id = promise.asmaY_Id;               
                $scope.AMST_Id = promise.amsT_Id;
                $scope.clsdrp = promise.asmcL_Id;
                $scope.sectiondrp = promise.asmS_Id;
                //$scope.FSWO_Date = promise.fswO_Date;
                $scope.FSWO_Date = new Date(promise.fswO_Date);
               // $scope.user.fswO_WaivedOffAmount = promise.fswO_WaivedOffAmount;
                $scope.asmayiddisable = true;              
                $scope.grouplst = promise.fillgroup;
                $scope.grouplst[0].selected =true;
                $scope.headlst = promise.fillhead;
                $scope.headlst[0].studchecked = true;

                //if (promise.completefinewaiveoff == 1) {

                    //for (var i = 0; i < $scope.headlst.length; i++) {
                    //        $scope.disablefinecompete = true
                    //    }

                    if (promise.editamount.length > 0) {
                        $scope.disablefinecompete = true;
                    }
                    else {
                        $scope.disablefinecompete = false;
                    }
                //}

                $scope.headlst[0].fswO_WaivedOffAmount = promise.fswO_WaivedOffAmount;
                $scope.gridview1 = true; 


                $scope.fswO_WaivedOffRemarks = promise.fswO_WaivedOffRemarks;

                $scope.disablefinewaivedoff = true;

                if (promise.finewaiveoff == 1) {
                    $scope.finewav = true;
                }
                else if (promise.finewaiveoff == 0){
                    $scope.finewav = false;
                }

                $scope.completedisablefinewaivedoff = true;

                if (promise.completefinewaiveoff == 1) {
                    $scope.completefinewav = true;
                }
                else if (promise.completefinewaiveoff == 0) {
                    $scope.completefinewav = false;
                }

                var img = promise.filepath;
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];

                $scope.filetype = lastelement;

                if ($scope.filetype == "jpeg" || $scope.filetype == "jpg") {
                    $scope.filetype = "image/jpeg";
                }
                else if ($scope.filetype == "pdf") {
                    $scope.filetype = "application/pdf";
                }

                $scope.file_detail = promise.filename;
                $scope.filepath = promise.filepath;

                $scope.intB_FilePath = promise.filepath;

            })
        }
        //edit end
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

                if ($scope.search123 == "3") {
                    $scope.txt = false;
                    $scope.numbr = true;
                    $scope.dat = false;

                }
                else if ($scope.search123 == "4") {

                    $scope.txt = false;
                    $scope.numbr = false;
                    $scope.dat = true;

                }
                else {
                    $scope.txt = true;
                    $scope.numbr = false;
                    $scope.dat = false;

                }
                $scope.searchtxt = "";
                $scope.searchdat = "";
                $scope.searchnumbr = "";

            }
        }
        $scope.ShowSearch_Report = function () {
            
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            if ($scope.searchtxt != "" || $scope.searchnumbr != "" || $scope.searchdat != "") {
                if ($scope.search123 == "3") {
                    var data = {
                        "searchType": $scope.search123,
                        "searchnumber": $scope.searchnumbr
                    }
                }
                else if ($scope.search123 == "4") {
                    
                   
                    var date = new Date($scope.searchdat).toDateString("dd/MM/yyyy");
                    var data = {
                        "searchType": $scope.search123,
                        "searchdate": date
                    }
                }
                else {
                   
                    var data = {
                        "searchType": $scope.search123,
                        "searchtext": $scope.searchtxt
                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeWaivedOff/searching", data).
            then(function (promise) {
                $scope.thirdgrid = promise.filldata;
                $scope.totcountsearch = promise.filldata.length;
                if (promise.filldata == null || promise.filldata == "") {
                    swal("Record Does Not Exist For Searched Data !!!!!!")
                }
            })
            }
            else {
                swal("Data Is Needed For Search ");
            }
        }
        $scope.clearsearch = function () {
            $state.reload();
            $scope.search123 = "";
            $scope.search_flag = false;
            $scope.searchtxt = "";
            $scope.searchnumbr = "";
            $scope.searchdat = "";
            $scope.totcountsearch = 0;
        }
        //search end

         //-----------upload file/photo.............
        $scope.uploadFile = function (input, document) {

            $scope.UploadFile = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" || input.files[0].type == "image/png" || input.files[0].type == "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.ms-excel" &&
                    input.files[0].size <= 2097152) {

                    var reader = new FileReader();

                    reader.onload = function (e) {

                        $('#blahD')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        }
        function Uploadprofile() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadFile.length; i++) {

                formData.append("File", $scope.UploadFile[i]);
                $scope.file_detail = $scope.UploadFile[0].name;
                $scope.filetype = $scope.UploadFile[0].type;
            }
            //We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Waviedofffiles", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    $scope.filepath = d[0];
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }
        //----------End..........!
        $scope.showmodaldetails = function () {

            $('#preview').attr('src', $scope.filepath);
        }

        $scope.remove_file = function () {

            $scope.file_detail = "";
            $scope.filepath = "";
        }

    }
})();







