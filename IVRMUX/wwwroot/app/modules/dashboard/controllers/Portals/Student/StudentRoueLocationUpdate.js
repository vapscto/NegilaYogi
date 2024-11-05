(function () {
    'use strict';
    angular
.module('app')
.controller('StudentRoueLocationUpdateController', StudentRoueLocationUpdateController)

    StudentRoueLocationUpdateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile']
    function StudentRoueLocationUpdateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile) {



        $scope.abc = true;
        $scope.abcd = true;
        $scope.obj = {};
        $scope.pdlocation = false;
        $scope.modl = false;
        var HostName = location.host;
        $scope.locationdrop = true;
        $scope.locationpick = true;
        $scope.showarea = false;
        $scope.accchange = true;
        $scope.acc = true;
        $scope.ASMAY_Iddisble = false;



        $scope.onloadmodal = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;

            apiService.getDATA("StudentRoueLocationUpdate/getloaddataintruction").
                 then(function (promise) {
                     $scope.yearlst = promise.currfillyear;
                     $scope.routeDetlsty = promise.routeDetails;
                     if (promise.logoheader.length > 0) {

                         $scope.imgname = promise.logoheader[0].logopath;
                     }
                     else {
                         $scope.imgname = logopath;
                     }
                 })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        };
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgnameee = logopath;

        $scope.onclickloaddata = function () {
            if ($scope.PASR_UndertakingFlag == 1) {
                $scope.abc = false;
                $scope.abcd = true;
            }
            else {
                $scope.abc = true;
                $scope.abcd = false;
            }
        }

        $scope.gotohome = function () {
            $scope.modl = true;
            $('#myModalswal').modal('hide');


            //   $window.location.href = 'http://' + HostName + '/#/app/homepage';
        }



        $scope.UploadStudentProfilePic = [];

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

            for (var i = 0; i <= $scope.uploadStudentProfilePic.length; i++) {
                formData.append("File", $scope.UploadStudentProfilePic[i]);
            }

            //We can send more data to server using append         
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadprofilepic", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
            .success(function (d) {

                defer.resolve(d);
                // swal(d);
                $scope.obj.image = d;
            })
            .error(function () {
                defer.reject("File Upload Failed!");
            });
            // Uploads1(miid);
        }

        var searchObject = $location.search();
        //*****MAXMINAGE****
        //$scope.classdesable = true;

        // alert(searchObject.status);
        if (searchObject.status == "failure") {
            swal("Payment Unsuccessfull");
            //  Request.QueryString.Remove("status");
            //$location.url($location.path)
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "success") {
            swal("Thankyou for filling up form");
            //Request.QueryString.Remove("status");
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "Networkfailure") {
            swal('Network failure..!!', 'Try again after some time');
            //Request.QueryString.Remove("status");
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        $scope.maultiway_rdo = false;
        $scope.pdloca = false;
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            
            apiService.getDATA("StudentRoueLocationUpdate/getloaddata").
                then(function (promise) {

                    if (promise.logoheader.length > 0) {
                        $scope.imgname = promise.logoheader[0].logopath;
                    }
                    else {
                        $scope.imgname = logopath;
                    }

                    
                    // $scope.studentDetails = promise.studentDetails;
                    //$scope.areaLst = promise.areaList;
                    //if (promise.approvenot != null) {
                    //    if (promise.approvenot == 'Approved') {
                    //        $scope.showview = 1;
                    //    }
                    //    else {
                    //        $scope.showview = 0;
                    //    }
                    //}
                    //else {
                    //    $scope.showview = 0;
                    //}




                    $scope.routeDetls = promise.routeDetails;
                    $scope.stu_name = promise.stu_name;
                    $scope.prospectusPaymentlist = promise.prospectusPaymentlist;

                    $scope.prospectusPaymentlist = promise.prospectusPaymentlist;
                    if ($scope.prospectusPaymentlist.length > 0) {
                        for (var i = 0; i < $scope.routeDetls.length; i++) {
                            for (var j = 0; j < $scope.prospectusPaymentlist.length; j++) {
                                if ($scope.routeDetls[i].astA_Id == $scope.prospectusPaymentlist[j].astA_Id) {

                                    $scope.routeDetls[i].viewflag = true;
                                    $scope.routeDetls[i].download = false;
                                    if ($scope.routeDetls[i].astA_ApplStatus == 'Approved') {
                                        $scope.routeDetls[i].showview = 1;
                                    }
                                    else {
                                        $scope.routeDetls[i].showview = 0;
                                    }
                                    break;
                                }
                                else if ($scope.routeDetls[i].astA_Id != $scope.prospectusPaymentlist[j].astA_Id) {
                                    $scope.routeDetls[i].viewflag = false;
                                    $scope.routeDetls[i].download = true;

                                }
                            }
                        }
                        console.log($scope.pages);
                    }
                    else {
                        for (var j = 0; j < $scope.routeDetls.length; j++) {
                            $scope.routeDetls[j].viewflag = false;
                            $scope.routeDetls[j].download = true;
                        }
                    }
                })
        };

        $scope.ProspectuseScreen = true;
        var configsettings = JSON.parse(localStorage.getItem("configsettings"));

        $scope.configurationsettings = configsettings[i];

        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag == "OnlineRegular") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }

        var RegistrationNumbering;
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                $scope.transnumbconfigurationsettings = transnumconfigsettings[i];

                if (transnumconfigsettings[i].imN_Flag == "BusForm") {
                    RegistrationNumbering = transnumconfigsettings[i];
                }
            }
        }



        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        //-----student name selection change
        $scope.onchange = function (studentlst) {

            var studid = studentlst.amsT_Id;

            $scope.acc = true;
            $scope.accyer = $scope.ASMAY_Id
            
            var data = {
                "AMST_Id": studid,
                "ASMAY_Id": $scope.accyer
            }
            apiService.create("StudentRoueLocationUpdate/getstudata", data).
               then(function (promise) {
                   
                   //if (promise.trnsportcutoffdate == 'True') {
                       $scope.areaLst = promise.areaList;
                       //$scope.routeList = promise.routelist;
                       //$scope.locaList = promise.locationlist;


                       $scope.studentDetails = promise.studentDetails;


                       $scope.obj.amsT_FirstName = $scope.studentDetails[0].amsT_FirstName;
                       $scope.obj.asmcL_ClassName = $scope.studentDetails[0].asmcL_ClassName,
                       $scope.obj.asmC_SectionName = $scope.studentDetails[0].asmC_SectionName,
                        $scope.obj.amsT_BloodGroup = $scope.studentDetails[0].amsT_BloodGroup;
                       $scope.mark = $scope.studentDetails[0].astA_Landmark;
                       $scope.obj.asmaY_Year = $scope.studentDetails[0].asmaY_Year,
                       $scope.obj.amsT_AdmNo = $scope.studentDetails[0].amsT_AdmNo,
                       $scope.obj.amsT_DOB = $scope.studentDetails[0].amsT_DOB,
                       $scope.obj.amsT_emailId = $scope.studentDetails[0].amsT_emailId,
                       $scope.obj.amsT_MobileNo = $scope.studentDetails[0].amsT_MobileNo,
                       $scope.obj.amsT_PerStreet = $scope.studentDetails[0].amsT_PerStreet,
                       $scope.obj.amsT_PerCity = $scope.studentDetails[0].amsT_PerCity,
                       $scope.obj.amsT_PerArea = $scope.studentDetails[0].amsT_PerArea,
                       $scope.obj.amsT_PerPincode = $scope.studentDetails[0].amsT_PerPincode,
                       $scope.obj.ivrmmC_CountryName = $scope.studentDetails[0].IVRMMC_CountryName,
                       $scope.obj.ivrmmS_Name = $scope.studentDetails[0].IVRMMS_Name,
                       $scope.obj.amsT_FatherName = $scope.studentDetails[0].amsT_FatherName,
                       $scope.obj.amsT_MotherName = $scope.studentDetails[0].amsT_MotherName,
                       $scope.obj.amsT_FatherMobleNo = $scope.studentDetails[0].amsT_FatherMobleNo,
                       $scope.obj.amsT_MotherMobileNo = $scope.studentDetails[0].amsT_MotherMobileNo,
                       $scope.obj.amsT_FatherOfficeAdd = $scope.studentDetails[0].amsT_FatherOfficeAdd,
                       $scope.obj.amsT_BloodGroup = $scope.studentDetails[0].amsT_BloodGroup
                       $scope.obj.amsT_Officephon = $scope.studentDetails[0].astA_Phoneoff,
                     $scope.obj.amsT_Resphon = $scope.studentDetails[0].astA_PhoneRes,
                        $scope.mark = $scope.studentDetails[0].astA_Landmark,
                        $scope.obj.image = $scope.studentDetails[0].amsT_Photoname,
                       $scope.trmA_Id = $scope.studentDetails[0].trmA_Id,
                        $('#blah').attr('src', $scope.studentDetails[0].amsT_Photoname);

                       $scope.profile_photo = 1;

                       $scope.astA_AreaZoneName = $scope.studentDetails[0].astA_AreaZoneName,
                      $scope.obj.astA_PickUp_TRML_Id = $scope.studentDetails[0].astA_PickUp_TRML_Id,
                      $scope.obj.astA_Drop_TRML_Id = $scope.studentDetails[0].astA_Drop_TRML_Id,
                      $scope.obj.astA_PickUp_TRMR_Id = $scope.studentDetails[0].astA_PickUp_TRMR_Id
                       $scope.obj.astA_Drop_TRMR_Id = $scope.studentDetails[0].astA_Drop_TRMR_Id


                       if (($scope.astA_AreaZoneName.length > 0 && $scope.astA_AreaZoneName != "") && ($scope.obj.astA_PickUp_TRML_Id > 0 && $scope.obj.astA_PickUp_TRMR_Id > 0 && $scope.obj.astA_PickUp_TRML_Id != "" && $scope.obj.astA_PickUp_TRMR_Id != "") || ($scope.obj.astA_Drop_TRML_Id != "" && $scope.obj.astA_Drop_TRML_Id > 0 && $scope.obj.astA_Drop_TRMR_Id > 0 && $scope.obj.astA_Drop_TRMR_Id != "")) {
                           $scope.maultiway_rdo = true;
                           $scope.pdloca = true;
                       }
                       else {
                           $scope.maultiway_rdo = false;
                           $scope.pdloca = false;
                       }

                       $scope.studentpercountryl = promise.countryid

                       for (var t = 0; t < $scope.studentDetails.length; t++) {
                           if ($scope.studentDetails[t].ivrmmC_CountryName == $scope.studentpercountryl[0].ivrmmC_CountryName) {
                               $scope.ivrmmC_Id = $scope.studentpercountryl[t].ivrmmC_Id;
                           }
                       }

                       //$scope.studentconstatel = promise.studentconstate
                       //for (var s = 0; s < $scope.studentDetails.length; s++) {
                       //    if ($scope.studentDetails[s].ivrmmS_Name == $scope.studentconstatel[0].ivrmmS_Name) {
                       $scope.ivrmmS_Id = $scope.studentDetails[0].ivrmmS_Id;
                       //    }
                       //}
                       //angular.forEach($scope.areaLst, function (y) {
                       //    if (y.trmA_AreaName == $scope.astA_AreaZoneName) {
                       //        $scope.trmA_Id = y.trmA_Id;

                       //    }
                       //})

                       $scope.onareachange($scope.trmA_Id);
                       $scope.onroutechange($scope.trmA_Id);
                       $scope.onroutechangedrop($scope.trmA_Id);

                       //$scope.yearid = $scope.studentDetails[0].astA_FutureAY;
                       //$scope.ASMAY = $scope.studentDetails[0].astA_FutureAY;
                       $scope.ASMAY_Iddisble = true;


                       $scope.routeDetlsss = promise.routeDetails;

                       if ($scope.studentDetails.length > 0) {
                           for (var i = 0; i < $scope.routeDetlsss.length; i++) {
                               for (var j = 0; j < $scope.studentDetails.length; j++) {
                                   if ($scope.routeDetlsss[i].astA_Id == $scope.studentDetails[j].astA_Id) {
                                       $scope.trmL_Idp = $scope.routeDetlsss[i].astA_PickUp_TRML_Id;
                                       $scope.trmL_Idd = $scope.routeDetlsss[i].astA_Drop_TRML_Id;
                                       $scope.trmR_Idp = $scope.routeDetlsss[i].trmR_Idp;
                                       $scope.trmR_Idd = $scope.routeDetlsss[i].trmR_Idd;
                                       break;
                                   }
                               }
                           }
                       }

                       if ($scope.trmR_Idp > 0 && $scope.trmL_Idp > 0 && $scope.trmR_Idd > 0 && $scope.trmL_Idd > 0 && $scope.trmR_Idp != "" && $scope.trmL_Idp != "" && $scope.trmR_Idd != "" && $scope.trmL_Idd != "") {
                           $scope.multiway = "two";
                           $scope.plocation = true;
                           $scope.pickd = false;
                           $scope.dlocation = true;
                           $scope.dropd = false;

                       }
                       else {
                           $scope.multiway = "one";
                           if ($scope.trmR_Idp > 0 && $scope.trmL_Idp > 0 && $scope.trmR_Idp != "" && $scope.trmL_Idp != "") {
                               $scope.plocation = true;
                               $scope.pickd = false;
                               $scope.dropd = true;
                               $scope.trmL_Idd = "";
                               $scope.trmR_Idd = "";
                           }
                           if ($scope.trmR_Idd > 0 && $scope.trmL_Idd > 0 && $scope.trmR_Idd != "" && $scope.trmL_Idd != "") {
                               $scope.dlocation = true;
                               $scope.dropd = false;
                               $scope.pickd = true;
                               $scope.trmR_Idp = "";
                               $scope.trmL_Idp = "";
                           }
                       }
                   //}
                   //else {
                   //    swal('Now You Are Not Eligible to Fill Application !!');
                   //}

               })
        }

        //----- on onform click
        $scope.printbusspassbbhs = function () {
            
            var innerContents = document.getElementById("BBHSBUSSFORM").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

           '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BBHS/BBHSBUSSFORM/BBHSBUSSFORMPdf.css" />' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.BGHSAPP = function () {
            
            var innerContents = document.getElementById("BGHSAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                       '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BGHS/BGHSAPPPdf.css" />' +
                   '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                 '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.onformclick = function (astA_Id, amsT_Id) {
            
            var data = {
                "AMST_Id": amsT_Id,
                "ASTA_Id": astA_Id
            }
            apiService.create("StudentRoueLocationUpdate/getbuspassdata", data).
               then(function (promise) {
                   //
                   //$scope.buspassdatalst = promise.buspassdatalist;
                   //$scope.obj.amsT_FirstName = $scope.buspassdatalst[0].stuname;
                   //$scope.obj.amsT_FatherName = $scope.buspassdatalst[0].AMST_FatherName;
                   //$scope.obj.asmcL_ClassName = $scope.buspassdatalst[0].ASMCL_ClassName; 
                   //$scope.obj.amsT_BloodGroup = $scope.buspassdatalst[0].AMST_BloodGroup;
                   //$scope.obj.trmR_RouteName = $scope.buspassdatalst[0].PickUp_Route;
                   //$scope.obj.trmR_RouteNamedrop = $scope.buspassdatalst[0].Drop_Route;
                   //$scope.obj.PickUp_Location = $scope.buspassdatalst[0].PickUp_Location;
                   //$scope.obj.trmL_LocationName = $scope.buspassdatalst[0].TRML_LocationName;
                   //$scope.obj.amsT_FatherMobleNo = $scope.buspassdatalst[0].AMST_FatherMobleNo;

                   //$scope.obj.amsT_MotherMobileNo = $scope.buspassdatalst[0].AMST_MotherMobileNo;
                   //$scope.obj.amsT_emailId = $scope.buspassdatalst[0].AMST_emailId;
                   ////------------Address
                   //$scope.obj.amsT_PerStreet = $scope.buspassdatalst[0].AMST_PerStreet;
                   //$scope.obj.amsT_PerArea = $scope.buspassdatalst[0].AMST_PerArea;
                   //$scope.obj.amsT_PerCity = $scope.buspassdatalst[0].AMST_PerCity;
                   //$scope.obj.ivrmmS_Name = $scope.buspassdatalst[0].IVRMMS_Name;
                   //$scope.obj.ivrmmC_CountryName = $scope.buspassdatalst[0].IVRMMC_CountryName;
                   //$scope.obj.amsT_PerPincode = $scope.buspassdatalst[0].AMST_PerPincode;

                   $scope.getdate = new Date();
                   $scope.buspassdatalst = promise.buspassdatalist;
                   $scope.appno = $scope.buspassdatalst[0].appno;
                   $('#blahnew').attr('src', $scope.buspassdatalst[0].AMST_Photoname);
                   $scope.pickuprouteno = $scope.buspassdatalst[0].pickuprouteno;
                   $scope.ASMAY_Year = $scope.buspassdatalst[0].ASMAY_Year;
                   $scope.AMST_AdmNo = $scope.buspassdatalst[0].AMST_AdmNo;
                   $scope.ASTA_Landmark = $scope.buspassdatalst[0].ASTA_Landmark;
                   $scope.amsT_FirstName = $scope.buspassdatalst[0].stuname;
                   $scope.amsT_FatherName = $scope.buspassdatalst[0].AMST_FatherName;
                   $scope.asmcL_ClassName = $scope.buspassdatalst[0].ASMCL_ClassName;
                   $scope.amsT_BloodGroup = $scope.buspassdatalst[0].AMST_BloodGroup;
                   $scope.trmR_RouteName = $scope.buspassdatalst[0].PickUp_Route;
                   $scope.trmR_RouteName_no = $scope.buspassdatalst[0].PickUp_Route_no;
                   $scope.PickUp_Location = $scope.buspassdatalst[0].PickUp_Location;
                   $scope.fuyear = $scope.buspassdatalst[0].fuyear;

                   $scope.Drop_Route = $scope.buspassdatalst[0].Drop_Route;
                   $scope.Drop_Route_no = $scope.buspassdatalst[0].Drop_Route_no;
                   $scope.DropUp_Location = $scope.buspassdatalst[0].DropUp_Location;

                   $scope.amsT_FatherMobleNo = $scope.buspassdatalst[0].ASTA_FatherMobileNo;

                   $scope.amsT_MotherMobileNo = $scope.buspassdatalst[0].ASTA_MotherMobileNo;
                   $scope.amsT_emailId = $scope.buspassdatalst[0].AMST_emailId;
                   //------------Address
                   $scope.amsT_PerStreet = $scope.buspassdatalst[0].AMST_PerStreet;
                   $scope.amsT_PerArea = $scope.buspassdatalst[0].AMST_PerArea;
                   $scope.amsT_PerCity = $scope.buspassdatalst[0].AMST_PerCity;
                   $scope.ivrmmS_Name = $scope.buspassdatalst[0].IVRMMS_Name;
                   $scope.ivrmmC_CountryName = $scope.buspassdatalst[0].IVRMMC_CountryName;
                   $scope.amsT_PerPincode = $scope.buspassdatalst[0].AMST_PerPincode;
                   $scope.ASTA_Regnew = $scope.buspassdatalst[0].ASTA_Regnew;
                   $scope.amsT_Office = $scope.buspassdatalst[0].ASTA_Phoneoff;
                   $scope.amsT_Res = $scope.buspassdatalst[0].ASTA_PhoneRes;
                   $scope.getdate = $scope.buspassdatalst[0].ASTA_ApplicationDate;

                   var e1 = angular.element(document.getElementById("test"));
                   $compile(e1.html(promise.htmldata))(($scope));

                   $('#blahnew').attr('src', $scope.buspassdatalst[0].AMST_Photoname);
                   $('#blahnewF').attr('src', $scope.buspassdatalst[0].ANST_FatherPhoto);
                   $('#blahnewM').attr('src', $scope.buspassdatalst[0].ANST_MotherPhoto);
                   debugger;

               })
        }




        $scope.onchangeacc = function (trmA_Id) {
            
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("StudentRoueLocationUpdate/getbuspassdataupdate", data).
              then(function (promise) {
                  $scope.studentlst = promise.fillstudent;
                  if (promise.stu_name.length>0)
                  {
                      $scope.accchange = false;
                       $scope.stu_name = promise.stu_name;
                  }
                  else {
                      
                      swal('No Records Found!!');
                      $scope.clear();
                  }
              
              })
        }

        //--Area selection change 
        $scope.onareachange = function (trmA_Id) {
            
            var data = {
                "pasaA_Id": $scope.pasaA_Id,
            }
            apiService.create("StudentRoueLocationUpdate/getroutedata", data).
               then(function (promise) {
                   

                   $scope.routelst = promise.routelist;
                   if ($scope.routelst.length != 0) {
                       $scope.maultiway_rdo = true;
                   }
                   else {

                       $scope.maultiway_rdo = false;
                       $scope.pdloca = false;
                   }

               })
        }

        //--Route selection change 
        $scope.onroutechange = function (trmA_Id) {
            
            var data = {
                "pasaA_Id": $scope.pasaA_Id,
            }
            apiService.create("StudentRoueLocationUpdate/getlocationdata", data).
               then(function (promise) {
                   
                   $scope.locationlst = promise.locationlist;

               })
        }

        $scope.onroutechangedrop = function (trmA_Id) {
            
            var data = {
                "pasaA_Id": $scope.pasaA_Id,
            }
            apiService.create("StudentRoueLocationUpdate/getlocationdata", data).
               then(function (promise) {
                   
                   $scope.locationlstdrop = promise.locationlist;

               })
        }
        //--------one or two way radio change

        //--loadRoute selection change 
        $scope.onroutechangeload = function (trmA_Id) {
            
            var data = {
                "TRMR_Id": trmA_Id,

            }
            apiService.create("StudentRoueLocationUpdate/getlocationdataonly", data).
               then(function (promise) {
                   
                   $scope.locationlst = promise.locationlist;
                   $scope.locationpick = false;

               })
        }

        $scope.onroutechangedropload = function (trmA_Id) {
            
            var data = {
                "TRMR_Id": trmA_Id,

            }
            apiService.create("StudentRoueLocationUpdate/getlocationdataonly", data).
               then(function (promise) {
                   
                   $scope.locationlstdrop = promise.locationlist;
                   $scope.locationdrop = false;
               })
        }




        $scope.onclickoneway = function () {
            $scope.pdloca = true;
            if ($scope.multiway == "two") {
                $scope.dropd = false;
                $scope.pickd = false;
                $scope.plocation = false;
                $scope.dlocation = false;
            }
            else if ($scope.multiway == "one") {
                $scope.dropd = false;
                $scope.pickd = false;
                $scope.plocation = false;
                $scope.dlocation = false;
            }
            $scope.trmL_Idd = "";
            $scope.trmR_Idd = "";
            $scope.trmR_Idp = "";
            $scope.trmL_Idp = "";
        }

        $scope.testonwy = function (objj) {
            
            if ($scope.multiway == "one") {
                if ($scope.plocation == true) {
                    $scope.dropd = true;
                } else {
                    $scope.dropd = false;
                }
                if ($scope.dlocation == true) {
                    $scope.pickd = true;
                } else {
                    $scope.pickd = false;
                }
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.trmL_Idd = "";
                $scope.trmR_Idd = "";
                $scope.trmR_Idp = "";
                $scope.trmL_Idp = "";
            }

            else if ($scope.multiway == "two") {
                $scope.dropd = false;
                $scope.pickd = false;
            }
        }

        //---------SAVE         
        $scope.submitted = false;
        $scope.savedata = function () {
            
            var areaname = "";
            if ($scope.myForm.$valid) {

                if ($scope.multiway == null || $scope.multiway == undefined) {
                    swal("Select Transport Type");
                    return;
                }

                if ($scope.multiway == 'one') {
                    if ($scope.plocation == true || $scope.dlocation == true) {

                    }
                    else {
                        swal("Select at least one route");
                        return;
                    }
                }
                if ($scope.multiway == 'two') {
                    if ($scope.plocation == true && $scope.dlocation == true) {

                    }
                    else {
                        swal("Select route details");
                        return;
                    }
                }
                angular.forEach($scope.areaLst, function (y) {
                    if (y.trmA_Id == $scope.trmA_Id) {
                        areaname = y.trmA_AreaName
                    }
                })
                if ($scope.plocation && !$scope.dlocation) {
                    $scope.trmL_Idd = 0;
                    $scope.trmR_Idd = 0;
                }
                else if (!$scope.plocation && $scope.dlocation) {
                    $scope.trmR_Idp = 0;
                    $scope.trmL_Idp = 0;
                }

                var data = {
                    "AMST_Id": $scope.Amst_Id.amsT_Id,
                    "studentid": $scope.Amst_Id.amsT_Id,
                    "TRMA_Id": $scope.trmA_Id,
                    "TRMR_Idp": $scope.trmR_Idp,
                    "TRMR_Idd": $scope.trmR_Idd,
                    "TRML_Idp": $scope.trmL_Idp,
                    "TRML_Idd": $scope.trmL_Idd,
                    "TRMA_AreaName": areaname,
                    "ASTA_Landmark": $scope.mark,
                    "ASTA_FatherMobileNo": $scope.obj.amsT_FatherMobleNo,
                    "ASTA_MotherMobileNo": $scope.obj.amsT_MotherMobileNo,
                    "ASTA_Phoneoff": $scope.obj.amsT_Officephon,
                    "ASTA_PhoneRes": $scope.obj.amsT_Resphon,
                    "AMST_Photoname": $scope.obj.image,
                    "transportyear": $scope.ASMAY_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    configurationsettings: $scope.configurationsettings,
                    transnumbconfigurationsettingsss: RegistrationNumbering
                }
                //var config = {
                //    headers: {
                //        'Content-Type': 'application/json;'
                //    },
                //}
                apiService.create("StudentRoueLocationUpdate/savedata", data).then(function (promise) {

                    
                    if (promise.returnval == "true") {
                        swal("Record Updated Successfully");
                        $state.reload();
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Updated Successfully");
                        $state.reload();
                    }
                    else if (promise.returnval == "false") {
                        swal("Failed to Save/Update");
                    }
                    else if (promise.returnval == "Y") {
                        swal('Failed to Update Data', 'Application approved Already!!');
                    }

                })
            }
            else {
                $scope.submitted = true;
            }
        };

        //payment for aggregtor
        //
        $scope.pamentsave = function () {
            
            var payu_URL = $scope.payu_URL;
            var url = payu_URL;
            var method = 'POST';
            var params = {
                "key": $scope.key,
                "txnid": $scope.txnid,
                "amount": $scope.amount,
                "productinfo": $scope.productinfo,
                "firstname": $scope.firstname,
                "email": $scope.email,
                "phone": $scope.phone,
                "udf1": $scope.udf1,
                "udf2": $scope.udf2,
                "udf3": $scope.udf3,
                "udf4": $scope.udf4,
                "udf5": $scope.udf5,
                "udf6": $scope.udf6,
                "service_provider": $scope.service_provider,
                "hash": $scope.hash,
                "surl": "http://localhost:57606/api/StudentRoueLocationUpdate/paymentresponse/",
                "furl": "http://localhost:57606/api/StudentRoueLocationUpdate/paymentresponse/"

                //    "surl": "http://localhost:57606/api/StudentApplication/paymentresponse/",
                //"furl": "http://localhost:57606/api/StudentApplication/paymentresponse/"
            }
            FormSubmitter.submit(url, method, params);
        }


        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //payment
        $scope.paynow = function (pasR_Id, asta_id) {



            // $scope.submitted = true;
            var data = {
                "pasr_Id": pasR_Id,
                "ASTA_Id": asta_id,
                configurationsettings: $scope.configurationsettings,
                transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
            }
            apiService.create("StudentRoueLocationUpdate/paynow", data).then(function (promise) {

                if (promise.paydet != null) {

                    $scope.reg.PASR_FirstName = promise.paydet[0].firstname;
                    $scope.reg.PASR_emailId = promise.paydet[0].email;
                    $scope.reg.PASR_MobileNo = promise.paydet[0].phone;
                    $scope.reg.Pasr_amount = promise.paydet[0].amount;

                    $scope.key = promise.paydet[0].marchanT_ID;
                    $scope.txnid = promise.paydet[0].trans_id;
                    $scope.amount = promise.paydet[0].amount;
                    $scope.productinfo = promise.paydet[0].productinfo;
                    $scope.firstname = promise.paydet[0].firstname;
                    $scope.email = promise.paydet[0].email;
                    $scope.phone = promise.paydet[0].phone;
                    $scope.surl = promise.paydet[0].surl;
                    $scope.furl = promise.paydet[0].furl;
                    $scope.hash = promise.paydet[0].hash;
                    $scope.udf1 = promise.paydet[0].udf1;
                    $scope.udf2 = promise.paydet[0].udf2;
                    $scope.udf3 = promise.paydet[0].udf3;
                    $scope.udf4 = promise.paydet[0].udf4;
                    $scope.udf5 = promise.paydet[0].udf5;
                    $scope.udf6 = promise.paydet[0].udf6;
                    $scope.service_provider = promise.paydet[0].service_provider;

                    $scope.hash_string = promise.paydet[0].hash_string;
                    $scope.payu_URL = promise.paydet[0].payu_URL;





                    $scope.PaymentMode = true;
                    $scope.ProspectuseScreen = false;
                    if ($scope.myTabIndex == 1) {
                        $scope.myTabIndex = $scope.myTabIndex - 1;
                    }
                    swal.close();
                    showConfirmButton: false
                }
                else {
                    swal('Registered Successfully,But Payment gateway details are not mapped to institute', 'Contact Administrator..!!');
                    $state.reload();
                }

            });
        }

        $scope.Back = function () {

            $scope.PaymentMode = false;
            $scope.ProspectuseScreen = true;

            // $scope.cancel();
            $state.reload();


        }


        //Clear
        $scope.clear = function () {
            $scope.submitted = false;
            $scope.amsT_Id = "";
            $scope.obj.amsT_BloodGroup = "";
            $scope.obj.amsT_FatherName = "";
            $scope.obj.asmcL_ClassName = "";
            $scope.obj.amsT_emailId = "";
            $scope.obj.amsT_FatherMobleNo = "";
            $scope.obj.amsT_MotherMobileNo = "";
            $scope.trmA_Id = "";
            $scope.trmR_Id = "";
            $scope.trmL_Idp = "";
            $scope.trmL_Idd = "";
            $scope.plocation = false;
            $scope.dlocation = false;
            $scope.pdlocation = false;
            $scope.obj.amsT_PerStreet = "";
            $scope.obj.amsT_PerArea = "";
            $scope.ivrmmC_Id = "";
            $scope.ivrmmS_Id = "";
            $scope.obj.amsT_PerCity = "";
            $scope.obj.amsT_PerPincode = "";
            //$scope.obj.pasR_FatherOfficePhNo = "";
            // $scope.obj.pasR_FatherHomePhNo = "";
            $scope.multiway = "";
            $scope.pdloca = false;
            $scope.maultiway_rdo = false;
            $scope.profile_photo = 2;
            $scope.accchange = true;
        }

        //Cancel
        $scope.cancel = function () {
            $state.reload();
        }


        $scope.searchfilter = function (objj) {
          
                if (objj.search.length >= '2') {

                    var data = {
                        "searchfilter": objj.search,
                        "ASMAY_Id":$scope.ASMAY_Id
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("StudentRoueLocationUpdate/searchfilter", data).
                then(function (promise) {

                    $scope.studentlst = promise.fillstudent;

                    angular.forEach($scope.studentlst, function (objectt) {
                        if (objectt.amsT_FirstName.length > 0) {
                            var string = objectt.amsT_FirstName;
                            objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                        }
                    })
                })
                }
            
        }


    };
})();