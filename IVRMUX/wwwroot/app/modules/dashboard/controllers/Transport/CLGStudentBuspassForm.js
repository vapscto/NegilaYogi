(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGStudentBuspassFormController', CLGStudentBuspassFormController)

    CLGStudentBuspassFormController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile']
    function CLGStudentBuspassFormController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile) {



        $scope.abc = true;
        $scope.abcd = true;
        $scope.paydisable = true;
        $scope.obj = {};
        $scope.pdlocation = false;
        $scope.modl = false;
        var HostName = location.host;
        $scope.locationdrop = true;
        $scope.locationpick = true;
        $scope.showarea = false;
        $scope.acc = true;
        $scope.ASMAY_Iddisble = false;


        //console.log($scope.routeDetls)

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


        $scope.academicload = function () {
            apiService.getDATA("CLGStudentBuspassForm/academicload").
                then(function (promise) {
                    $scope.yearlst = promise.fillyear;
                    $scope.locationlst = promise.locationlist;
                    $scope.locationlstdrop = promise.locationlist;
                    if (promise.fillyear != null) {
                        if ($scope.yearlst.length>0) {
                            $scope.ASMAY_Id = $scope.yearlst[0].asmaY_Id
                            $scope.loaddata();
                            $scope.onloadmodal();
                        }
                    }


                })
        };
               
        $scope.onloadmodal = function () {
            apiService.getURI("CLGStudentBuspassForm/getloaddataintruction",$scope.ASMAY_Id).
                then(function (promise) {
                    if (promise.trnsportcutoffdate == 'True') {
                       
                        $scope.paydisable = false;
                        //$scope.yearlst = promise.fillyear;
                        $scope.ASMAY_Id = promise.currfillyear[0].asmaY_Id;
                        if (promise.openba == false && promise.routeDetails.length == 0) {
                            $scope.modl = false ;
                            var e1 = angular.element(document.getElementById("test1"));
                            $compile(e1.html(promise.htmldata))(($scope));
                            $('#myModalswal').modal({ backdrop: 'static', keyboard: false })
                        }
                        else if (promise.openba == true) {
                            $scope.modl = true;
                            swal('Please Pay the Pending Amount for Academic Year 2018-19 Transport Fee!!');
                        }
                    }
                    else {
                        $scope.paydisable = true;
                        $scope.modl = true;

                        swal('Transport Application is Not Available For This Date!!');
                    }



                })
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
        $scope.routeDetls = [];

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            apiService.getURI("CLGStudentBuspassForm/getloaddata", $scope.ASMAY_Id).
                then(function (promise) {
                    if (promise.trnsportcutoffdate == 'True') {
                        $scope.paydisable = false;
                    }
                    else {
                        $scope.paydisable = true;
                    }

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

                  //  $scope.stu_name = promise.stu_name[0].amcsT_FirstName;
                    $scope.stu_name = promise.stu_name;
                    $scope.prospectusPaymentlist = promise.prospectusPaymentlist;

                    $scope.prospectusPaymentlist = promise.prospectusPaymentlist;
                    if ($scope.prospectusPaymentlist.length > 0) {
                        for (var i = 0; i < $scope.routeDetls.length; i++) {
                            for (var j = 0; j < $scope.prospectusPaymentlist.length; j++) {
                                if ($scope.routeDetls[i].astA_Id == $scope.prospectusPaymentlist[j].astA_Id) {
                                    debugger;
                                    if ($scope.prospectusPaymentlist[j].pastA_Amount > 0) {
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

                    console.log($scope.routeDetls)
                    console.log($scope.paydisable)

                    if ($scope.routeDetls.length > 0) {
                       // $scope.amcsT_Id = $scope.routeDetls[0].amcsT_Id;

                        //$scope.onchange($scope.amcsT_Id);
                    }

                })

           

        };

        
 
        $scope.edit = function (item) {
            $scope.trmL_LocationName = item.trmL_LocationName;
            $scope.obj.amsT_FatherMobleNo = item.amsT_FatherMobleNo;
            $scope.obj.amsT_MotherMobileNo = item.amsT_MotherMobileNo;
            $scope.obj.mark = item.astA_Landmark;
            $scope.amsT_Officephon = item.astA_Phoneoff;
            $scope.amsT_Resphon = item.astA_PhoneRes;
            $scope.amcsT_Id = item.amcsT_Id;
            $scope.astacO_Id = item.astacO_Id;

            $scope.onchange(item.amcsT_Id);

            

        }


        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        //-----student name selection change
        $scope.onchange = function (amcsT_Id) {
            $scope.acc = true;
            $scope.accyer = $scope.ASMAY_Id

            var data = {
                "AMCST_Id": $scope.amcsT_Id,
                "ASMAY_Id": $scope.accyer
            }
            apiService.create("CLGStudentBuspassForm/getstudata", data).
                then(function (promise) {

                    if (promise.trnsportcutoffdate == 'True') {
                        $scope.paydisable = false;
                        $scope.areaLst = promise.areaList;
                        //$scope.routeList = promise.routelist;
                        //$scope.locaList = promise.locationlist;
                        $scope.profile_photo = 1;
                        $scope.studentDetails = promise.studentDetails;
                        console.log($scope.studentDetails);

                        $scope.obj.amsT_FirstName = $scope.studentDetails[0].amcsT_FirstName;
                        $scope.obj.asmcL_ClassName = $scope.studentDetails[0].amcO_CourseName,
                            $scope.obj.amB_BranchName = $scope.studentDetails[0].amB_BranchName,
                            $scope.obj.amsE_SEMName = $scope.studentDetails[0].amsE_SEMName,
                            $scope.obj.asmC_SectionName = $scope.studentDetails[0].acmS_SectionName,
                            $scope.obj.amsT_BloodGroup = $scope.studentDetails[0].amcsT_BloodGroup;
                        $scope.mark = $scope.studentDetails[0].ASTACO_Landmark;
                        $scope.obj.asmaY_Year = $scope.studentDetails[0].asmaY_Year,
                            $scope.obj.amsT_AdmNo = $scope.studentDetails[0].amcsT_AdmNo,
                            $scope.obj.amsT_DOB = $scope.studentDetails[0].amcsT_DOB,
                            $scope.obj.amsT_emailId = $scope.studentDetails[0].amcsT_emailId,
                            $scope.obj.amsT_MobileNo = $scope.studentDetails[0].amcsT_MobileNo,
                            $scope.obj.amsT_PerStreet = $scope.studentDetails[0].amcsT_PerStreet,
                            $scope.obj.amsT_PerCity = $scope.studentDetails[0].amcsT_PerCity,
                            $scope.obj.amsT_PerArea = $scope.studentDetails[0].amcsT_PerArea,
                            $scope.obj.amsT_PerPincode = $scope.studentDetails[0].amcsT_PerPincode,
                            $scope.obj.ivrmmC_CountryName = $scope.studentDetails[0].IVRMMC_CountryName,
                            $scope.obj.ivrmmS_Name = $scope.studentDetails[0].ivrmmS_Name,
                            $scope.obj.amsT_FatherName = $scope.studentDetails[0].amcsT_FatherName,
                            $scope.obj.amsT_MotherName = $scope.studentDetails[0].amcsT_MotherName,
                            $scope.obj.amsT_FatherMobleNo = $scope.studentDetails[0].ASTACO_PickupSMSMobileNo,
                            $scope.obj.amsT_MotherMobileNo = $scope.studentDetails[0].ASTACO_DropSMSMobileNo,
                            $scope.obj.amsT_FatherOfficeAdd = $scope.studentDetails[0].amcsT_FatherOfficeAdd,
                            $scope.obj.amsT_BloodGroup = $scope.studentDetails[0].amcsT_BloodGroup
                        $scope.obj.amsT_Officephon = $scope.studentDetails[0].ASTACO_Phoneoff,
                            $scope.obj.amsT_Resphon = $scope.studentDetails[0].ASTACO_PhoneRes,
                            $scope.mark = $scope.studentDetails[0].ASTACO_Landmark,
                            $scope.obj.image = $scope.studentDetails[0].amcsT_StudentPhoto,
                            $scope.trmA_Id = $scope.studentDetails[0].trmA_Id,
                            $('#blah').attr('src', $scope.studentDetails[0].amcsT_StudentPhoto);

                      

                        $scope.astA_AreaZoneName = $scope.studentDetails[0].ASTACO_AreaZoneName,
                            $scope.obj.astA_PickUp_TRML_Id = $scope.studentDetails[0].astacO_PickUp_TRML_Id,
                            $scope.obj.astA_Drop_TRML_Id = $scope.studentDetails[0].astacO_Drop_TRML_Id,
                            $scope.obj.astA_PickUp_TRMR_Id = $scope.studentDetails[0].astacO_PickUp_TRMR_Id
                        $scope.obj.astA_Drop_TRMR_Id = $scope.studentDetails[0].astacO_Drop_TRMR_Id


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
                        if ($scope.routeDetlsss[0].astA_PickUp_TRML_Id > 0 && $scope.routeDetlsss[0].astA_Drop_TRML_Id > 0) {
                            $scope.obj.multiway = "two";
                        }
                        else {
                            $scope.obj.multiway = "one";
                        }
                        $scope.trmL_Idp = 0;
                        $scope.trmL_Idd = 0;
                        if ($scope.studentDetails.length > 0) {
                            for (var i = 0; i < $scope.routeDetlsss.length; i++) {
                                for (var j = 0; j < $scope.studentDetails.length; j++) {
                                    if ($scope.routeDetlsss[i].astA_Id == $scope.studentDetails[j].astA_Id) {
                                        $scope.obj.trmL_Idp = $scope.routeDetlsss[i].astA_PickUp_TRML_Id;
                                        $scope.obj.trmL_Idd = $scope.routeDetlsss[i].astA_Drop_TRML_Id;
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
                    }
                    else {
                        $scope.paydisable = true;
                        swal('Now You Are Not Able to Fill/Update Application !!');
                    }

                })
        }

        //----- on onform click


        $scope.onformclick = function (astA_Id, amcsT_Id, ASMMM) {
            var data = {
                "AMCST_Id": amcsT_Id,
                "ASTACO_Id": astA_Id,
                "ASMAY_Id": ASMMM
            }
            apiService.create("CLGStudentBuspassForm/getbuspassdata", data).
                then(function (promise) {
                    $scope.getdate = new Date();
                    $scope.buspassdatalst = promise.buspassdatalist;
                    $scope.appno = $scope.buspassdatalst[0].appno;
                    $('#blahnew').attr('src', $scope.buspassdatalst[0].AMST_Photoname);
                    $scope.AMST_AdmNo = $scope.buspassdatalst[0].AMCST_AdmNo;
                    $scope.ASTA_Landmark = $scope.buspassdatalst[0].ASTACO_Landmark;
                    $scope.amcsT_FirstName = $scope.buspassdatalst[0].stuname;
                    $scope.amcsT_FatherName = $scope.buspassdatalst[0].AMCST_FatherName;
                    $scope.amsE_SEMName = $scope.buspassdatalst[0].FutureSem;
                    $scope.amcO_CourseName = $scope.buspassdatalst[0].AMCO_CourseName;
                    $scope.amB_BranchName = $scope.buspassdatalst[0].AMB_BranchName;
                    $scope.amsT_BloodGroup = $scope.buspassdatalst[0].AMCST_BloodGroup;
                    $scope.trmR_RouteName = $scope.buspassdatalst[0].PickUp_Route;
                    $scope.trmR_RouteName_no = $scope.buspassdatalst[0].PickUp_Route_no;
                    $scope.PickUp_Location = $scope.buspassdatalst[0].PickUp_Location;
                    $scope.fuyear = $scope.buspassdatalst[0].fuyear;

                    $scope.Drop_Route = $scope.buspassdatalst[0].Drop_Route;
                    $scope.Drop_Route_no = $scope.buspassdatalst[0].Drop_Route_no;
                    $scope.DropUp_Location = $scope.buspassdatalst[0].DropUp_Location;

                    $scope.amsT_FatherMobleNo = $scope.buspassdatalst[0].ASTACO_PickupSMSMobileNo;

                    $scope.amsT_MotherMobileNo = $scope.buspassdatalst[0].ASTACO_DropSMSMobileNo;
                    $scope.amsT_emailId = $scope.buspassdatalst[0].AMCST_emailId;
                    //------------Address
                    $scope.amsT_PerStreet = $scope.buspassdatalst[0].AMCST_PerStreet;
                    $scope.amsT_PerArea = $scope.buspassdatalst[0].AMCST_PerArea;
                    $scope.amsT_PerCity = $scope.buspassdatalst[0].AMCST_PerCity;
                    $scope.ivrmmS_Name = $scope.buspassdatalst[0].IVRMMS_Name;
                    $scope.ivrmmC_CountryName = $scope.buspassdatalst[0].IVRMMC_CountryName;
                    $scope.amsT_PerPincode = $scope.buspassdatalst[0].AMCST_PerPincode;
                    $scope.ASTA_Regnew = $scope.buspassdatalst[0].ASTACO_Regnew;
                    $scope.amsT_Office = $scope.buspassdatalst[0].ASTACO_Phoneoff;
                    $scope.amsT_Res = $scope.buspassdatalst[0].ASTACO_PhoneRes;
                    $scope.getdate = $scope.buspassdatalst[0].ASTACO_ApplicationDate;
                    $scope.MI_Name = $scope.buspassdatalst[0].MI_Name;
                    $scope.IVRMMCT_Name = $scope.buspassdatalst[0].IVRMMCT_Name;
                    $scope.MI_Pincode = $scope.buspassdatalst[0].MI_Pincode;
                    $scope.MI_Address1 = $scope.buspassdatalst[0].MI_Address1;
                    var e1 = angular.element(document.getElementById("test"));
                    $compile(e1.html(promise.htmldata))(($scope));
                    $('#blahnew').attr('src', $scope.buspassdatalst[0].AMCST_StudentPhoto);
                    $('#blahnewF').attr('src', $scope.buspassdatalst[0].AMCST_FatherPhoto);
                    $('#blahnewM').attr('src', $scope.buspassdatalst[0].AMCST_MotherPhoto);
                })
        }


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

        $scope.onchangeacc = function (trmA_Id) {
            $scope.clear();
            $scope.routeDetls = [];

            $scope.amcsT_Id = "";
            if (trmA_Id != 0) {
                //$scope.modl = true;

            }
            else {
                //$scope.modl = false;
            }

            $scope.loaddata();
            $scope.onloadmodal();

        }

        //--Area selection change 
        $scope.onareachange = function (trmA_Id) {

            var data = {
                "pasaA_Id": $scope.pasaA_Id,
            }
            apiService.create("CLGStudentBuspassForm/getroutedata", data).
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
            apiService.create("CLGStudentBuspassForm/getlocationdata", data).
                then(function (promise) {

                    $scope.locationlst = promise.locationlist;

                })
        }

        $scope.onroutechangedrop = function (trmA_Id) {

            var data = {
                "pasaA_Id": $scope.pasaA_Id,
            }
            apiService.create("CLGStudentBuspassForm/getlocationdata", data).
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
            apiService.create("CLGStudentBuspassForm/getlocationdataonly", data).
                then(function (promise) {

                    $scope.locationlst = promise.locationlist;
                    $scope.locationpick = false;

                })
        }

        $scope.onroutechangedropload = function (trmA_Id) {

            var data = {
                "TRMR_Id": trmA_Id,

            }
            apiService.create("CLGStudentBuspassForm/getlocationdataonly", data).
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
            $scope.mark = ""; $scope.obj.amsT_Resphon = 0; $scope.obj.amsT_Officephon = 0;
            var areaname = "";
            if ($scope.myForm.$valid) {

                //if ($scope.multiway == null || $scope.multiway == undefined) {
                //    swal("Select Transport Type");
                //    return;
                //}

                //if ($scope.multiway == 'one') {
                //    if ($scope.plocation == true || $scope.dlocation == true) {

                //    }
                //    else {
                //        swal("Select at least one route");
                //        return;
                //    }
                //}
                if ($scope.multiway == 'two') {
                    //if ($scope.plocation == true && $scope.dlocation == true) {

                    //}
                    //else {
                    //    swal("Select route details");
                    //    return;
                    //}
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
                    "AMCST_Id": $scope.amcsT_Id,
                    "TRMA_Id": $scope.trmA_Id,
                    "ASTACO_Id": $scope.astacO_Id,
                    //"TRMR_Idp": $scope.trmR_Idp,
                    //"TRMR_Idd": $scope.trmR_Idd,
                    "TRML_Idp": $scope.obj.trmL_Idp,
                    "TRML_Idd": $scope.obj.trmL_Idd,
                    "TRMA_AreaName": areaname,
                    "ASTA_Landmark": $scope.obj.mark,
                    "ASTA_FatherMobileNo": $scope.obj.amsT_FatherMobleNo,
                    "ASTA_MotherMobileNo": $scope.obj.amsT_MotherMobileNo,
                    "ASTA_Phoneoff": $scope.amsT_Officephon,
                    "ASTA_PhoneRes": $scope.amsT_Resphon,
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
                apiService.create("CLGStudentBuspassForm/savedata", data).then(function (promise) {


                    if (promise.returnval == "true") {
                        swal("Record Saved Successfully");
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
                    else if (promise.returnval == "Update") {
                        swal('Record Updated Successfully');
                        $state.reload();
                    }
                    else if (promise.returnval == "NotUpdate") {
                        swal('Failed to Update Record');
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
            $scope.onclickloaddata();


        }


        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.paymenttype = '';
        $scope.onclickloaddata1 = function (paydd) {
            $scope.paymenttype = paydd;
            if ($scope.paymenttype =='RAZORPAY') {
                var data = {
                    "pasr_Id": $scope.newpasR_Id,
                    "ASTA_Id": $scope.newasta_id,
                    "paytype": $scope.paymenttype,

                    configurationsettings: $scope.configurationsettings,
                    transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                }
                apiService.create("CLGStudentBuspassForm/paynow2", data).then(function (promise) {


                    $scope.reg.PASR_FirstName = promise.amsT_FirstName;
                    $scope.reg.PASR_emailId = promise.amsT_emailId;
                    $scope.reg.PASR_MobileNo = promise.amsT_MobileNo;
                    // $scope.reg.Pasr_amount = promise.paydet[0].amount;

                    $scope.PaymentMode = true;
                    $scope.ProspectuseScreen = false;
                    if ($scope.myTabIndex == 1) {
                        $scope.myTabIndex = $scope.myTabIndex - 1;
                    }
                    swal.close();
                    showConfirmButton: false


                   // if (promise.paydet != null) {



                        if (paydd == "RAZORPAY") {
                            debugger;
                            //$scope.instidet = promise.instidet;
                           $scope.instidet = promise.instidet;


                            //$scope.txnamt = Number(promise.fmA_Amount) * 100;
                            //$scope.SaltKey = promise.merchantkey;
                            //$scope.orderid = promise.trans_id;

                            //$scope.institutioname = $scope.institutioname;
                            //$scope.institulogo = $scope.institulogo;

                            ////$scope.stuname = promise.fillstudent[0].amsT_FirstName;
                            ////$scope.stuemailid = promise.fillstudent[0].amst_email_id;
                            ////$scope.stuaddress = promise.fillstudent[0].amsT_PerCity;
                            ////$scope.stumobileno = promise.fillstudent[0].amst_mobile;
                            ////$scope.stuadmno = promise.fillstudent[0].amsT_AdmNo;

                            //$scope.stuname = promise.amsT_FirstName;
                            //$scope.stuemailid = promise.amsT_emailId;
                            ////$scope.stuaddress = promise.fillstudent[0].amsT_PerCity;
                            //$scope.stumobileno = promise.amsT_MobileNo;
                            ////$scope.stuadmno = promise.fillstudent[0].amsT_AdmNo;

                            //$scope.splitpayinfor = promise.splitpayinformation;

                            $scope.mI_Id = promise.mI_Id;
                            $scope.asmaY_Id = promise.asmaY_Id;
                            $scope.amcst_Id = promise.amcst_Id;

                            $scope.txnamt = Number(promise.fmA_Amount) * 100;
                            $scope.SaltKey = promise.merchantkey;
                            $scope.orderid = promise.trans_id;

                            $scope.institutioname = $scope.instidet[0].inT_NAME;
                            $scope.institulogo = $scope.instidet[0].institutioN_LOGO;

                            $scope.stuname = promise.amsT_FirstName;
                            $scope.stuemailid = promise.amsT_emailId;
                          //  $scope.stuaddress = promise.fillstudent[0].amsT_PerCity;
                            $scope.stumobileno = promise.amsT_MobileNo;
                            $scope.stuadmno = promise.amsT_AdmNo;
                            $scope.splitpayinfor = promise.splitpayinformation;

                            $scope.mI_Id = promise.mI_Id;
                            $scope.asmaY_Id = promise.asmaY_Id;
                            $scope.amcst_Id = promise.amcst_Id;

                        }
                    //}

                });
            }
          
            
        }


            $scope.onclickloaddata = function (paydd) {
            debugger;
            var data = {
                "pasr_Id": $scope.newpasR_Id,
                "ASTA_Id": $scope.newasta_id,
                "paytype": $scope.paymenttype,

                configurationsettings: $scope.configurationsettings,
                transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
            }
                apiService.create("CLGStudentBuspassForm/paynow1", data).then(function (promise) {


                $scope.reg.PASR_FirstName = promise.amsT_FirstName;
                $scope.reg.PASR_emailId = promise.amsT_emailId;
                $scope.reg.PASR_MobileNo = promise.amsT_MobileNo;
                // $scope.reg.Pasr_amount = promise.paydet[0].amount;

                $scope.PaymentMode = true;
                $scope.ProspectuseScreen = false;
                if ($scope.myTabIndex == 1) {
                    $scope.myTabIndex = $scope.myTabIndex - 1;
                }
                swal.close();
                showConfirmButton: false

                //if ((paydd == "PAYU")) {
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

                    debugger;
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
                        "surl": "http://localhost:57606/api/StudentBuspassform/paymentresponse/",
                        "furl": "http://localhost:57606/api/StudentBuspassform/paymentresponse/"

                        //    "surl": "http://localhost:57606/api/StudentApplication/paymentresponse/",
                        //"furl": "http://localhost:57606/api/StudentApplication/paymentresponse/"
                    }
                    FormSubmitter.submit(url, method, params);


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
                // }
                //else if (paydd == "RAZORPAY") {

                //    $scope.txnamt = Number(promise.fmA_Amount) * 100;
                //    $scope.SaltKey = promise.merchantkey;
                //    $scope.orderid = promise.trans_id;

                //    $scope.institutioname = $scope.institutioname;
                //    $scope.institulogo = $scope.institulogo;

                //    $scope.stuname = promise.fillstudent[0].amsT_FirstName;
                //    $scope.stuemailid = promise.fillstudent[0].amst_email_id;
                //    $scope.stuaddress = promise.fillstudent[0].amsT_PerCity;
                //    $scope.stumobileno = promise.fillstudent[0].amst_mobile;
                //    $scope.stuadmno = promise.fillstudent[0].amsT_AdmNo;
                //    $scope.splitpayinfor = promise.splitpayinformation;

                //    $scope.mI_Id = promise.mI_Id;
                //    $scope.asmaY_Id = promise.asmaY_Id;
                //    $scope.amcst_Id = promise.amcst_Id;
                //}


            });
        }


        $scope.newpasR_Id = 0;
        $scope.newasta_id = 0;

        //payment
        $scope.paynow = function (pasR_Id, asta_id) {

            $scope.newpasR_Id = pasR_Id;
            $scope.newasta_id = asta_id;

            // $scope.submitted = true;
            var data = {
                "pasr_Id": pasR_Id,
                "ASTA_Id": asta_id,
               // "paytype": qwe.paygtw,

                configurationsettings: $scope.configurationsettings,
                transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
            }
            apiService.create("StudentBuspassForm/paynow", data).then(function (promise) {


                if (promise.fillpaymentgateway != null && promise.fillpaymentgateway.length > 0) {
                    $scope.paymenttest = promise.fillpaymentgateway;
                    console.log("tttttttttttttttt");
                    console.log($scope.paymenttest);
                }
                $scope.reg.PASR_FirstName = promise.amsT_FirstName;
                $scope.reg.PASR_emailId = promise.amsT_emailId;
                $scope.reg.PASR_MobileNo = promise.amsT_MobileNo;
               // $scope.reg.Pasr_amount = promise.paydet[0].amount;

                 $scope.PaymentMode = true;
                    $scope.ProspectuseScreen = false;
                    if ($scope.myTabIndex == 1) {
                        $scope.myTabIndex = $scope.myTabIndex - 1;
                    }
                    swal.close();
                    showConfirmButton: false

                //    if ((qwe.paygtw == "PAYU")) {
                //if (promise.paydet != null) {


                    
                //    $scope.reg.PASR_FirstName = promise.paydet[0].firstname;
                //    $scope.reg.PASR_emailId = promise.paydet[0].email;
                //    $scope.reg.PASR_MobileNo = promise.paydet[0].phone;
                //    $scope.reg.Pasr_amount = promise.paydet[0].amount;

                //    $scope.key = promise.paydet[0].marchanT_ID;
                //    $scope.txnid = promise.paydet[0].trans_id;
                //    $scope.amount = promise.paydet[0].amount;
                //    $scope.productinfo = promise.paydet[0].productinfo;
                //    $scope.firstname = promise.paydet[0].firstname;
                //    $scope.email = promise.paydet[0].email;
                //    $scope.phone = promise.paydet[0].phone;
                //    $scope.surl = promise.paydet[0].surl;
                //    $scope.furl = promise.paydet[0].furl;
                //    $scope.hash = promise.paydet[0].hash;
                //    $scope.udf1 = promise.paydet[0].udf1;
                //    $scope.udf2 = promise.paydet[0].udf2;
                //    $scope.udf3 = promise.paydet[0].udf3;
                //    $scope.udf4 = promise.paydet[0].udf4;
                //    $scope.udf5 = promise.paydet[0].udf5;
                //    $scope.udf6 = promise.paydet[0].udf6;
                //    $scope.service_provider = promise.paydet[0].service_provider;

                //    $scope.hash_string = promise.paydet[0].hash_string;
                //    $scope.payu_URL = promise.paydet[0].payu_URL;





                //    $scope.PaymentMode = true;
                //    $scope.ProspectuseScreen = false;
                //    if ($scope.myTabIndex == 1) {
                //        $scope.myTabIndex = $scope.myTabIndex - 1;
                //    }
                //    swal.close();
                //    showConfirmButton: false
                //        }
                //        else {
                //            swal('Registered Successfully,But Payment gateway details are not mapped to institute', 'Contact Administrator..!!');
                //            $state.reload();
                //        }
               // }
                //        else if (qwe.paygtw == "RAZORPAY") {

                //    $scope.txnamt = Number(promise.fmA_Amount) * 100;
                //    $scope.SaltKey = promise.merchantkey;
                //    $scope.orderid = promise.trans_id;

                //    $scope.institutioname = $scope.institutioname;
                //    $scope.institulogo = $scope.institulogo;

                //    $scope.stuname = promise.fillstudent[0].amsT_FirstName;
                //    $scope.stuemailid = promise.fillstudent[0].amst_email_id;
                //    $scope.stuaddress = promise.fillstudent[0].amsT_PerCity;
                //    $scope.stumobileno = promise.fillstudent[0].amst_mobile;
                //    $scope.stuadmno = promise.fillstudent[0].amsT_AdmNo;
                //    $scope.splitpayinfor = promise.splitpayinformation;

                //    $scope.mI_Id = promise.mI_Id;
                //    $scope.asmaY_Id = promise.asmaY_Id;
                //    $scope.amcst_Id = promise.amcst_Id;
                //}
               

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
            $scope.amcsT_Id = "";
            $scope.trmA_Id = "";
            $scope.obj.amsT_BloodGroup = "";
            $scope.obj.amsT_FatherName = "";
            $scope.obj.asmcL_ClassName = "";
            $scope.obj.amsT_emailId = "";
            $scope.obj.amsT_FatherMobleNo = "";
            $scope.obj.amsT_MotherMobileNo = "";
            $scope.obj.amB_BranchName = "";
            $scope.obj.amsE_SEMName = "";
            $scope.areaLst = {};
            $scope.trmR_Id = "";
            $scope.obj.mark = "";
            $scope.amsT_Officephon = "";
            $scope.amsT_Resphon = "";
            $scope.obj.trmL_Idd = "";
            $scope.obj.trmL_Idp = "";
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
        }

        //Cancel
        $scope.cancel = function () {

            $scope.loaddata();
            $scope.clear();
        }

    };
})();