(function () {
    'use strict';

    angular
        .module('app')
        .controller('trannumberController', trannumberController);
    trannumberController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'superCache']
    function trannumberController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, superCache) {

        //PREADMISSION REGISTRATION start
        $scope.preregno = {};
        $scope.enquiry = {};
        $scope.Prospectus = {};
        $scope.Registration = {};
        $scope.Admission = {};
        $scope.AdmissionReg = {};
        $scope.tcno = {};
        $scope.Rollno = {};


        //Bus hire.
        $scope.OnlineBooking = {};
        $scope.Trip = {};
        $scope.TripBill = {};

        $scope.Loan = {};
        $scope.Leave = {};
        $scope.Receipt = {};

        $scope.Transaction = {};
        $scope.Application = {};
        $scope.Voucher = {};
        $scope.enqform = false;
        $scope.preregdis = false;
        $scope.prospdis = false;
        $scope.regdis = false;
        $scope.admCanceldis = false;

        $scope.AdmRegform = false;
        $scope.Receiptdis = false;

        //Bus hire.
        $scope.OnlineBookingdis = false;
        $scope.Tripdis = false;
        $scope.TripBilldis = false;

        $scope.Transactiondis = false;
        $scope.Applicationdis = false;

        $scope.Voucherdis = false;
        $scope.tcdis = false;

        $scope.loandis = false;
        $scope.LeaveCanceldis = false;

        $scope.details = function () {

            var MI = 2;
            apiService.getURI("TransactionNumbering/getalldetails", MI).
                then(function (promise) {

                    if (promise.enquiryNumberingArraylist.length > 0) {

                        $scope.enqform = true;

                        //$scope.enqtmanual = true;
                        //$scope.enqtautoParti = true;
                        //$scope.enqtautoPartisuf = true;
                        //$scope.enqslnostarting = true;
                        //$scope.enqtauto = true;


                        $scope.enquiry.IMN_Id = promise.enquiryNumberingArraylist[0].imN_Id;
                        $scope.enquiry.IMN_AutoManualFlag = promise.enquiryNumberingArraylist[0].imN_AutoManualFlag;

                        $scope.manualEnq($scope.enquiry.IMN_AutoManualFlag);

                        if ($scope.enquiry.IMN_AutoManualFlag == "Manual") {

                            $scope.enquiry.IMN_RestartNumFlag = null;
                            $scope.enquiry.IMN_PrefixAcadYearCode = null;
                            $scope.enquiry.IMN_SuffixAcadYearCode = null;
                            $scope.enqtmanual = false;

                        }
                        else {

                            $scope.enquiry.IMN_RestartNumFlag = promise.enquiryNumberingArraylist[0].imN_RestartNumFlag;
                            $scope.enquiry.IMN_PrefixAcadYearCode = promise.enquiryNumberingArraylist[0].imN_PrefixAcadYearCode;
                            $scope.enquiry.IMN_SuffixAcadYearCode = promise.enquiryNumberingArraylist[0].imN_SuffixAcadYearCode;

                            // $scope.particularpreEnq($scope.enquiry.IMN_PrefixAcadYearCode);
                            // $scope.particularsufEnq($scope.enquiry.IMN_SuffixAcadYearCode);

                        }

                        $scope.enquiry.IMN_DuplicatesFlag = promise.enquiryNumberingArraylist[0].imN_DuplicatesFlag;
                        $scope.enquiry.IMN_StartingNo = promise.enquiryNumberingArraylist[0].imN_StartingNo;
                        $scope.enquiry.IMN_WidthNumeric = promise.enquiryNumberingArraylist[0].imN_WidthNumeric;
                        $scope.enquiry.IMN_ZeroPrefixFlag = promise.enquiryNumberingArraylist[0].imN_ZeroPrefixFlag;
                        $scope.enquiry.IMN_PrefixParticular = promise.enquiryNumberingArraylist[0].imN_PrefixParticular;
                        $scope.enquiry.IMN_SuffixParticular = promise.enquiryNumberingArraylist[0].imN_SuffixParticular;
                        // $scope.enquiry.IMN_RestartAcadYear = promise.enquiryNumberingArraylist[0].imN_RestartAcadYear;

                    }

                    if (promise.registrationNumberingArraylist.length > 0) {
                        $scope.regdis = true;

                        //$scope.regtmanual = true;
                        //$scope.regtautoParti = true;
                        //$scope.regtautoPartisuf = true;
                        //$scope.regslnostarting = true;
                        //$scope.regtauto = true;



                        $scope.Registration.IMN_Id = promise.registrationNumberingArraylist[0].imN_Id;
                        $scope.Registration.IMN_AutoManualFlag = promise.registrationNumberingArraylist[0].imN_AutoManualFlag;

                        $scope.manuallreg($scope.Registration.IMN_AutoManualFlag);

                        if ($scope.Registration.IMN_AutoManualFlag == "Manual") {

                            $scope.Registration.IMN_RestartNumFlag = null;
                            $scope.Registration.IMN_PrefixAcadYearCode = null;
                            $scope.Registration.IMN_SuffixAcadYearCode = null;
                            $scope.regtmanual = false;
                        }
                        else {
                            $scope.Registration.IMN_RestartNumFlag = promise.registrationNumberingArraylist[0].imN_RestartNumFlag;
                            $scope.Registration.IMN_PrefixAcadYearCode = promise.registrationNumberingArraylist[0].imN_PrefixAcadYearCode;
                            $scope.Registration.IMN_SuffixAcadYearCode = promise.registrationNumberingArraylist[0].imN_SuffixAcadYearCode;

                            //  $scope.particularprereg($scope.Registration.IMN_PrefixAcadYearCode);
                            //  $scope.particularsufreg($scope.Registration.IMN_SuffixAcadYearCode);
                        }

                        $scope.Registration.IMN_DuplicatesFlag = promise.registrationNumberingArraylist[0].imN_DuplicatesFlag;
                        $scope.Registration.IMN_StartingNo = promise.registrationNumberingArraylist[0].imN_StartingNo;
                        $scope.Registration.IMN_WidthNumeric = promise.registrationNumberingArraylist[0].imN_WidthNumeric;
                        $scope.Registration.IMN_ZeroPrefixFlag = promise.registrationNumberingArraylist[0].imN_ZeroPrefixFlag;
                        $scope.Registration.IMN_PrefixParticular = promise.registrationNumberingArraylist[0].imN_PrefixParticular;
                        $scope.Registration.IMN_SuffixParticular = promise.registrationNumberingArraylist[0].imN_SuffixParticular;
                        //  $scope.Registration.IMN_RestartAcadYear = promise.registrationNumberingArraylist[0].imN_RestartAcadYear;

                    }
                    if (promise.prospectusNumberingArraylist.length > 0) {
                        $scope.prospdis = true;

                        //$scope.prosptmanual = true;
                        //$scope.prosptautoParti = true;
                        //$scope.prosptautoPartisuf = true;
                        //$scope.prospslnostarting = true;
                        //$scope.prosptauto = true;



                        $scope.Prospectus.IMN_Id = promise.prospectusNumberingArraylist[0].imN_Id;
                        $scope.Prospectus.IMN_AutoManualFlag = promise.prospectusNumberingArraylist[0].imN_AutoManualFlag;

                        $scope.manualprosp($scope.Prospectus.IMN_AutoManualFlag);

                        if ($scope.Prospectus.IMN_AutoManualFlag == "Manual") {

                            $scope.Prospectus.IMN_RestartNumFlag = null;
                            $scope.Prospectus.IMN_PrefixAcadYearCode = null;
                            $scope.Prospectus.IMN_SuffixAcadYearCode = null;
                            $scope.prosptmanual = false;
                        }
                        else {
                            $scope.Prospectus.IMN_RestartNumFlag = promise.prospectusNumberingArraylist[0].imN_RestartNumFlag;
                            $scope.Prospectus.IMN_PrefixAcadYearCode = promise.prospectusNumberingArraylist[0].imN_PrefixAcadYearCode;
                            $scope.Prospectus.IMN_SuffixAcadYearCode = promise.prospectusNumberingArraylist[0].imN_SuffixAcadYearCode;

                            // $scope.particularpreprosp($scope.Prospectus.IMN_PrefixAcadYearCode);
                            // $scope.particularsufprosp($scope.Prospectus.IMN_SuffixAcadYearCode);
                        }

                        $scope.Prospectus.IMN_DuplicatesFlag = promise.prospectusNumberingArraylist[0].imN_DuplicatesFlag;
                        $scope.Prospectus.IMN_StartingNo = promise.prospectusNumberingArraylist[0].imN_StartingNo;
                        $scope.Prospectus.IMN_WidthNumeric = promise.prospectusNumberingArraylist[0].imN_WidthNumeric;
                        $scope.Prospectus.IMN_ZeroPrefixFlag = promise.prospectusNumberingArraylist[0].imN_ZeroPrefixFlag;
                        $scope.Prospectus.IMN_PrefixParticular = promise.prospectusNumberingArraylist[0].imN_PrefixParticular;
                        $scope.Prospectus.IMN_SuffixParticular = promise.prospectusNumberingArraylist[0].imN_SuffixParticular;
                        // $scope.Prospectus.IMN_RestartAcadYear = promise.prospectusNumberingArraylist[0].imN_RestartAcadYear;

                    }

                    if (promise.preRegistrationNumberingArraylist.length > 0) {
                        $scope.preregdis = true;

                        //$scope.tmanual = true;
                        //$scope.tautoParti = true;
                        //$scope.tautoPartisuf = true;
                        //$scope.slnostarting = true;
                        //$scope.tauto = true;


                        $scope.preregno.IMN_Id = promise.preRegistrationNumberingArraylist[0].imN_Id;
                        $scope.preregno.IMN_AutoManualFlag = promise.preRegistrationNumberingArraylist[0].imN_AutoManualFlag;

                        $scope.manuall($scope.preregno.IMN_AutoManualFlag);

                        if ($scope.preregno.IMN_AutoManualFlag == "Manual") {
                            $scope.preregno.IMN_RestartNumFlag = null;
                            $scope.preregno.IMN_PrefixAcadYearCode = null;
                            $scope.preregno.IMN_SuffixAcadYearCode = null;
                            $scope.tmanual = false;
                        }
                        else {
                            $scope.preregno.IMN_RestartNumFlag = promise.preRegistrationNumberingArraylist[0].imN_RestartNumFlag;
                            $scope.preregno.IMN_PrefixAcadYearCode = promise.preRegistrationNumberingArraylist[0].imN_PrefixAcadYearCode;
                            $scope.preregno.IMN_SuffixAcadYearCode = promise.preRegistrationNumberingArraylist[0].imN_SuffixAcadYearCode;

                            //   $scope.particularpre($scope.preregno.IMN_PrefixAcadYearCode);
                            //   $scope.particularsufprosp($scope.preregno.IMN_SuffixAcadYearCode);

                        }

                        $scope.preregno.IMN_DuplicatesFlag = promise.preRegistrationNumberingArraylist[0].imN_DuplicatesFlag;
                        $scope.preregno.IMN_StartingNo = promise.preRegistrationNumberingArraylist[0].imN_StartingNo;
                        $scope.preregno.IMN_WidthNumeric = promise.preRegistrationNumberingArraylist[0].imN_WidthNumeric;
                        $scope.preregno.IMN_ZeroPrefixFlag = promise.preRegistrationNumberingArraylist[0].imN_ZeroPrefixFlag;
                        $scope.preregno.IMN_PrefixParticular = promise.preRegistrationNumberingArraylist[0].imN_PrefixParticular;
                        $scope.preregno.IMN_SuffixParticular = promise.preRegistrationNumberingArraylist[0].imN_SuffixParticular;
                        //  $scope.preregno.IMN_RestartAcadYear= promise.preRegistrationNumberingArraylist[0].imN_RestartAcadYear;
                    }


                    //Admission
                    if (promise.admissionNumberingArraylist.length > 0) {
                        $scope.admCanceldis = true;

                        $scope.Admission.IMN_Id = promise.admissionNumberingArraylist[0].imN_Id;
                        $scope.Admission.IMN_AutoManualFlag = promise.admissionNumberingArraylist[0].imN_AutoManualFlag;

                        $scope.manualAdm($scope.Admission.IMN_AutoManualFlag);

                        if ($scope.Admission.IMN_AutoManualFlag == "Manual") {

                            $scope.Admission.IMN_RestartNumFlag = null;
                            $scope.Admission.IMN_PrefixAcadYearCode = null;
                            $scope.Admission.IMN_SuffixAcadYearCode = null;
                            $scope.admtmanual = false;

                        }
                        else {
                            $scope.Admission.IMN_RestartNumFlag = promise.admissionNumberingArraylist[0].imN_RestartNumFlag;
                            $scope.Admission.IMN_PrefixAcadYearCode = promise.admissionNumberingArraylist[0].imN_PrefixAcadYearCode;
                            $scope.Admission.IMN_SuffixAcadYearCode = promise.admissionNumberingArraylist[0].imN_SuffixAcadYearCode;

                            // $scope.particularpreAdm($scope.Admission.IMN_PrefixAcadYearCode);
                            //$scope.particularsufAdm($scope.Admission.IMN_SuffixAcadYearCode);
                        }

                        $scope.Admission.IMN_DuplicatesFlag = promise.admissionNumberingArraylist[0].imN_DuplicatesFlag;
                        $scope.Admission.IMN_StartingNo = promise.admissionNumberingArraylist[0].imN_StartingNo;
                        $scope.Admission.IMN_WidthNumeric = promise.admissionNumberingArraylist[0].imN_WidthNumeric;
                        $scope.Admission.IMN_ZeroPrefixFlag = promise.admissionNumberingArraylist[0].imN_ZeroPrefixFlag;
                        $scope.Admission.IMN_PrefixParticular = promise.admissionNumberingArraylist[0].imN_PrefixParticular;
                        $scope.Admission.IMN_SuffixParticular = promise.admissionNumberingArraylist[0].imN_SuffixParticular;
                        // $scope.Admission.IMN_RestartAcadYear = promise.admissionNumberingArraylist[0].imN_RestartAcadYear;

                    }

                    //Admission registration

                    if (promise.admissionRegNumberingArraylist.length > 0) {

                        $scope.AdmRegform = true;

                        //$scope.AdmRegtmanual = true;
                        //$scope.AdmRegtautoParti = true;
                        //$scope.AdmRegtautoPartisuf = true;
                        //$scope.AdmRegslnostarting = true;
                        //$scope.AdmRegtauto = true;


                        $scope.AdmissionReg.IMN_Id = promise.admissionRegNumberingArraylist[0].imN_Id;
                        $scope.AdmissionReg.IMN_AutoManualFlag = promise.admissionRegNumberingArraylist[0].imN_AutoManualFlag;

                        $scope.manualAdmReg($scope.AdmissionReg.IMN_AutoManualFlag);

                        if ($scope.AdmissionReg.IMN_AutoManualFlag == "Manual") {

                            $scope.AdmissionReg.IMN_RestartNumFlag = null;
                            $scope.AdmissionReg.IMN_PrefixAcadYearCode = null;
                            $scope.AdmissionReg.IMN_SuffixAcadYearCode = null;
                            $scope.AdmRegtmanual = false;

                        }
                        else {

                            $scope.AdmissionReg.IMN_RestartNumFlag = promise.admissionRegNumberingArraylist[0].imN_RestartNumFlag;
                            $scope.AdmissionReg.IMN_PrefixAcadYearCode = promise.admissionRegNumberingArraylist[0].imN_PrefixAcadYearCode;
                            $scope.AdmissionReg.IMN_SuffixAcadYearCode = promise.admissionRegNumberingArraylist[0].imN_SuffixAcadYearCode;

                            // $scope.particularpreAdmReg($scope.AdmissionReg.IMN_PrefixAcadYearCode);
                            // $scope.particularsufAdmReg($scope.AdmissionReg.IMN_SuffixAcadYearCode);

                        }

                        $scope.AdmissionReg.IMN_DuplicatesFlag = promise.admissionRegNumberingArraylist[0].imN_DuplicatesFlag;
                        $scope.AdmissionReg.IMN_StartingNo = promise.admissionRegNumberingArraylist[0].imN_StartingNo;
                        $scope.AdmissionReg.IMN_WidthNumeric = promise.admissionRegNumberingArraylist[0].imN_WidthNumeric;
                        $scope.AdmissionReg.IMN_ZeroPrefixFlag = promise.admissionRegNumberingArraylist[0].imN_ZeroPrefixFlag;
                        $scope.AdmissionReg.IMN_PrefixParticular = promise.admissionRegNumberingArraylist[0].imN_PrefixParticular;
                        $scope.AdmissionReg.IMN_SuffixParticular = promise.admissionRegNumberingArraylist[0].imN_SuffixParticular;
                        // $scope.AdmissionReg.IMN_RestartAcadYear = promise.admissionRegNumberingArraylist[0].imN_RestartAcadYear;

                    }

                    //transaction

                    //Transaction

                    if (promise.transactionNumberingArraylist.length > 0) {
                        $scope.Transactiondis = true;

                        //$scope.Transactiontmanual = true;
                        //$scope.TransactiontautoParti = true;
                        //$scope.TransactiontautoPartisuf = true;
                        //$scope.Transactionslnostarting = true;
                        //$scope.Transactiontauto = true;



                        $scope.Transaction.IMN_Id = promise.transactionNumberingArraylist[0].imN_Id;
                        $scope.Transaction.IMN_AutoManualFlag = promise.transactionNumberingArraylist[0].imN_AutoManualFlag;

                        $scope.manualTransaction($scope.Transaction.IMN_AutoManualFlag);

                        if ($scope.Transaction.IMN_AutoManualFlag == "Manual") {

                            $scope.Transaction.IMN_RestartNumFlag = null;
                            $scope.Transaction.IMN_PrefixAcadYearCode = null;
                            $scope.Transaction.IMN_SuffixAcadYearCode = null;
                            $scope.Transactiontmanual = false;
                        }
                        else {
                            $scope.Transaction.IMN_RestartNumFlag = promise.transactionNumberingArraylist[0].imN_RestartNumFlag;

                            if (promise.transactionNumberingArraylist[0].imN_PrefixAcadYearCode == true) {
                                $scope.Transaction.IMN_PrefixAcadYearCode = "1";
                            }

                            else if (promise.transactionNumberingArraylist[0].imN_PrefixFinYearCode == true) {
                                $scope.Transaction.IMN_PrefixAcadYearCode = "2";
                            }

                            else if (promise.transactionNumberingArraylist[0].imN_PrefixCalYearCode == true) {
                                $scope.Transaction.IMN_PrefixAcadYearCode = "3";
                            }
                            else {
                                $scope.Transaction.IMN_PrefixAcadYearCode = "0";
                            }

                            if (promise.transactionNumberingArraylist[0].imN_SuffixAcadYearCode == true) {
                                $scope.Transaction.IMN_SuffixAcadYearCode = "1";
                            }

                            else if (promise.transactionNumberingArraylist[0].imN_SuffixFinYearCode == true) {
                                $scope.Transaction.IMN_SuffixAcadYearCode = "2";
                            }

                            else if (promise.transactionNumberingArraylist[0].imN_SuffixCalYearCode == true) {
                                $scope.Transaction.IMN_SuffixAcadYearCode = "3";
                            }
                            else {
                                $scope.Transaction.IMN_SuffixAcadYearCode = "0";
                            }


                            // $scope.Transaction.IMN_PrefixAcadYearCode = promise.transactionNumberingArraylist[0].imN_PrefixAcadYearCode;
                            // $scope.Transaction.IMN_SuffixAcadYearCode = promise.transactionNumberingArraylist[0].imN_SuffixAcadYearCode;

                            // $scope.particularpreTransaction($scope.Transaction.IMN_PrefixAcadYearCode);
                            // $scope.particularsufTransaction($scope.Transaction.IMN_SuffixAcadYearCode);
                        }

                        $scope.Transaction.IMN_DuplicatesFlag = promise.transactionNumberingArraylist[0].imN_DuplicatesFlag;
                        $scope.Transaction.IMN_StartingNo = promise.transactionNumberingArraylist[0].imN_StartingNo;
                        $scope.Transaction.IMN_WidthNumeric = promise.transactionNumberingArraylist[0].imN_WidthNumeric;
                        $scope.Transaction.IMN_ZeroPrefixFlag = promise.transactionNumberingArraylist[0].imN_ZeroPrefixFlag;
                        $scope.Transaction.IMN_PrefixParticular = promise.transactionNumberingArraylist[0].imN_PrefixParticular;
                        $scope.Transaction.IMN_SuffixParticular = promise.transactionNumberingArraylist[0].imN_SuffixParticular;
                        // $scope.Transaction.IMN_RestartAcadYear = promise.transactionNumberingArraylist[0].imN_RestartAcadYear;

                    }

                    //Application

                    if (promise.applicationNumberingArraylist.length > 0) {
                        $scope.Applicationdis = true;

                        //$scope.Applicationtmanual = true;
                        //$scope.ApplicationtautoParti = true;
                        //$scope.ApplicationtautoPartisuf = true;
                        //$scope.Applicationslnostarting = true;
                        //$scope.Applicationtauto = true;



                        $scope.Application.IMN_Id = promise.applicationNumberingArraylist[0].imN_Id;
                        $scope.Application.IMN_AutoManualFlag = promise.applicationNumberingArraylist[0].imN_AutoManualFlag;

                        $scope.manualApplication($scope.Application.IMN_AutoManualFlag);

                        if ($scope.Application.IMN_AutoManualFlag == "Manual") {

                            $scope.Application.IMN_RestartNumFlag = null;
                            $scope.Application.IMN_PrefixAcadYearCode = null;
                            $scope.Application.IMN_SuffixAcadYearCode = null;
                            $scope.Applicationtmanual = false;
                        }
                        else {
                            $scope.Application.IMN_RestartNumFlag = promise.applicationNumberingArraylist[0].imN_RestartNumFlag;

                            if (promise.applicationNumberingArraylist[0].imN_PrefixAcadYearCode == true) {
                                $scope.Application.IMN_PrefixAcadYearCode = "1";
                            }

                            else if (promise.applicationNumberingArraylist[0].imN_PrefixFinYearCode == true) {
                                $scope.Application.IMN_PrefixAcadYearCode = "2";
                            }

                            else if (promise.applicationNumberingArraylist[0].imN_PrefixCalYearCode == true) {
                                $scope.Application.IMN_PrefixAcadYearCode = "3";
                            }
                            else {
                                $scope.Application.IMN_PrefixAcadYearCode = "0";
                            }

                            if (promise.applicationNumberingArraylist[0].imN_SuffixAcadYearCode == true) {
                                $scope.Application.IMN_SuffixAcadYearCode = "1";
                            }

                            else if (promise.applicationNumberingArraylist[0].imN_SuffixFinYearCode == true) {
                                $scope.Application.IMN_SuffixAcadYearCode = "2";
                            }

                            else if (promise.applicationNumberingArraylist[0].imN_SuffixCalYearCode == true) {
                                $scope.Application.IMN_SuffixAcadYearCode = "3";
                            }
                            else {
                                $scope.Application.IMN_SuffixAcadYearCode = "0";
                            }




                            //$scope.Application.IMN_PrefixAcadYearCode = promise.applicationNumberingArraylist[0].imN_PrefixAcadYearCode;
                            // $scope.Application.IMN_SuffixAcadYearCode = promise.applicationNumberingArraylist[0].imN_SuffixAcadYearCode;

                            // $scope.particularpreApplication($scope.Application.IMN_PrefixAcadYearCode);
                            // $scope.particularsufApplication($scope.Application.IMN_SuffixAcadYearCode);
                        }

                        $scope.Application.IMN_DuplicatesFlag = promise.applicationNumberingArraylist[0].imN_DuplicatesFlag;
                        $scope.Application.IMN_StartingNo = promise.applicationNumberingArraylist[0].imN_StartingNo;
                        $scope.Application.IMN_WidthNumeric = promise.applicationNumberingArraylist[0].imN_WidthNumeric;
                        $scope.Application.IMN_ZeroPrefixFlag = promise.applicationNumberingArraylist[0].imN_ZeroPrefixFlag;
                        $scope.Application.IMN_PrefixParticular = promise.applicationNumberingArraylist[0].imN_PrefixParticular;
                        $scope.Application.IMN_SuffixParticular = promise.applicationNumberingArraylist[0].imN_SuffixParticular;
                        // $scope.Application.IMN_RestartAcadYear = promise.applicationNumberingArraylist[0].imN_RestartAcadYear;

                    }

                    //receipt


                    //Receipt

                    if (promise.receiptNumberingArraylist.length > 0) {
                        $scope.Receiptdis = true;

                        //$scope.Receipttmanual = true;
                        //$scope.ReceipttautoParti = true;
                        //$scope.ReceipttautoPartisuf = true;
                        //$scope.Receiptslnostarting = true;
                        //$scope.Receipttauto = true;



                        $scope.Receipt.IMN_Id = promise.receiptNumberingArraylist[0].irN_Id;
                        $scope.Receipt.IMN_AutoManualFlag = promise.receiptNumberingArraylist[0].irN_AutoManualFlag;

                        $scope.manualReceipt($scope.Receipt.IMN_AutoManualFlag);

                        if ($scope.Receipt.IMN_AutoManualFlag == "Manual") {

                            $scope.Receipt.IMN_RestartNumFlag = null;
                            $scope.Receipt.IMN_PrefixAcadYearCode = null;
                            $scope.Receipt.IMN_SuffixAcadYearCode = null;
                            $scope.Receipttmanual = false;
                        }
                        else {
                            $scope.Receipt.IMN_RestartNumFlag = promise.receiptNumberingArraylist[0].irN_RestartNumFlag;

                            if (promise.receiptNumberingArraylist[0].irN_PrefixAcadYearCode == true) {
                                $scope.Receipt.IMN_PrefixAcadYearCode = "1";
                            }

                            else if (promise.receiptNumberingArraylist[0].irN_PrefixFinYearCode == true) {
                                $scope.Receipt.IMN_PrefixAcadYearCode = "2";
                            }

                            else if (promise.receiptNumberingArraylist[0].irN_PrefixCalYearCode == true) {
                                $scope.Receipt.IMN_PrefixAcadYearCode = "3";
                            }
                            else {
                                $scope.Receipt.IMN_PrefixAcadYearCode = "0";
                            }

                            if (promise.receiptNumberingArraylist[0].irN_SuffixAcadYearCode == true) {
                                $scope.Receipt.IMN_SuffixAcadYearCode = "1";
                            }

                            else if (promise.receiptNumberingArraylist[0].irN_SuffixFinYearCode == true) {
                                $scope.Receipt.IMN_SuffixAcadYearCode = "2";
                            }

                            else if (promise.receiptNumberingArraylist[0].irN_SuffixCalYearCode == true) {
                                $scope.Receipt.IMN_SuffixAcadYearCode = "3";
                            }
                            else {
                                $scope.Receipt.IMN_SuffixAcadYearCode = "0";
                            }




                            //$scope.Receipt.IMN_PrefixAcadYearCode = promise.receiptNumberingArraylist[0].irN_PrefixAcadYearCode;
                            // $scope.Receipt.IMN_SuffixAcadYearCode = promise.receiptNumberingArraylist[0].irN_SuffixAcadYearCode;

                            // $scope.particularpreReceipt($scope.Receipt.IMN_PrefixAcadYearCode);
                            // $scope.particularsufReceipt($scope.Receipt.IMN_SuffixAcadYearCode);
                        }

                        $scope.Receipt.IMN_DuplicatesFlag = promise.receiptNumberingArraylist[0].irN_DuplicatesFlag;
                        $scope.Receipt.IMN_StartingNo = promise.receiptNumberingArraylist[0].irN_StartingNo;
                        $scope.Receipt.IMN_WidthNumeric = promise.receiptNumberingArraylist[0].irN_WidthNumeric;
                        $scope.Receipt.IMN_ZeroPrefixFlag = promise.receiptNumberingArraylist[0].irN_ZeroPrefixFlag;
                        $scope.Receipt.IMN_PrefixParticular = promise.receiptNumberingArraylist[0].irN_PrefixParticular;
                        $scope.Receipt.IMN_SuffixParticular = promise.receiptNumberingArraylist[0].irN_SuffixParticular;

                        if (promise.receiptNumberingArraylist[0].irN_RestartNumFlag == "Yearly") {
                            $scope.Receipt.IRN_RestartAcadYear = promise.receiptNumberingArraylist[0].irN_RestartAcadYear;
                            $scope.Receipt.IRN_RestartFinYear = promise.receiptNumberingArraylist[0].irN_RestartFinYear;
                            $scope.Receipt.IRN_RestartcalendYear = promise.receiptNumberingArraylist[0].irN_RestartcalendYear;
                        }

                        // $scope.Receipt.IMN_RestartAcadYear = promise.receiptNumberingArraylist[0].irN_RestartAcadYear;

                    }
                    //Voucher

                    if (promise.voucherNumberingArraylist.length > 0) {
                        $scope.Voucherdis = true;

                        //$scope.Vouchertmanual = true;
                        //$scope.VouchertautoParti = true;
                        //$scope.VouchertautoPartisuf = true;
                        //$scope.Voucherslnostarting = true;
                        //$scope.Vouchertauto = true;

                        $scope.Voucher.VoucherName = promise.voucherNumberingArraylist[0].ivN_VoucherName;
                        $scope.Voucher.VoucherType = promise.voucherNumberingArraylist[0].ivN_VoucherID;
                        // $scope.Voucher.Observation = promise.voucherNumberingArraylist[0].ivN_Observation;

                        $scope.Voucher.IMN_Id = promise.voucherNumberingArraylist[0].ivN_Id;
                        $scope.Voucher.IMN_AutoManualFlag = promise.voucherNumberingArraylist[0].ivN_AutoManualFlag;

                        $scope.manualVoucher($scope.Voucher.IMN_AutoManualFlag);

                        if ($scope.Voucher.IMN_AutoManualFlag == "Manual") {

                            $scope.Voucher.IMN_RestartNumFlag = null;
                            $scope.Voucher.IMN_PrefixAcadYearCode = null;
                            $scope.Voucher.IMN_SuffixAcadYearCode = null;
                            $scope.Vouchertmanual = false;
                        }
                        else {
                            $scope.Voucher.IMN_RestartNumFlag = promise.voucherNumberingArraylist[0].ivN_RestartNumFlag;

                            if (promise.voucherNumberingArraylist[0].ivN_PrefixFinYearCode == true) {
                                $scope.Voucher.IMN_PrefixAcadYearCode = "1";
                            }

                            else {
                                $scope.Voucher.IMN_PrefixAcadYearCode = "0";
                            }

                            if (promise.voucherNumberingArraylist[0].ivN_SuffixFinYearCode == true) {
                                $scope.Voucher.IMN_SuffixAcadYearCode = "1";
                            }
                            else {
                                $scope.Voucher.IMN_SuffixAcadYearCode = "0";
                            }

                            //$scope.Voucher.IMN_PrefixAcadYearCode = promise.voucherNumberingArraylist[0].ivN_PrefixAcadYearCode;
                            // $scope.Voucher.IMN_SuffixAcadYearCode = promise.voucherNumberingArraylist[0].ivN_SuffixAcadYearCode;

                            // $scope.particularpreVoucher($scope.Voucher.IMN_PrefixAcadYearCode);
                            // $scope.particularsufVoucher($scope.Voucher.IMN_SuffixAcadYearCode);
                        }

                        $scope.Voucher.IMN_DuplicatesFlag = promise.voucherNumberingArraylist[0].ivN_DuplicatesFlag;
                        $scope.Voucher.IMN_StartingNo = promise.voucherNumberingArraylist[0].ivN_StartingNo;
                        $scope.Voucher.IMN_WidthNumeric = promise.voucherNumberingArraylist[0].ivN_WidthNumeric;
                        $scope.Voucher.IMN_ZeroPrefixFlag = promise.voucherNumberingArraylist[0].ivN_ZeroPrefixFlag;
                        $scope.Voucher.IMN_PrefixParticular = promise.voucherNumberingArraylist[0].ivN_PrefixParticular;
                        $scope.Voucher.IMN_SuffixParticular = promise.voucherNumberingArraylist[0].ivN_SuffixParticular;
                        // $scope.Voucher.IMN_RestartAcadYear = promise.voucherNumberingArraylist[0].ivN_RestartAcadYear;

                    }


                    //-------tc---------

                    if (promise.tcNumberingArraylist.length > 0) {
                        $scope.Transactiondis = true;

                        //$scope.Transactiontmanual = true;
                        //$scope.TransactiontautoParti = true;
                        //$scope.TransactiontautoPartisuf = true;
                        //$scope.Transactionslnostarting = true;
                        //$scope.Transactiontauto = true;



                        $scope.tcno.IMN_Id = promise.tcNumberingArraylist[0].imN_Id;
                        $scope.tcno.IMN_AutoManualFlag = promise.tcNumberingArraylist[0].imN_AutoManualFlag;

                        $scope.manualTransaction($scope.tcno.IMN_AutoManualFlag);

                        if ($scope.tcno.IMN_AutoManualFlag == "Manual") {

                            $scope.tcno.IMN_RestartNumFlag = null;
                            $scope.tcno.IMN_PrefixAcadYearCode = null;
                            $scope.tcno.IMN_SuffixAcadYearCode = null;
                            $scope.tcmanual = false;
                        }
                        else {
                            $scope.tcno.IMN_RestartNumFlag = promise.tcNumberingArraylist[0].imN_RestartNumFlag;

                            if (promise.tcNumberingArraylist[0].imN_PrefixAcadYearCode == true) {
                                $scope.tcno.IMN_PrefixAcadYearCode = "1";
                            }

                            else if (promise.tcNumberingArraylist[0].imN_PrefixFinYearCode == true) {
                                $scope.tcno.IMN_PrefixAcadYearCode = "2";
                            }

                            else if (promise.tcNumberingArraylist[0].imN_PrefixCalYearCode == true) {
                                $scope.tcno.IMN_PrefixAcadYearCode = "3";
                            }
                            else {
                                $scope.tcno.IMN_PrefixAcadYearCode = "0";
                            }

                            if (promise.tcNumberingArraylist[0].imN_SuffixAcadYearCode == true) {
                                $scope.tcno.IMN_SuffixAcadYearCode = "1";
                            }

                            else if (promise.tcNumberingArraylist[0].imN_SuffixFinYearCode == true) {
                                $scope.tcno.IMN_SuffixAcadYearCode = "2";
                            }

                            else if (promise.tcNumberingArraylist[0].imN_SuffixCalYearCode == true) {
                                $scope.tcno.IMN_SuffixAcadYearCode = "3";
                            }
                            else {
                                $scope.tcno.IMN_SuffixAcadYearCode = "0";
                            }


                            // $scope.Transaction.IMN_PrefixAcadYearCode = promise.transactionNumberingArraylist[0].imN_PrefixAcadYearCode;
                            // $scope.Transaction.IMN_SuffixAcadYearCode = promise.transactionNumberingArraylist[0].imN_SuffixAcadYearCode;

                            // $scope.particularpreTransaction($scope.Transaction.IMN_PrefixAcadYearCode);
                            // $scope.particularsufTransaction($scope.Transaction.IMN_SuffixAcadYearCode);
                        }

                        $scope.tcno.IMN_DuplicatesFlag = promise.tcNumberingArraylist[0].imN_DuplicatesFlag;
                        $scope.tcno.IMN_StartingNo = promise.tcNumberingArraylist[0].imN_StartingNo;
                        $scope.tcno.IMN_WidthNumeric = promise.tcNumberingArraylist[0].imN_WidthNumeric;
                        $scope.tcno.IMN_ZeroPrefixFlag = promise.tcNumberingArraylist[0].imN_ZeroPrefixFlag;
                        $scope.tcno.IMN_PrefixParticular = promise.tcNumberingArraylist[0].imN_PrefixParticular;
                        $scope.tcno.IMN_SuffixParticular = promise.tcNumberingArraylist[0].imN_SuffixParticular;
                        // $scope.Transaction.IMN_RestartAcadYear = promise.transactionNumberingArraylist[0].imN_RestartAcadYear;

                    }
                    //OnlineBooking
                    if (promise.onlineBookingNumberingArraylist.length > 0) {
                        $scope.OnlineBookingdis = true;

                        //$scope.Receipttmanual = true;
                        //$scope.ReceipttautoParti = true;
                        //$scope.ReceipttautoPartisuf = true;
                        //$scope.Receiptslnostarting = true;
                        //$scope.Receipttauto = true;



                        $scope.OnlineBooking.IMN_Id = promise.onlineBookingNumberingArraylist[0].imN_Id;
                        $scope.OnlineBooking.IMN_AutoManualFlag = promise.onlineBookingNumberingArraylist[0].imN_AutoManualFlag;

                        $scope.manualOnlineBooking($scope.OnlineBooking.IMN_AutoManualFlag);

                        if ($scope.OnlineBooking.IMN_AutoManualFlag == "Manual") {

                            $scope.OnlineBooking.IMN_RestartNumFlag = null;
                            $scope.OnlineBooking.IMN_PrefixAcadYearCode = null;
                            $scope.OnlineBooking.IMN_SuffixAcadYearCode = null;
                            $scope.OnlineBookingmanual = false;
                        }
                        else {
                            $scope.OnlineBooking.IMN_RestartNumFlag = promise.onlineBookingNumberingArraylist[0].imN_RestartNumFlag;

                            if (promise.onlineBookingNumberingArraylist[0].imN_PrefixAcadYearCode == true) {
                                $scope.OnlineBooking.IMN_PrefixAcadYearCode = "1";
                            }

                            else if (promise.onlineBookingNumberingArraylist[0].imN_PrefixFinYearCode == true) {
                                $scope.OnlineBooking.IMN_PrefixAcadYearCode = "2";
                            }

                            else if (promise.onlineBookingNumberingArraylist[0].imN_PrefixCalYearCode == true) {
                                $scope.OnlineBooking.IMN_PrefixAcadYearCode = "3";
                            }
                            else {
                                $scope.OnlineBooking.IMN_PrefixAcadYearCode = "0";
                            }

                            if (promise.onlineBookingNumberingArraylist[0].imN_SuffixAcadYearCode == true) {
                                $scope.OnlineBooking.IMN_SuffixAcadYearCode = "1";
                            }

                            else if (promise.onlineBookingNumberingArraylist[0].imN_SuffixFinYearCode == true) {
                                $scope.OnlineBooking.IMN_SuffixAcadYearCode = "2";
                            }

                            else if (promise.onlineBookingNumberingArraylist[0].imN_SuffixCalYearCode == true) {
                                $scope.OnlineBooking.IMN_SuffixAcadYearCode = "3";
                            }
                            else {
                                $scope.OnlineBooking.IMN_SuffixAcadYearCode = "0";
                            }




                            //$scope.Receipt.IMN_PrefixAcadYearCode = promise.receiptNumberingArraylist[0].irN_PrefixAcadYearCode;
                            // $scope.Receipt.IMN_SuffixAcadYearCode = promise.receiptNumberingArraylist[0].irN_SuffixAcadYearCode;

                            // $scope.particularpreReceipt($scope.Receipt.IMN_PrefixAcadYearCode);
                            // $scope.particularsufReceipt($scope.Receipt.IMN_SuffixAcadYearCode);
                        }

                        $scope.OnlineBooking.IMN_DuplicatesFlag = promise.onlineBookingNumberingArraylist[0].imN_DuplicatesFlag;
                        $scope.OnlineBooking.IMN_StartingNo = promise.onlineBookingNumberingArraylist[0].imN_StartingNo;
                        $scope.OnlineBooking.IMN_WidthNumeric = promise.onlineBookingNumberingArraylist[0].imN_WidthNumeric;
                        $scope.OnlineBooking.IMN_ZeroPrefixFlag = promise.onlineBookingNumberingArraylist[0].imN_ZeroPrefixFlag;
                        $scope.OnlineBooking.IMN_PrefixParticular = promise.onlineBookingNumberingArraylist[0].imN_PrefixParticular;
                        $scope.OnlineBooking.IMN_SuffixParticular = promise.onlineBookingNumberingArraylist[0].imN_SuffixParticular;



                        // $scope.Receipt.IMN_RestartAcadYear = promise.receiptNumberingArraylist[0].irN_RestartAcadYear;

                    }
                    //Trip Numbering.
                    if (promise.tripNumberingArraylist.length > 0) {
                        $scope.Tripdis = true;

                        $scope.Trip.IMN_Id = promise.tripNumberingArraylist[0].imN_Id;
                        $scope.Trip.IMN_AutoManualFlag = promise.tripNumberingArraylist[0].imN_AutoManualFlag;

                        $scope.manualTrip($scope.Trip.IMN_AutoManualFlag);

                        if ($scope.Trip.IMN_AutoManualFlag == "Manual") {

                            $scope.Trip.IMN_RestartNumFlag = null;
                            $scope.Trip.IMN_PrefixAcadYearCode = null;
                            $scope.Trip.IMN_SuffixAcadYearCode = null;
                            $scope.Tripmanual = false;
                        }
                        else {
                            $scope.Trip.IMN_RestartNumFlag = promise.tripNumberingArraylist[0].imN_RestartNumFlag;

                            if (promise.tripNumberingArraylist[0].imN_PrefixAcadYearCode == true) {
                                $scope.Trip.IMN_PrefixAcadYearCode = "1";
                            }

                            else if (promise.tripNumberingArraylist[0].imN_PrefixFinYearCode == true) {
                                $scope.Trip.IMN_PrefixAcadYearCode = "2";
                            }

                            else if (promise.tripNumberingArraylist[0].imN_PrefixCalYearCode == true) {
                                $scope.Trip.IMN_PrefixAcadYearCode = "3";
                            }
                            else {
                                $scope.Trip.IMN_PrefixAcadYearCode = "0";
                            }

                            if (promise.tripNumberingArraylist[0].imN_SuffixAcadYearCode == true) {
                                $scope.Trip.IMN_SuffixAcadYearCode = "1";
                            }

                            else if (promise.tripNumberingArraylist[0].imN_SuffixFinYearCode == true) {
                                $scope.Trip.IMN_SuffixAcadYearCode = "2";
                            }

                            else if (promise.tripNumberingArraylist[0].imN_SuffixCalYearCode == true) {
                                $scope.Trip.IMN_SuffixAcadYearCode = "3";
                            }
                            else {
                                $scope.Trip.IMN_SuffixAcadYearCode = "0";
                            }
                        }

                        $scope.Trip.IMN_DuplicatesFlag = promise.tripNumberingArraylist[0].imN_DuplicatesFlag;
                        $scope.Trip.IMN_StartingNo = promise.tripNumberingArraylist[0].imN_StartingNo;
                        $scope.Trip.IMN_WidthNumeric = promise.tripNumberingArraylist[0].imN_WidthNumeric;
                        $scope.Trip.IMN_ZeroPrefixFlag = promise.tripNumberingArraylist[0].imN_ZeroPrefixFlag;
                        $scope.Trip.IMN_PrefixParticular = promise.tripNumberingArraylist[0].imN_PrefixParticular;
                        $scope.Trip.IMN_SuffixParticular = promise.tripNumberingArraylist[0].imN_SuffixParticular;
                    }

                    //
                    //TripBill Numbering.
                    if (promise.tripBillNumberingArraylist.length > 0) {
                        $scope.TripBilldis = true;

                        $scope.TripBill.IMN_Id = promise.tripBillNumberingArraylist[0].imN_Id;
                        $scope.TripBill.IMN_AutoManualFlag = promise.tripBillNumberingArraylist[0].imN_AutoManualFlag;

                        $scope.manualTrip($scope.TripBill.IMN_AutoManualFlag);

                        if ($scope.TripBill.IMN_AutoManualFlag == "Manual") {

                            $scope.TripBill.IMN_RestartNumFlag = null;
                            $scope.TripBill.IMN_PrefixAcadYearCode = null;
                            $scope.TripBill.IMN_SuffixAcadYearCode = null;
                            $scope.TripBillmanual = false;
                        }
                        else {
                            $scope.TripBill.IMN_RestartNumFlag = promise.tripBillNumberingArraylist[0].imN_RestartNumFlag;

                            if (promise.tripBillNumberingArraylist[0].imN_PrefixAcadYearCode == true) {
                                $scope.TripBill.IMN_PrefixAcadYearCode = "1";
                            }

                            else if (promise.tripBillNumberingArraylist[0].imN_PrefixFinYearCode == true) {
                                $scope.TripBill.IMN_PrefixAcadYearCode = "2";
                            }

                            else if (promise.tripBillNumberingArraylist[0].imN_PrefixCalYearCode == true) {
                                $scope.TripBill.IMN_PrefixAcadYearCode = "3";
                            }
                            else {
                                $scope.TripBill.IMN_PrefixAcadYearCode = "0";
                            }

                            if (promise.tripBillNumberingArraylist[0].imN_SuffixAcadYearCode == true) {
                                $scope.TripBill.IMN_SuffixAcadYearCode = "1";
                            }

                            else if (promise.tripBillNumberingArraylist[0].imN_SuffixFinYearCode == true) {
                                $scope.TripBill.IMN_SuffixAcadYearCode = "2";
                            }

                            else if (promise.tripBillNumberingArraylist[0].imN_SuffixCalYearCode == true) {
                                $scope.TripBill.IMN_SuffixAcadYearCode = "3";
                            }
                            else {
                                $scope.TripBill.IMN_SuffixAcadYearCode = "0";
                            }
                        }

                        $scope.TripBill.IMN_DuplicatesFlag = promise.tripBillNumberingArraylist[0].imN_DuplicatesFlag;
                        $scope.TripBill.IMN_StartingNo = promise.tripBillNumberingArraylist[0].imN_StartingNo;
                        $scope.TripBill.IMN_WidthNumeric = promise.tripBillNumberingArraylist[0].imN_WidthNumeric;
                        $scope.TripBill.IMN_ZeroPrefixFlag = promise.tripBillNumberingArraylist[0].imN_ZeroPrefixFlag;
                        $scope.TripBill.IMN_PrefixParticular = promise.tripBillNumberingArraylist[0].imN_PrefixParticular;
                        $scope.TripBill.IMN_SuffixParticular = promise.tripBillNumberingArraylist[0].imN_SuffixParticular;
                    }

                    //--------------

                    $scope.Loan.IMN_AutoManualFlag = "Auto";
                    //Loan Numbering
                    if (promise.loanNumberingArraylist != null && promise.loanNumberingArraylist.length > 0) {
                        $scope.loandis = true;

                        //$scope.loantmanual = true;
                        //$scope.loantautoParti = true;
                        //$scope.loantautoPartisuf = true;
                        //$scope.loanslnostarting = true;
                        //$scope.loantauto = true;



                        $scope.Loan.IMN_Id = promise.loanNumberingArraylist[0].imN_Id;
                        $scope.Loan.IMN_AutoManualFlag = promise.loanNumberingArraylist[0].imN_AutoManualFlag;

                        $scope.manualloan($scope.Loan.IMN_AutoManualFlag);

                        if ($scope.Loan.IMN_AutoManualFlag == "Manual") {

                            $scope.Loan.IMN_RestartNumFlag = null;
                            $scope.Loan.IMN_PrefixAcadYearCode = null;
                            $scope.Loan.IMN_SuffixAcadYearCode = null;
                            $scope.loantmanual = false;
                        }
                        else {
                            $scope.Loan.IMN_RestartNumFlag = promise.loanNumberingArraylist[0].imN_RestartNumFlag;
                            $scope.Loan.IMN_PrefixAcadYearCode = promise.loanNumberingArraylist[0].imN_PrefixAcadYearCode;
                            $scope.Loan.IMN_SuffixAcadYearCode = promise.loanNumberingArraylist[0].imN_SuffixAcadYearCode;

                            //  $scope.particularpreloan($scope.Loan.IMN_PrefixAcadYearCode);
                            //  $scope.particularsufloan($scope.Loan.IMN_SuffixAcadYearCode);
                        }

                        $scope.Loan.IMN_DuplicatesFlag = promise.loanNumberingArraylist[0].imN_DuplicatesFlag;
                        $scope.Loan.IMN_StartingNo = promise.loanNumberingArraylist[0].imN_StartingNo;
                        $scope.Loan.IMN_WidthNumeric = promise.loanNumberingArraylist[0].imN_WidthNumeric;
                        $scope.Loan.IMN_ZeroPrefixFlag = promise.loanNumberingArraylist[0].imN_ZeroPrefixFlag;
                        $scope.Loan.IMN_PrefixParticular = promise.loanNumberingArraylist[0].imN_PrefixParticular;
                        $scope.Loan.IMN_SuffixParticular = promise.loanNumberingArraylist[0].imN_SuffixParticular;
                        //  $scope.Loan.IMN_RestartAcadYear = promise.loanNumberingArraylist[0].imN_RestartAcadYear;

                    }
                    //Leave Numbering.
                    if (promise.leaveNumberingArraylist.length > 0) {
                        $scope.LeaveCanceldis = true;

                        $scope.Leave.IMN_Id = promise.leaveNumberingArraylist[0].imN_Id;
                        $scope.Leave.IMN_AutoManualFlag = promise.leaveNumberingArraylist[0].imN_AutoManualFlag;

                        $scope.manualLeave($scope.Leave.IMN_AutoManualFlag);

                        if ($scope.Leave.IMN_AutoManualFlag == "Manual") {

                            $scope.Leave.IMN_RestartNumFlag = null;
                            $scope.Leave.IMN_PrefixAcadYearCode = null;
                            $scope.Leave.IMN_SuffixAcadYearCode = null;
                            $scope.Leavemanual = false;

                        }
                        else {
                            $scope.Leave.IMN_RestartNumFlag = promise.leaveNumberingArraylist[0].imN_RestartNumFlag;
                            $scope.Leave.IMN_PrefixAcadYearCode = promise.leaveNumberingArraylist[0].imN_PrefixAcadYearCode;
                            $scope.Leave.IMN_SuffixAcadYearCode = promise.leaveNumberingArraylist[0].imN_SuffixAcadYearCode;

                            // $scope.particularpreAdm($scope.Admission.IMN_PrefixAcadYearCode);
                            //$scope.particularsufAdm($scope.Admission.IMN_SuffixAcadYearCode);
                        }

                        $scope.Leave.IMN_DuplicatesFlag = promise.leaveNumberingArraylist[0].imN_DuplicatesFlag;
                        $scope.Leave.IMN_StartingNo = promise.leaveNumberingArraylist[0].imN_StartingNo;
                        $scope.Leave.IMN_WidthNumeric = promise.leaveNumberingArraylist[0].imN_WidthNumeric;
                        $scope.Leave.IMN_ZeroPrefixFlag = promise.leaveNumberingArraylist[0].imN_ZeroPrefixFlag;
                        $scope.Leave.IMN_PrefixParticular = promise.leaveNumberingArraylist[0].imN_PrefixParticular;
                        $scope.Leave.IMN_SuffixParticular = promise.leaveNumberingArraylist[0].imN_SuffixParticular;
                        // $scope.Admission.IMN_RestartAcadYear = promise.admissionNumberingArraylist[0].imN_RestartAcadYear;

                    }


                    //rollno 

                    if (promise.rolenoNumbering.length > 0) {
                        //$scope.LeaveCanceldis = true;

                        $scope.Rollno.IMN_Id = promise.rolenoNumbering[0].imN_Id;
                        $scope.Rollno.IMN_AutoManualFlag = promise.rolenoNumbering[0].imN_AutoManualFlag;


                        if ($scope.Rollno.IMN_AutoManualFlag == "Manual") {

                            $scope.Rollno.IMN_RestartNumFlag = null;
                            $scope.Rollno.IMN_PrefixAcadYearCode = null;
                            $scope.Rollno.IMN_SuffixAcadYearCode = null;
                            //$scope.Leavemanual = false;

                        }
                        else {
                            $scope.Rollno.IMN_RestartNumFlag = promise.leaveNumberingArraylist[0].imN_RestartNumFlag;
                            $scope.Rollno.IMN_PrefixAcadYearCode = promise.leaveNumberingArraylist[0].imN_PrefixAcadYearCode;
                            $scope.Rollno.IMN_SuffixAcadYearCode = promise.leaveNumberingArraylist[0].imN_SuffixAcadYearCode;

                            // $scope.particularpreAdm($scope.Admission.IMN_PrefixAcadYearCode);
                            //$scope.particularsufAdm($scope.Admission.IMN_SuffixAcadYearCode);
                        }

                        $scope.Rollno.IMN_DuplicatesFlag = promise.rolenoNumbering[0].imN_DuplicatesFlag;
                        $scope.Rollno.IMN_StartingNo = promise.rolenoNumbering[0].imN_StartingNo;
                        $scope.Rollno.IMN_WidthNumeric = promise.rolenoNumbering[0].imN_WidthNumeric;
                        $scope.Rollno.IMN_ZeroPrefixFlag = promise.rolenoNumbering[0].imN_ZeroPrefixFlag;
                        $scope.Rollno.IMN_PrefixParticular = promise.rolenoNumbering[0].imN_PrefixParticular;
                        $scope.Rollno.IMN_SuffixParticular = promise.rolenoNumbering[0].imN_SuffixParticular;
                        // $scope.Admission.IMN_RestartAcadYear = promise.admissionNumberingArraylist[0].imN_RestartAcadYear;

                        if (promise.rolenoNumberingConfig != null && promise.rolenoNumberingConfig.length > 0) {
                            $scope.rollnoauto = promise.rolenoNumberingConfig;
                        }
                        else {
                            $scope.rollnoauto = [];
                            $scope.Sib = true;
                            $scope.rollnoauto = [{ id: 'roll1' }];
                            $scope.addNewsibling = function () {
                                var newItemNo = $scope.rollnoauto.length + 1;
                                if (newItemNo <= 5) {
                                    $scope.rollnoauto.push({ 'id': 'roll' + newItemNo });
                                }
                            };
                        }

                    }

                    else {
                        $scope.rollnoauto = [];
                        $scope.Sib = true;
                        $scope.rollnoauto = [{ id: 'roll1' }];
                        $scope.addNewsibling = function () {
                            var newItemNo = $scope.rollnoauto.length + 1;
                            if (newItemNo <= 5) {
                                $scope.rollnoauto.push({ 'id': 'roll' + newItemNo });
                            }
                        };
                    }


                    //if (promise.fieldArray !=null && promise.fieldArray.length > 0) {
                    //    $scope.fieldlist = promise.fieldArray;
                    //}


                })

        }

        $scope.manualconfigsettings = function () {
            if ($scope.IMN_AutoManualFlagtare == 'Manual') {

            }
            else {

            }
        }

        $scope.Autoconfigsettings = function () {
            if ($scope.IMN_AutoManualFlagtare == 'Auto') {

            }
            else {

            }
        }

        //roll number config
        // $scope.Sib = true;
        $scope.rollnoauto = [{ id: 'roll1' }];
        $scope.addNewrollno = function () {
            var newItemNo = $scope.rollnoauto.length + 1;

            if (newItemNo <= 5) {
                $scope.rollnoauto.push({ 'id': 'roll' + newItemNo });
            }
        };
        $scope.itemsPerPages = 10;
        $scope.currentPages = 1;

        $scope.removeNewrollno = function (index, id) {


            swal({
                title: "Are you sure?",
                text: "Do you want to delete record ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        var IVRMARNC_Id = 0;
                        angular.forEach($scope.rollnoauto, function (opqr1) {
                            if (opqr1.ivrmarnC_Id == id) {
                                IVRMARNC_Id = opqr1.ivrmarnC_Id;
                            }
                        });


                        apiService.getURI("TransactionNumbering/deleteRollnoconfig", IVRMARNC_Id).then(function (promise) {
                            if (promise.message == "Success") {
                                //$scope.sectionlist = promise.sectionlist;
                                //$scope.getclass = false;
                              
                                var newItemNo = $scope.rollnoauto.length - 1;
                                $scope.rollnoauto.splice(index, 1);

                                if ($scope.rollnoauto.length == 0) {
                                    // $scope.Sib = false;
                                }

                                swal('Record Deleted Successfully', 'success');
                                $state.reload();
                            }
                            else {
                                swal('Failed!!!');
                            }
                        });
                    }
                });
        };

        //roll ends

        ///Enquiry Numbering
        $scope.enqtmanual = true;
        $scope.enqtautoParti = true;
        $scope.enqtautoPartisuf = true;
        $scope.enqslnostarting = true;
        $scope.enqtauto = true;

        $scope.manualEnq = function (obj) {
            if (obj == "Manual") {
                $scope.enqtmanual = false;
                $scope.enqtauto = true;
                $scope.enqtautoParti = true;
                $scope.enqtautoPartisuf = true;
                $scope.enquiry.IMN_RestartNumFlag = null;
                $scope.enquiry.IMN_PrefixAcadYearCode = null;
                $scope.enquiry.IMN_PrefixParticular = '';
                $scope.enquiry.IMN_SuffixAcadYearCode = null;
                $scope.enquiry.IMN_SuffixParticular = '';
                $scope.enquiry.IMN_WidthNumeric = '';
                $scope.enquiry.IMN_StartingNo = '';
                $scope.enquiry.IMN_ZeroPrefixFlag = '';

            }
            else {
                $scope.enqtmanual = true;
                $scope.enqtauto = false;
                $scope.enqtautoParti = true;
                $scope.enqtautoPartisuf = true;
                $scope.enquiry.IMN_DuplicatesFlag = '';

            }
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }
        $scope.particularpreEnq = function (prepart) {
            if (prepart == 1) {
                $scope.enqtautoParti = true;
            }
            else {
                $scope.enqtautoParti = false;

                if ($scope.enquiry.IMN_PrefixAcadYearCode == 0 && $scope.enquiry.IMN_SuffixAcadYearCode == 0) {
                    $scope.enquiry.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.enquiry.IMN_RestartNumFlag = null;
                    $scope.submitted = false;
                    $scope.myForm.$setPristine();
                    $scope.myForm.$setUntouched();
                }
            }

        }
        $scope.particularsufEnq = function (sufpart) {
            if (sufpart == 1) {
                $scope.enqtautoPartisuf = true;
            }
            else {
                $scope.enqtautoPartisuf = false;

                if ($scope.enquiry.IMN_PrefixAcadYearCode == 0 && $scope.enquiry.IMN_SuffixAcadYearCode == 0) {
                    $scope.enquiry.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.enquiry.IMN_RestartNumFlag = null;

                    $scope.submitted = false;
                    $scope.myForm.$setPristine();
                    $scope.myForm.$setUntouched();
                }
            }

        }

        $scope.submitted = false;
        $scope.SaveEnquiryNumbering = function (enquiry) {

            if ($scope.myForm.$valid) {
                var IMN_PrefixAcadYearCode;
                if ($scope.enquiry.IMN_PrefixAcadYearCode == "0") {
                    IMN_PrefixAcadYearCode = false;
                }
                else {
                    IMN_PrefixAcadYearCode = true;
                }
                var IMN_SuffixAcadYearCode;
                if ($scope.enquiry.IMN_SuffixAcadYearCode == "0") {
                    IMN_SuffixAcadYearCode = false;
                }
                else {
                    IMN_SuffixAcadYearCode = true;
                }
                swal({
                    title: "Are you sure?",
                    text: "Do you want to save enquiry Numbering Details ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {


                            var data = {
                                "IMN_Id": $scope.enquiry.IMN_Id,
                                "IMN_Flag": "Enquiry",  //Enquiry Numbering
                                "IMN_AutoManualFlag": $scope.enquiry.IMN_AutoManualFlag,
                                "IMN_DuplicatesFlag": $scope.enquiry.IMN_DuplicatesFlag,
                                "IMN_StartingNo": $scope.enquiry.IMN_StartingNo,
                                "IMN_WidthNumeric": $scope.enquiry.IMN_WidthNumeric,
                                "IMN_ZeroPrefixFlag": $scope.enquiry.IMN_ZeroPrefixFlag,
                                "IMN_PrefixAcadYearCode": IMN_PrefixAcadYearCode,
                                "IMN_PrefixParticular": $scope.enquiry.IMN_PrefixParticular,
                                "IMN_SuffixAcadYearCode": IMN_SuffixAcadYearCode,
                                "IMN_SuffixParticular": $scope.enquiry.IMN_SuffixParticular,
                                "IMN_RestartNumFlag": $scope.enquiry.IMN_RestartNumFlag,
                                //"IMN_RestartAcadYear": $scope.enquiry.IMN_RestartAcadYear
                            }
                            apiService.create("TransactionNumbering/", data).then(function (promise) {
                                $scope.details();
                                swal('Record Saved Successfully');
                            });
                        }
                        else {
                            swal("Cancelled");
                        }
                    });

            }
            else {
                $scope.submitted = true;
            }

        };
        $scope.ClearEnquiryNumbering = function () {

            $scope.enquiry = {};
            $scope.submitted = false;

            $scope.enqtmanual = true;
            $scope.enqtautoParti = true;
            $scope.enqtautoPartisuf = true;
            $scope.enqtauto = true;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }

        $scope.interacteden = function (field) {
            // swal(field);

            return $scope.submitted || field.$dirty;
        };
        $scope.interacted1 = function (field) {
            // swal(field);

            return $scope.submitted1 || field.$dirty;
        };
        $scope.interacted2 = function (field) {
            // swal(field);

            return $scope.submitted2 || field.$dirty;
        };
        $scope.interacted3 = function (field) {
            // swal(field);

            return $scope.submitted3 || field.$dirty;
        };


        //admission
        $scope.interacted4 = function (field) {
            // swal(field);

            return $scope.submitted4 || field.$dirty;
        };

        //admission Registration
        $scope.interactedAdmReg = function (field) {
            // swal(field);

            return $scope.submitted5 || field.$dirty;
        };

        //Receipt
        $scope.interactedReceipt = function (field) {
            // swal(field);

            return $scope.submittedReceipt || field.$dirty;
        };

        //Voucher
        $scope.interactedVoucher = function (field) {
            // swal(field);

            return $scope.submittedVoucher || field.$dirty;
        };


        //Transaction
        $scope.interactedTransaction = function (field) {
            // swal(field);

            return $scope.submittedTransaction || field.$dirty;
        };

        //Application
        $scope.interactedApplication = function (field) {
            // swal(field);

            return $scope.submittedApplication || field.$dirty;
        };

        //------------tc-------------

        $scope.interactedTransactiontc = function (field) {
            // swal(field);

            return $scope.submittedtc || field.$dirty;
        };


        //----- Loan ---

        $scope.interactedTransactionLoan = function (field) {
            // swal(field);

            return $scope.submittedLoan || field.$dirty;
        };
        //Leave Numbering.
        $scope.interactedLeave = function (field) {
            // swal(field);

            return $scope.submittedLeave || field.$dirty;
        };


        //-------------

        //TripOnlineBooking.
        $scope.interactedOnlineBooking = function (field) {
            return $scope.submittedOnlineBooking || field.$dirty;
        };
        //Trip Numbering.
        $scope.interactedTrip = function (field) {
            return $scope.submittedTrip || field.$dirty;
        };
        //TripBill Numbering.
        $scope.interactedTripBill = function (field) {
            return $scope.submittedTripBill || field.$dirty;
        };

        // registration

        $scope.regtmanual = true;
        $scope.regtautoParti = true;
        $scope.regtautoPartisuf = true;
        $scope.regslnostarting = true;
        $scope.regtauto = true;

        $scope.manuallreg = function (obj) {
            if (obj == "Manual") {
                $scope.regtmanual = false;
                $scope.regtauto = true;
                $scope.regtautoParti = true;
                $scope.regtautoPartisuf = true;
                $scope.Registration.IMN_RestartNumFlag = null;
                $scope.Registration.IMN_PrefixAcadYearCode = null;
                $scope.Registration.IMN_PrefixParticular = '';
                $scope.Registration.IMN_SuffixAcadYearCode = null;
                $scope.Registration.IMN_SuffixParticular = '';
                $scope.Registration.IMN_WidthNumeric = '';
                $scope.Registration.IMN_StartingNo = '';
                $scope.Registration.IMN_ZeroPrefixFlag = '';

            }
            else {
                $scope.regtmanual = true;
                $scope.regtauto = false;
                $scope.regtautoParti = true;
                $scope.regtautoPartisuf = true;
                $scope.Registration.IMN_DuplicatesFlag = '';

            }
            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
        }
        $scope.particularprereg = function (prepart) {
            if (prepart == 1) {
                $scope.regtautoParti = true;
            }
            else {
                $scope.regtautoParti = false;

                if ($scope.Registration.IMN_PrefixAcadYearCode == 0 && $scope.Registration.IMN_SuffixAcadYearCode == 0) {
                    $scope.Registration.IMN_RestartNumFlag = "Never";

                }
                else {
                    $scope.Registration.IMN_RestartNumFlag = null;
                    $scope.submitted2 = false;
                    $scope.myForm2.$setPristine();
                    $scope.myForm2.$setUntouched();
                }
            }


        }
        $scope.particularsufreg = function (sufpart) {
            if (sufpart == 1) {
                $scope.regtautoPartisuf = true;
            }
            else {
                $scope.regtautoPartisuf = false;

                if ($scope.Registration.IMN_PrefixAcadYearCode == 0 && $scope.Registration.IMN_SuffixAcadYearCode == 0) {
                    $scope.Registration.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.Registration.IMN_RestartNumFlag = null;

                    $scope.submitted2 = false;
                    $scope.myForm2.$setPristine();
                    $scope.myForm2.$setUntouched();

                }
            }

        }



        $scope.submitted2 = false;
        $scope.SaveRegistrationNumbering = function (Registration) {

            if ($scope.myForm2.$valid) {
                var IMN_PrefixAcadYearCode;
                if ($scope.Registration.IMN_PrefixAcadYearCode == "0") {
                    IMN_PrefixAcadYearCode = false;
                }
                else {
                    IMN_PrefixAcadYearCode = true;
                }
                var IMN_SuffixAcadYearCode;
                if ($scope.Registration.IMN_SuffixAcadYearCode == "0") {
                    IMN_SuffixAcadYearCode = false;
                }
                else {
                    IMN_SuffixAcadYearCode = true;
                }

                swal({
                    title: "Are you sure?",
                    text: "Do you want to save  Registration  Details ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {

                            var data1 = {
                                "IMN_Id": $scope.Registration.IMN_Id,
                                "IMN_Flag": "Registration",  //Registration Numbering
                                "IMN_AutoManualFlag": $scope.Registration.IMN_AutoManualFlag,
                                "IMN_DuplicatesFlag": $scope.Registration.IMN_DuplicatesFlag,
                                "IMN_StartingNo": $scope.Registration.IMN_StartingNo,
                                "IMN_WidthNumeric": $scope.Registration.IMN_WidthNumeric,
                                "IMN_ZeroPrefixFlag": $scope.Registration.IMN_ZeroPrefixFlag,
                                "IMN_PrefixAcadYearCode": IMN_PrefixAcadYearCode,
                                "IMN_PrefixParticular": $scope.Registration.IMN_PrefixParticular,
                                "IMN_SuffixAcadYearCode": IMN_SuffixAcadYearCode,
                                "IMN_SuffixParticular": $scope.Registration.IMN_SuffixParticular,
                                "IMN_RestartNumFlag": $scope.Registration.IMN_RestartNumFlag,
                                //"IMN_RestartAcadYear": $scope.Registration.IMN_RestartAcadYear
                            }
                            apiService.create("TransactionNumbering/", data1).then(function (promise) {
                                $scope.ClearRegistrationNumbering();
                                $scope.details();
                                swal('Record Saved Successfully');
                            });
                        }
                        else {
                            swal("Cancelled");
                        }
                    });

            }
            else {
                $scope.submitted2 = true;
            }

        };
        $scope.ClearRegistrationNumbering = function () {

            $scope.Registration = {};
            $scope.submitted2 = false;

            $scope.regtmanual = true;
            $scope.regtautoParti = true;
            $scope.regtautoPartisuf = true;
            $scope.regtauto = true;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
        }

        /////Prospectus Numbering
        $scope.prosptmanual = true;
        $scope.prosptautoParti = true;
        $scope.prosptautoPartisuf = true;
        $scope.prospslnostarting = true;
        $scope.prosptauto = true;

        $scope.manualprosp = function (obj) {
            if (obj == "Manual") {
                $scope.prosptmanual = false;
                $scope.prosptauto = true;
                $scope.prosptautoParti = true;
                $scope.prosptautoPartisuf = true;
                $scope.Prospectus.IMN_RestartNumFlag = null;
                $scope.Prospectus.IMN_PrefixAcadYearCode = null;
                $scope.Prospectus.IMN_PrefixParticular = '';
                $scope.Prospectus.IMN_SuffixAcadYearCode = null;
                $scope.Prospectus.IMN_SuffixParticular = '';
                $scope.Prospectus.IMN_WidthNumeric = '';
                $scope.Prospectus.IMN_StartingNo = '';
                $scope.Prospectus.IMN_ZeroPrefixFlag = '';

            }
            else {
                $scope.prosptmanual = true;
                $scope.prosptauto = false;
                $scope.prosptautoParti = true;
                $scope.prosptautoPartisuf = true;
                $scope.Prospectus.IMN_DuplicatesFlag = '';

            }
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
        }
        $scope.particularpreprosp = function (prepart) {
            if (prepart == 1) {
                $scope.prosptautoParti = true;
            }
            else {
                $scope.prosptautoParti = false;

                if ($scope.Prospectus.IMN_PrefixAcadYearCode == 0 && $scope.Prospectus.IMN_SuffixAcadYearCode == 0) {
                    $scope.Prospectus.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.Prospectus.IMN_RestartNumFlag = null;

                    $scope.submitted1 = false;
                    $scope.myForm1.$setPristine();
                    $scope.myForm1.$setUntouched();
                }

            }

        }
        $scope.particularsufprosp = function (sufpart) {
            if (sufpart == 1) {
                $scope.prosptautoPartisuf = true;
            }
            else {
                $scope.prosptautoPartisuf = false;

                if ($scope.Prospectus.IMN_PrefixAcadYearCode == 0 && $scope.Prospectus.IMN_SuffixAcadYearCode == 0) {
                    $scope.Prospectus.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.Prospectus.IMN_RestartNumFlag = null;

                    $scope.submitted1 = false;
                    $scope.myForm1.$setPristine();
                    $scope.myForm1.$setUntouched();
                }
            }


        }

        $scope.submitted1 = false;
        $scope.SaveProspectusNumbering = function (Prospectus) {

            if ($scope.myForm1.$valid) {
                var IMN_PrefixAcadYearCode;
                if ($scope.Prospectus.IMN_PrefixAcadYearCode == "0") {
                    IMN_PrefixAcadYearCode = false;
                }
                else {
                    IMN_PrefixAcadYearCode = true;
                }
                var IMN_SuffixAcadYearCode;
                if ($scope.Prospectus.IMN_SuffixAcadYearCode == "0") {
                    IMN_SuffixAcadYearCode = false;
                }
                else {
                    IMN_SuffixAcadYearCode = true;
                }
                swal({
                    title: "Are you sure?",
                    text: "Do you want to save Prospectus Numbering Details ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {


                            var data2 = {
                                "IMN_Id": $scope.Prospectus.IMN_Id,
                                "IMN_Flag": "Prospectus",  //Prospectus Numbering
                                "IMN_AutoManualFlag": $scope.Prospectus.IMN_AutoManualFlag,
                                "IMN_DuplicatesFlag": $scope.Prospectus.IMN_DuplicatesFlag,
                                "IMN_StartingNo": $scope.Prospectus.IMN_StartingNo,
                                "IMN_WidthNumeric": $scope.Prospectus.IMN_WidthNumeric,
                                "IMN_ZeroPrefixFlag": $scope.Prospectus.IMN_ZeroPrefixFlag,
                                "IMN_PrefixAcadYearCode": IMN_PrefixAcadYearCode,
                                "IMN_PrefixParticular": $scope.Prospectus.IMN_PrefixParticular,
                                "IMN_SuffixAcadYearCode": IMN_SuffixAcadYearCode,
                                "IMN_SuffixParticular": $scope.Prospectus.IMN_SuffixParticular,
                                "IMN_RestartNumFlag": $scope.Prospectus.IMN_RestartNumFlag,
                                //"IMN_RestartAcadYear": $scope.Prospectus.IMN_RestartAcadYear
                            }
                            apiService.create("TransactionNumbering/", data2).then(function (promise) {
                                $scope.details();
                                swal('Record Saved Successfully');
                            });
                        }
                        else {
                            swal("Cancelled");
                        }
                    });

            }
            else {
                $scope.submitted1 = true;
            }

        };
        $scope.ClearProspectusNumbering = function () {

            $scope.Prospectus = {};
            $scope.submitted1 = false;

            $scope.prosptmanual = true;
            $scope.prosptautoParti = true;
            $scope.prosptautoPartisuf = true;
            $scope.prosptauto = true;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
        }

        //pre registration

        $scope.tmanual = true;
        $scope.tautoParti = true;
        $scope.tautoPartisuf = true;
        $scope.slnostarting = true;
        $scope.tauto = true;

        $scope.manuall = function (obj) {
            if (obj == "Manual") {
                $scope.tmanual = false;
                $scope.tauto = true;
                $scope.tautoParti = true;
                $scope.tautoPartisuf = true;
                $scope.preregno.IMN_RestartNumFlag = null;
                $scope.preregno.IMN_PrefixAcadYearCode = null;
                $scope.preregno.IMN_PrefixParticular = '';
                $scope.preregno.IMN_SuffixAcadYearCode = null;
                $scope.preregno.IMN_SuffixParticular = '';
                $scope.preregno.IMN_WidthNumeric = '';
                $scope.preregno.IMN_StartingNo = '';
                $scope.preregno.IMN_ZeroPrefixFlag = '';

            }
            else {
                $scope.tmanual = true;
                $scope.tauto = false;
                $scope.tautoParti = true;
                $scope.tautoPartisuf = true;
                $scope.preregno.IMN_DuplicatesFlag = '';

            }
            $scope.submitted3 = false;
            $scope.myForm3.$setPristine();
            $scope.myForm3.$setUntouched();
        }
        $scope.particularpre = function (prepart) {
            if (prepart == 1) {
                $scope.tautoParti = true;
            }
            else {
                $scope.tautoParti = false;

                if ($scope.preregno.IMN_PrefixAcadYearCode == 0 && $scope.preregno.IMN_SuffixAcadYearCode == 0) {
                    $scope.preregno.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.preregno.IMN_RestartNumFlag = null;

                    $scope.submitted3 = false;
                    $scope.myForm3.$setPristine();
                    $scope.myForm3.$setUntouched();
                }
            }



        }
        $scope.particularsuf = function (sufpart) {
            if (sufpart == 1) {
                $scope.tautoPartisuf = true;
            }
            else {
                $scope.tautoPartisuf = false;

                if ($scope.preregno.IMN_PrefixAcadYearCode == 0 && $scope.preregno.IMN_SuffixAcadYearCode == 0) {
                    $scope.preregno.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.preregno.IMN_RestartNumFlag = null;

                    $scope.submitted3 = false;
                    $scope.myForm3.$setPristine();
                    $scope.myForm3.$setUntouched();
                }
            }


        }

        $scope.submitted3 = false;
        $scope.SavePreRegistrationNumbering = function (preregno) {

            if ($scope.myForm3.$valid) {
                var IMN_PrefixAcadYearCode;
                if ($scope.preregno.IMN_PrefixAcadYearCode == "0") {
                    IMN_PrefixAcadYearCode = false;
                }
                else {
                    IMN_PrefixAcadYearCode = true;
                }
                var IMN_SuffixAcadYearCode;
                if ($scope.preregno.IMN_SuffixAcadYearCode == "0") {
                    IMN_SuffixAcadYearCode = false;
                }
                else {
                    IMN_SuffixAcadYearCode = true;
                }


                swal({
                    title: "Are you sure?",
                    text: "Do you want to save Preadmission Registration Numbering Details ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {

                            var data3 = {
                                "IMN_Id": $scope.preregno.IMN_Id,
                                "IMN_Flag": "PreRegistration",  //Prospectus Numbering
                                "IMN_AutoManualFlag": $scope.preregno.IMN_AutoManualFlag,
                                "IMN_DuplicatesFlag": $scope.preregno.IMN_DuplicatesFlag,
                                "IMN_StartingNo": $scope.preregno.IMN_StartingNo,
                                "IMN_WidthNumeric": $scope.preregno.IMN_WidthNumeric,
                                "IMN_ZeroPrefixFlag": $scope.preregno.IMN_ZeroPrefixFlag,
                                "IMN_PrefixAcadYearCode": IMN_PrefixAcadYearCode,
                                "IMN_PrefixParticular": $scope.preregno.IMN_PrefixParticular,
                                "IMN_SuffixAcadYearCode": IMN_SuffixAcadYearCode,
                                "IMN_SuffixParticular": $scope.preregno.IMN_SuffixParticular,
                                "IMN_RestartNumFlag": $scope.preregno.IMN_RestartNumFlag,
                                //"IMN_RestartAcadYear": $scope.preregno.IMN_RestartAcadYear
                            }
                            apiService.create("TransactionNumbering/", data3).then(function (promise) {
                                $scope.details();
                                swal('Record Saved Successfully');
                            });
                        }
                        else {
                            swal("Cancelled");
                        }
                    });



            }
            else {
                $scope.submitted3 = true;
            }

        };
        $scope.ClearPreRegistrationNumbering = function () {

            $scope.preregno = {};
            $scope.submitted3 = false;

            $scope.tmanual = true;
            $scope.tautoParti = true;
            $scope.tautoPartisuf = true;
            $scope.tauto = true;
            $scope.myForm3.$setPristine();
            $scope.myForm3.$setUntouched();
        }
        // PREADMISSION REGISTRATION end


        //Admission

        ///Admission Numbering
        $scope.admtmanual = true;
        $scope.admtautoParti = true;
        $scope.admtautoPartisuf = true;
        $scope.admslnostarting = true;
        $scope.admtauto = true;

        $scope.manualAdm = function (obj) {
            if (obj == "Manual") {
                $scope.admtmanual = false;
                $scope.admtauto = true;
                $scope.admtautoParti = true;
                $scope.admtautoPartisuf = true;
                $scope.Admission.IMN_RestartNumFlag = null;
                $scope.Admission.IMN_PrefixAcadYearCode = null;
                $scope.Admission.IMN_PrefixParticular = '';
                $scope.Admission.IMN_SuffixAcadYearCode = null;
                $scope.Admission.IMN_SuffixParticular = '';
                $scope.Admission.IMN_WidthNumeric = '';
                $scope.Admission.IMN_StartingNo = '';
                $scope.Admission.IMN_ZeroPrefixFlag = '';

            }
            else {
                $scope.admtmanual = true;
                $scope.admtauto = false;
                $scope.admtautoParti = true;
                $scope.admtautoPartisuf = true;
                $scope.Admission.IMN_DuplicatesFlag = '';

            }
            $scope.submitted4 = false;
            $scope.myForm4.$setPristine();
            $scope.myForm4.$setUntouched();
        }
        $scope.particularpreAdm = function (prepart) {
            if (prepart == 1) {
                $scope.admtautoParti = true;
            }
            else {
                $scope.admtautoParti = false;

                if ($scope.Admission.IMN_PrefixAcadYearCode == 0 && $scope.Admission.IMN_SuffixAcadYearCode == 0) {
                    $scope.Admission.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.Admission.IMN_RestartNumFlag = null;
                    $scope.submitted4 = false;
                    $scope.myForm4.$setPristine();
                    $scope.myForm4.$setUntouched();
                }
            }

        }
        $scope.particularsufAdm = function (sufpart) {
            if (sufpart == 1) {
                $scope.admtautoPartisuf = true;
            }
            else {
                $scope.admtautoPartisuf = false;

                if ($scope.Admission.IMN_PrefixAcadYearCode == 0 && $scope.Admission.IMN_SuffixAcadYearCode == 0) {
                    $scope.Admission.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.Admission.IMN_RestartNumFlag = null;

                    $scope.submitted4 = false;
                    $scope.myForm4.$setPristine();
                    $scope.myForm4.$setUntouched();
                }
            }

        }

        $scope.submitted4 = false;
        $scope.SaveAdmissionNumbering = function (Admission) {

            if ($scope.myForm4.$valid) {
                var IMN_PrefixAcadYearCode;
                if ($scope.Admission.IMN_PrefixAcadYearCode == "0") {
                    IMN_PrefixAcadYearCode = false;
                }
                else {
                    IMN_PrefixAcadYearCode = true;
                }
                var IMN_SuffixAcadYearCode;
                if ($scope.Admission.IMN_SuffixAcadYearCode == "0") {
                    IMN_SuffixAcadYearCode = false;
                }
                else {
                    IMN_SuffixAcadYearCode = true;
                }
                swal({
                    title: "Are you sure?",
                    text: "Do you want to save Admission Numbering Details ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {


                            var data4 = {
                                "IMN_Id": $scope.Admission.IMN_Id,
                                "IMN_Flag": "Admission",  //Admission Numbering
                                "IMN_AutoManualFlag": $scope.Admission.IMN_AutoManualFlag,
                                "IMN_DuplicatesFlag": $scope.Admission.IMN_DuplicatesFlag,
                                "IMN_StartingNo": $scope.Admission.IMN_StartingNo,
                                "IMN_WidthNumeric": $scope.Admission.IMN_WidthNumeric,
                                "IMN_ZeroPrefixFlag": $scope.Admission.IMN_ZeroPrefixFlag,
                                "IMN_PrefixAcadYearCode": IMN_PrefixAcadYearCode,
                                "IMN_PrefixParticular": $scope.Admission.IMN_PrefixParticular,
                                "IMN_SuffixAcadYearCode": IMN_SuffixAcadYearCode,
                                "IMN_SuffixParticular": $scope.Admission.IMN_SuffixParticular,
                                "IMN_RestartNumFlag": $scope.Admission.IMN_RestartNumFlag,
                                //"IMN_RestartAcadYear": $scope.Admission.IMN_RestartAcadYear
                            }
                            apiService.create("TransactionNumbering/", data4).then(function (promise) {
                                $scope.details();
                                swal('Record Saved Successfully');
                            });
                        }
                        else {
                            swal("Cancelled");
                        }
                    });

            }
            else {
                $scope.submitted4 = true;
            }

        };
        $scope.ClearAdmissionNumbering = function () {

            $scope.Admission = {};
            $scope.submitted4 = false;

            $scope.admtmanual = true;
            $scope.admtautoParti = true;
            $scope.admtautoPartisuf = true;
            $scope.admtauto = true;
            $scope.myForm4.$setPristine();
            $scope.myForm4.$setUntouched();
        }


        //Admission Registration

        ///Admission Registration Numbering
        $scope.AdmRegtmanual = true;
        $scope.AdmRegtautoParti = true;
        $scope.AdmRegtautoPartisuf = true;
        $scope.AdmRegslnostarting = true;
        $scope.AdmRegtauto = true;

        $scope.manualAdmReg = function (obj) {
            if (obj == "Manual") {
                $scope.AdmRegtmanual = false;
                $scope.AdmRegtauto = true;
                $scope.AdmRegtautoParti = true;
                $scope.AdmRegtautoPartisuf = true;
                $scope.AdmissionReg.IMN_RestartNumFlag = null;
                $scope.AdmissionReg.IMN_PrefixAcadYearCode = null;
                $scope.AdmissionReg.IMN_PrefixParticular = '';
                $scope.AdmissionReg.IMN_SuffixAcadYearCode = null;
                $scope.AdmissionReg.IMN_SuffixParticular = '';
                $scope.AdmissionReg.IMN_WidthNumeric = '';
                $scope.AdmissionReg.IMN_StartingNo = '';
                $scope.AdmissionReg.IMN_ZeroPrefixFlag = '';

            }
            else {
                $scope.AdmRegtmanual = true;
                $scope.AdmRegtauto = false;
                $scope.AdmRegtautoParti = true;
                $scope.AdmRegtautoPartisuf = true;
                $scope.AdmissionReg.IMN_DuplicatesFlag = '';

            }
            $scope.submitted5 = false;
            $scope.myForm5.$setPristine();
            $scope.myForm5.$setUntouched();
        }
        $scope.particularpreAdmReg = function (prepart) {
            if (prepart == 1) {
                $scope.AdmRegtautoParti = true;
            }
            else {
                $scope.AdmRegtautoParti = false;

                if ($scope.AdmissionReg.IMN_PrefixAcadYearCode == 0 && $scope.AdmissionReg.IMN_SuffixAcadYearCode == 0) {
                    $scope.AdmissionReg.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.AdmissionReg.IMN_RestartNumFlag = null;
                    $scope.submitted5 = false;
                    $scope.myForm5.$setPristine();
                    $scope.myForm5.$setUntouched();
                }
            }

        }
        $scope.particularsufAdmReg = function (sufpart) {
            if (sufpart == 1) {
                $scope.AdmRegtautoPartisuf = true;
            }
            else {
                $scope.AdmRegtautoPartisuf = false;

                if ($scope.AdmissionReg.IMN_PrefixAcadYearCode == 0 && $scope.AdmissionReg.IMN_SuffixAcadYearCode == 0) {
                    $scope.AdmissionReg.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.AdmissionReg.IMN_RestartNumFlag = null;

                    $scope.submitted5 = false;
                    $scope.myForm5.$setPristine();
                    $scope.myForm5.$setUntouched();
                }
            }

        }

        $scope.submitted5 = false;
        $scope.SaveAdmissionRegNumbering = function (AdmissionReg) {

            if ($scope.myForm5.$valid) {
                var IMN_PrefixAcadYearCode;
                if ($scope.AdmissionReg.IMN_PrefixAcadYearCode == "0") {
                    IMN_PrefixAcadYearCode = false;
                }
                else {
                    IMN_PrefixAcadYearCode = true;
                }
                var IMN_SuffixAcadYearCode;
                if ($scope.AdmissionReg.IMN_SuffixAcadYearCode == "0") {
                    IMN_SuffixAcadYearCode = false;
                }
                else {
                    IMN_SuffixAcadYearCode = true;
                }
                swal({
                    title: "Are you sure?",
                    text: "Do you want to save AdmissionReg Numbering Details ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {


                            var data5 = {
                                "IMN_Id": $scope.AdmissionReg.IMN_Id,
                                "IMN_Flag": "AdmissionReg",  //AdmissionReg Numbering
                                "IMN_AutoManualFlag": $scope.AdmissionReg.IMN_AutoManualFlag,
                                "IMN_DuplicatesFlag": $scope.AdmissionReg.IMN_DuplicatesFlag,
                                "IMN_StartingNo": $scope.AdmissionReg.IMN_StartingNo,
                                "IMN_WidthNumeric": $scope.AdmissionReg.IMN_WidthNumeric,
                                "IMN_ZeroPrefixFlag": $scope.AdmissionReg.IMN_ZeroPrefixFlag,
                                "IMN_PrefixAcadYearCode": IMN_PrefixAcadYearCode,
                                "IMN_PrefixParticular": $scope.AdmissionReg.IMN_PrefixParticular,
                                "IMN_SuffixAcadYearCode": IMN_SuffixAcadYearCode,
                                "IMN_SuffixParticular": $scope.AdmissionReg.IMN_SuffixParticular,
                                "IMN_RestartNumFlag": $scope.AdmissionReg.IMN_RestartNumFlag,
                                //"IMN_RestartAcadYear": $scope.AdmissionReg.IMN_RestartAcadYear
                            }
                            apiService.create("TransactionNumbering/", data5).then(function (promise) {
                                $scope.details();
                                swal('Record Saved Successfully');
                            });
                        }
                        else {
                            swal("Cancelled");
                        }
                    });

            }
            else {
                $scope.submitted5 = true;
            }

        };
        $scope.ClearAdmissionRegNumbering = function () {

            $scope.AdmissionReg = {};
            $scope.submitted5 = false;

            $scope.enqtmanual = true;
            $scope.enqtautoParti = true;
            $scope.enqtautoPartisuf = true;
            $scope.enqtauto = true;
            $scope.myForm5.$setPristine();
            $scope.myForm5.$setUntouched();
        }



        //Receipt Numbering

        /////Receipt Numbering
        $scope.Receipttmanual = true;
        $scope.ReceipttautoParti = true;
        $scope.ReceipttautoPartisuf = true;
        $scope.Receiptslnostarting = true;
        $scope.Receipttauto = true;

        $scope.manualReceipt = function (obj) {
            if (obj == "Manual") {
                $scope.Receipttmanual = false;
                $scope.Receipttauto = true;
                $scope.ReceipttautoParti = true;
                $scope.ReceipttautoPartisuf = true;
                $scope.Receipt.IMN_RestartNumFlag = null;
                $scope.Receipt.IMN_PrefixAcadYearCode = null;
                // $scope.Receipt.IRN_PrefixFinYearCode = null;
                // $scope.Receipt.IRN_PrefixCalYearCode = null;
                $scope.Receipt.IMN_PrefixParticular = '';
                $scope.Receipt.IMN_SuffixAcadYearCode = null;
                //$scope.Receipt.IRN_SuffixFinYearCode = null;
                // $scope.Receipt.IRN_SuffixCalYearCode = null;
                $scope.Receipt.IMN_SuffixParticular = '';
                $scope.Receipt.IMN_WidthNumeric = '';
                $scope.Receipt.IMN_StartingNo = '';
                $scope.Receipt.IMN_ZeroPrefixFlag = '';

                $scope.Receipt.IRN_RestartAcadYear = "";
                $scope.Receipt.IRN_RestartFinYear = "";
                $scope.Receipt.IRN_RestartcalendYear = "";



            }
            else {
                $scope.Receipttmanual = true;
                $scope.Receipttauto = false;
                $scope.ReceipttautoParti = true;
                $scope.ReceipttautoPartisuf = true;
                $scope.Receipt.IMN_DuplicatesFlag = '';

            }
            $scope.submittedReceipt = false;
            $scope.myForm6.$setPristine();
            $scope.myForm6.$setUntouched();
        }
        $scope.particularpreReceipt = function (prepart) {
            if (prepart == 1 || prepart == 2 || prepart == 3) {
                $scope.ReceipttautoParti = true;
                $scope.Receipt.IMN_PrefixParticular = '';
            }
            else {
                $scope.ReceipttautoParti = false;

                if ($scope.Receipt.IMN_PrefixAcadYearCode == 0 && $scope.Receipt.IMN_SuffixAcadYearCode == 0) {
                    $scope.Receipt.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.Receipt.IMN_RestartNumFlag = null;

                    $scope.submittedReceipt = false;
                    $scope.myForm6.$setPristine();
                    $scope.myForm6.$setUntouched();
                }

            }

        }
        $scope.particularsufReceipt = function (sufpart) {
            if (sufpart == 1 || sufpart == 2 || sufpart == 3) {
                $scope.ReceipttautoPartisuf = true;
                $scope.Receipt.IMN_SuffixParticular = '';
            }
            else {
                $scope.ReceipttautoPartisuf = false;

                if ($scope.Receipt.IMN_PrefixAcadYearCode == 0 && $scope.Receipt.IMN_SuffixAcadYearCode == 0) {
                    $scope.Receipt.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.Receipt.IMN_RestartNumFlag = null;

                    $scope.submittedReceipt = false;
                    $scope.myForm6.$setPristine();
                    $scope.myForm6.$setUntouched();
                }
            }


        }

        $scope.submittedReceipt = false;
        $scope.SaveReceiptNumbering = function (Receipt) {

            if ($scope.myForm6.$valid) {
                var IMN_PrefixAcadYearCode;
                if ($scope.Receipt.IMN_PrefixAcadYearCode == "1") {
                    IMN_PrefixAcadYearCode = true;
                }
                else {
                    IMN_PrefixAcadYearCode = false;
                }

                var IMN_PrefixFinYearCode;
                if ($scope.Receipt.IMN_PrefixAcadYearCode == "2") {
                    IMN_PrefixFinYearCode = true;
                }
                else {
                    IMN_PrefixFinYearCode = false;
                }

                var IMN_PrefixCalYearCode;
                if ($scope.Receipt.IMN_PrefixAcadYearCode == "3") {
                    IMN_PrefixCalYearCode = true;
                }
                else {
                    IMN_PrefixCalYearCode = false;
                }

                var IMN_SuffixAcadYearCode;
                if ($scope.Receipt.IMN_SuffixAcadYearCode == "1") {
                    IMN_SuffixAcadYearCode = true;
                }
                else {
                    IMN_SuffixAcadYearCode = false;
                }

                var IMN_SuffixFinYearCode;
                if ($scope.Receipt.IMN_SuffixAcadYearCode == "2") {
                    IMN_SuffixFinYearCode = true;
                }
                else {
                    IMN_SuffixFinYearCode = false;
                }

                var IMN_SuffixCalYearCode;
                if ($scope.Receipt.IMN_SuffixAcadYearCode == "3") {
                    IMN_SuffixCalYearCode = true;
                }
                else {
                    IMN_SuffixCalYearCode = false;
                }


                swal({
                    title: "Are you sure?",
                    text: "Do you want to save Receipt Numbering Details ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {


                            var data6 = {
                                "IMN_Id": $scope.Receipt.IMN_Id,
                                "IMN_Flag": "Receipt",  //Receipt Numbering
                                "IMN_AutoManualFlag": $scope.Receipt.IMN_AutoManualFlag,
                                "IMN_DuplicatesFlag": $scope.Receipt.IMN_DuplicatesFlag,
                                "IMN_StartingNo": $scope.Receipt.IMN_StartingNo,
                                "IMN_WidthNumeric": $scope.Receipt.IMN_WidthNumeric,
                                "IMN_ZeroPrefixFlag": $scope.Receipt.IMN_ZeroPrefixFlag,

                                "IMN_PrefixAcadYearCode": IMN_PrefixAcadYearCode,
                                "IMN_PrefixParticular": $scope.Receipt.IMN_PrefixParticular,

                                "IMN_SuffixAcadYearCode": IMN_SuffixAcadYearCode,
                                "IMN_SuffixParticular": $scope.Receipt.IMN_SuffixParticular,
                                "IMN_RestartNumFlag": $scope.Receipt.IMN_RestartNumFlag,


                                "IMN_PrefixFinYearCode": IMN_PrefixFinYearCode,
                                "IMN_PrefixCalYearCode": IMN_PrefixCalYearCode,

                                "IMN_SuffixFinYearCode": IMN_SuffixFinYearCode,
                                "IMN_SuffixCalYearCode": IMN_SuffixCalYearCode,

                                "IRN_RestartAcadYear": $scope.Receipt.IRN_RestartAcadYear,
                                "IRN_RestartFinYear": $scope.Receipt.IRN_RestartFinYear,
                                "IRN_RestartcalendYear": $scope.Receipt.IRN_RestartcalendYear
                            }
                            apiService.create("TransactionNumbering/", data6).then(function (promise) {
                                $scope.details();
                                swal('Record Saved Successfully');
                            });
                        }
                        else {
                            swal("Cancelled");
                        }
                    });

            }
            else {
                $scope.submittedReceipt = true;
            }

        };
        $scope.ClearReceiptNumbering = function () {

            $scope.Receipt = {};
            $scope.submittedReceipt = false;

            $scope.Receipttmanual = true;
            $scope.ReceipttautoParti = true;
            $scope.ReceipttautoPartisuf = true;
            $scope.Receipttauto = true;
            $scope.myForm6.$setPristine();
            $scope.myForm6.$setUntouched();
        }

        $scope.OnchangeRestartNumReceipt = function (data) {
            if (data == "Never") {
                $scope.Receipt.IRN_RestartAcadYear = "";
                $scope.Receipt.IRN_RestartFinYear = "";
                $scope.Receipt.IRN_RestartcalendYear = "";
            }
        }

        //Voucher Numbering
        $scope.Vouchertmanual = true;
        $scope.VouchertautoParti = true;
        $scope.VouchertautoPartisuf = true;
        $scope.Voucherslnostarting = true;
        $scope.Vouchertauto = true;

        $scope.manualVoucher = function (obj) {
            if (obj == "Manual") {
                $scope.Vouchertmanual = false;
                $scope.Vouchertauto = true;
                $scope.VouchertautoParti = true;
                $scope.VouchertautoPartisuf = true;
                $scope.Voucher.IMN_RestartNumFlag = null;
                $scope.Voucher.IMN_PrefixFinYearCode = null;
                //$scope.Voucher.IRN_PrefixFinYearCode = null;
                // $scope.Voucher.IRN_PrefixCalYearCode = null;
                $scope.Voucher.IMN_PrefixParticular = '';
                $scope.Voucher.IMN_SuffixFinYearCode = null;
                // $scope.Voucher.IRN_SuffixFinYearCode = null;
                //  $scope.Voucher.IRN_SuffixCalYearCode = null;
                $scope.Voucher.IMN_SuffixParticular = '';
                $scope.Voucher.IMN_WidthNumeric = '';
                $scope.Voucher.IMN_StartingNo = '';
                $scope.Voucher.IMN_ZeroPrefixFlag = '';

            }
            else {
                $scope.Vouchertmanual = true;
                $scope.Vouchertauto = false;
                $scope.VouchertautoParti = true;
                $scope.VouchertautoPartisuf = true;
                $scope.Voucher.IMN_DuplicatesFlag = '';

            }
            $scope.submittedVoucher = false;
            $scope.myForm7.$setPristine();
            $scope.myForm7.$setUntouched();
        }
        $scope.particularpreVoucher = function (prepart) {
            if (prepart == 1) {
                $scope.VouchertautoParti = true;
                $scope.Voucher.IMN_PrefixParticular = '';
            }
            else {
                $scope.VouchertautoParti = false;

                if (($scope.Voucher.IMN_PrefixFinYearCode == 0) && ($scope.Voucher.IMN_SuffixFinYearCode == 0)) {
                    $scope.Voucher.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.Voucher.IMN_RestartNumFlag = null;

                    $scope.submittedVoucher = false;
                    $scope.myForm7.$setPristine();
                    $scope.myForm7.$setUntouched();
                }

            }

        }
        $scope.particularsufVoucher = function (sufpart) {
            if (sufpart == 1) {
                $scope.VouchertautoPartisuf = true;
                $scope.Voucher.IMN_SuffixParticular = '';
            }
            else {
                $scope.VouchertautoPartisuf = false;

                if ($scope.Voucher.IMN_PrefixFinYearCode == 0 && $scope.Voucher.IMN_SuffixFinYearCode == 0) {
                    $scope.Voucher.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.Voucher.IMN_RestartNumFlag = null;

                    $scope.submittedVoucher = false;
                    $scope.myForm7.$setPristine();
                    $scope.myForm7.$setUntouched();
                }
            }


        }

        $scope.submittedVoucher = false;
        $scope.SaveVoucherNumbering = function (Voucher) {

            if ($scope.myForm7.$valid) {
                var IMN_PrefixFinYearCode;
                if ($scope.Voucher.IMN_PrefixFinYearCode == "1") {
                    IMN_PrefixFinYearCode = true;
                }
                else {
                    IMN_PrefixFinYearCode = false;
                }



                var IMN_SuffixFinYearCode;
                if ($scope.Voucher.IMN_SuffixFinYearCode == "1") {
                    IMN_SuffixFinYearCode = true;
                }
                else {
                    IMN_SuffixFinYearCode = false;
                }


                swal({
                    title: "Are you sure?",
                    text: "Do you want to save Voucher Numbering Details ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {

                            var data7 = {
                                "IMN_Id": $scope.Voucher.IMN_Id,
                                "IMN_Flag": "Voucher",  //Voucher Numbering
                                "VoucherName": $scope.Voucher.VoucherName,
                                "VoucherType": $scope.Voucher.VoucherType,
                                "Observation": $scope.Voucher.Observation,
                                "IMN_AutoManualFlag": $scope.Voucher.IMN_AutoManualFlag,
                                "IMN_DuplicatesFlag": $scope.Voucher.IMN_DuplicatesFlag,
                                "IMN_StartingNo": $scope.Voucher.IMN_StartingNo,
                                "IMN_WidthNumeric": $scope.Voucher.IMN_WidthNumeric,
                                "IMN_ZeroPrefixFlag": $scope.Voucher.IMN_ZeroPrefixFlag,
                                // "IMN_PrefixAcadYearCode": IMN_PrefixAcadYearCode,
                                "IMN_PrefixParticular": $scope.Voucher.IMN_PrefixParticular,
                                // "IMN_SuffixAcadYearCode": IMN_SuffixAcadYearCode,
                                "IMN_SuffixParticular": $scope.Voucher.IMN_SuffixParticular,
                                "IMN_RestartNumFlag": $scope.Voucher.IMN_RestartNumFlag,

                                "IMN_PrefixFinYearCode": IMN_PrefixFinYearCode,
                                // "IMN_PrefixCalYearCode": IMN_PrefixCalYearCode,

                                "IMN_SuffixFinYearCode": IMN_SuffixFinYearCode,
                                // "IMN_SuffixCalYearCode": IMN_SuffixCalYearCode
                            }
                            apiService.create("TransactionNumbering/", data7).then(function (promise) {
                                $scope.details();
                                swal('Record Saved Successfully');
                            });
                        }
                        else {
                            swal("Cancelled");
                        }
                    });

            }
            else {
                $scope.submittedVoucher = true;
            }

        };
        $scope.ClearVoucherNumbering = function () {

            $scope.Voucher = {};
            $scope.submittedVoucher = false;

            $scope.Vouchertmanual = true;
            $scope.VouchertautoParti = true;
            $scope.VouchertautoPartisuf = true;
            $scope.Vouchertauto = true;
            $scope.myForm7.$setPristine();
            $scope.myForm7.$setUntouched();
        }


        //Transaction Numbering

        /////Transaction Numbering
        $scope.Transactiontmanual = true;
        $scope.TransactiontautoParti = true;
        $scope.TransactiontautoPartisuf = true;
        $scope.Transactionslnostarting = true;
        $scope.Transactiontauto = true;

        $scope.manualTransaction = function (obj) {
            if (obj == "Manual") {
                $scope.Transactiontmanual = false;
                $scope.Transactiontauto = true;
                $scope.TransactiontautoParti = true;
                $scope.TransactiontautoPartisuf = true;
                $scope.Transaction.IMN_RestartNumFlag = null;
                $scope.Transaction.IMN_PrefixAcadYearCode = null;
                //$scope.Transaction.IRN_PrefixFinYearCode = null;
                // $scope.Transaction.IRN_PrefixCalYearCode = null;
                $scope.Transaction.IMN_PrefixParticular = '';
                $scope.Transaction.IMN_SuffixAcadYearCode = null;
                // $scope.Transaction.IRN_SuffixFinYearCode = null;
                //  $scope.Transaction.IRN_SuffixCalYearCode = null;
                $scope.Transaction.IMN_SuffixParticular = '';
                $scope.Transaction.IMN_WidthNumeric = '';
                $scope.Transaction.IMN_StartingNo = '';
                $scope.Transaction.IMN_ZeroPrefixFlag = '';



            }
            else {
                $scope.Transactiontmanual = true;
                $scope.Transactiontauto = false;
                $scope.TransactiontautoParti = true;
                $scope.TransactiontautoPartisuf = true;
                $scope.Transaction.IMN_DuplicatesFlag = '';

            }
            $scope.submittedTransaction = false;
            $scope.myForm8.$setPristine();
            $scope.myForm8.$setUntouched();
        }
        $scope.particularpreTransaction = function (prepart) {
            if (prepart == 1 || prepart == 2 || prepart == 3) {
                $scope.TransactiontautoParti = true;
                $scope.Transaction.IMN_PrefixParticular = '';
            }
            else {
                $scope.TransactiontautoParti = false;

                if (($scope.Transaction.IMN_PrefixAcadYearCode == 0) && ($scope.Transaction.IMN_SuffixAcadYearCode == 0)) {
                    $scope.Transaction.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.Transaction.IMN_RestartNumFlag = null;

                    $scope.submittedTransaction = false;
                    $scope.myForm8.$setPristine();
                    $scope.myForm8.$setUntouched();
                }

            }

        }
        $scope.particularsufTransaction = function (sufpart) {
            if (sufpart == 1 || sufpart == 2 || sufpart == 3) {
                $scope.TransactiontautoPartisuf = true;
                $scope.Transaction.IMN_SuffixParticular = '';
            }
            else {
                $scope.TransactiontautoPartisuf = false;

                if ($scope.Transaction.IMN_PrefixAcadYearCode == 0 && $scope.Transaction.IMN_SuffixAcadYearCode == 0) {
                    $scope.Transaction.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.Transaction.IMN_RestartNumFlag = null;

                    $scope.submittedTransaction = false;
                    $scope.myForm8.$setPristine();
                    $scope.myForm8.$setUntouched();
                }
            }


        }

        $scope.submittedTransaction = false;
        $scope.SaveTransactionNumbering = function (Transaction) {

            if ($scope.myForm8.$valid) {
                var IMN_PrefixAcadYearCode;
                if ($scope.Transaction.IMN_PrefixAcadYearCode == "1") {
                    IMN_PrefixAcadYearCode = true;
                }
                else {
                    IMN_PrefixAcadYearCode = false;
                }

                var IMN_PrefixFinYearCode;
                if ($scope.Transaction.IMN_PrefixAcadYearCode == "2") {
                    IMN_PrefixFinYearCode = true;
                }
                else {
                    IMN_PrefixFinYearCode = false;
                }

                var IMN_PrefixCalYearCode;
                if ($scope.Transaction.IMN_PrefixAcadYearCode == "3") {
                    IMN_PrefixCalYearCode = true;
                }
                else {
                    IMN_PrefixCalYearCode = false;
                }

                var IMN_SuffixAcadYearCode;
                if ($scope.Transaction.IMN_SuffixAcadYearCode == "1") {
                    IMN_SuffixAcadYearCode = true;
                }
                else {
                    IMN_SuffixAcadYearCode = false;
                }

                var IMN_SuffixFinYearCode;
                if ($scope.Transaction.IMN_SuffixAcadYearCode == "2") {
                    IMN_SuffixFinYearCode = true;
                }
                else {
                    IMN_SuffixFinYearCode = false;
                }

                var IMN_SuffixCalYearCode;
                if ($scope.Transaction.IMN_SuffixAcadYearCode == "3") {
                    IMN_SuffixCalYearCode = true;
                }
                else {
                    IMN_SuffixCalYearCode = false;
                }

                swal({
                    title: "Are you sure?",
                    text: "Do you want to save Transaction Numbering Details ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {

                            var data8 = {
                                "IMN_Id": $scope.Transaction.IMN_Id,
                                "IMN_Flag": "Transaction",  //Transaction Numbering
                                "IMN_AutoManualFlag": $scope.Transaction.IMN_AutoManualFlag,
                                "IMN_DuplicatesFlag": $scope.Transaction.IMN_DuplicatesFlag,
                                "IMN_StartingNo": $scope.Transaction.IMN_StartingNo,
                                "IMN_WidthNumeric": $scope.Transaction.IMN_WidthNumeric,
                                "IMN_ZeroPrefixFlag": $scope.Transaction.IMN_ZeroPrefixFlag,
                                "IMN_PrefixAcadYearCode": IMN_PrefixAcadYearCode,
                                "IMN_PrefixParticular": $scope.Transaction.IMN_PrefixParticular,
                                "IMN_SuffixAcadYearCode": IMN_SuffixAcadYearCode,
                                "IMN_SuffixParticular": $scope.Transaction.IMN_SuffixParticular,
                                "IMN_RestartNumFlag": $scope.Transaction.IMN_RestartNumFlag,

                                "IMN_PrefixFinYearCode": IMN_PrefixFinYearCode,
                                "IMN_PrefixCalYearCode": IMN_PrefixCalYearCode,

                                "IMN_SuffixFinYearCode": IMN_SuffixFinYearCode,
                                "IMN_SuffixCalYearCode": IMN_SuffixCalYearCode
                            }
                            apiService.create("TransactionNumbering/", data8).then(function (promise) {
                                $scope.details();
                                swal('Record Saved Successfully');
                            });
                        }
                        else {
                            swal("Cancelled");
                        }
                    });

            }
            else {
                $scope.submittedTransaction = true;
            }

        };
        $scope.ClearTransactionNumbering = function () {

            $scope.Transaction = {};
            $scope.submittedTransaction = false;

            $scope.Transactiontmanual = true;
            $scope.TransactiontautoParti = true;
            $scope.TransactiontautoPartisuf = true;
            $scope.Transactiontauto = true;
            $scope.myForm8.$setPristine();
            $scope.myForm8.$setUntouched();
        }



        //Application Numbering

        /////Application Numbering
        $scope.Applicationtmanual = true;
        $scope.ApplicationtautoParti = true;
        $scope.ApplicationtautoPartisuf = true;
        $scope.Applicationslnostarting = true;
        $scope.Applicationtauto = true;

        $scope.manualApplication = function (obj) {
            if (obj == "Manual") {
                $scope.Applicationtmanual = false;
                $scope.Applicationtauto = true;
                $scope.ApplicationtautoParti = true;
                $scope.ApplicationtautoPartisuf = true;
                $scope.Application.IMN_RestartNumFlag = null;
                $scope.Application.IMN_PrefixAcadYearCode = null;
                //$scope.Application.IRN_PrefixFinYearCode = null;
                // $scope.Application.IRN_PrefixCalYearCode = null;
                $scope.Application.IMN_PrefixParticular = '';
                $scope.Application.IMN_SuffixAcadYearCode = null;
                // $scope.Application.IRN_SuffixFinYearCode = null;
                //  $scope.Application.IRN_SuffixCalYearCode = null;
                $scope.Application.IMN_SuffixParticular = '';
                $scope.Application.IMN_WidthNumeric = '';
                $scope.Application.IMN_StartingNo = '';
                $scope.Application.IMN_ZeroPrefixFlag = '';

            }
            else {
                $scope.Applicationtmanual = true;
                $scope.Applicationtauto = false;
                $scope.ApplicationtautoParti = true;
                $scope.ApplicationtautoPartisuf = true;
                $scope.Application.IMN_DuplicatesFlag = '';

            }
            $scope.submittedApplication = false;
            $scope.myForm9.$setPristine();
            $scope.myForm9.$setUntouched();
        }
        $scope.particularpreApplication = function (prepart) {
            if (prepart == 1 || prepart == 2 || prepart == 3) {
                $scope.ApplicationtautoParti = true;
                $scope.Application.IMN_PrefixParticular = '';
            }
            else {
                $scope.ApplicationtautoParti = false;

                if (($scope.Application.IMN_PrefixAcadYearCode == 0) && ($scope.Application.IMN_SuffixAcadYearCode == 0)) {
                    $scope.Application.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.Application.IMN_RestartNumFlag = null;

                    $scope.submittedApplication = false;
                    $scope.myForm9.$setPristine();
                    $scope.myForm9.$setUntouched();
                }

            }

        }
        $scope.particularsufApplication = function (sufpart) {
            if (sufpart == 1 || sufpart == 2 || sufpart == 3) {
                $scope.ApplicationtautoPartisuf = true;
                $scope.Application.IMN_SuffixParticular = '';
            }
            else {
                $scope.ApplicationtautoPartisuf = false;

                if ($scope.Application.IMN_PrefixAcadYearCode == 0 && $scope.Application.IMN_SuffixAcadYearCode == 0) {
                    $scope.Application.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.Application.IMN_RestartNumFlag = null;

                    $scope.submittedApplication = false;
                    $scope.myForm9.$setPristine();
                    $scope.myForm9.$setUntouched();
                }
            }


        }

        $scope.submittedApplication = false;
        $scope.SaveApplicationNumbering = function (Application) {

            if ($scope.myForm9.$valid) {
                var IMN_PrefixAcadYearCode;
                if ($scope.Application.IMN_PrefixAcadYearCode == "1") {
                    IMN_PrefixAcadYearCode = true;
                }
                else {
                    IMN_PrefixAcadYearCode = false;
                }

                var IMN_PrefixFinYearCode;
                if ($scope.Application.IMN_PrefixAcadYearCode == "2") {
                    IMN_PrefixFinYearCode = true;
                }
                else {
                    IMN_PrefixFinYearCode = false;
                }

                var IMN_PrefixCalYearCode;
                if ($scope.Application.IMN_PrefixAcadYearCode == "3") {
                    IMN_PrefixCalYearCode = true;
                }
                else {
                    IMN_PrefixCalYearCode = false;
                }

                var IMN_SuffixAcadYearCode;
                if ($scope.Application.IMN_SuffixAcadYearCode == "1") {
                    IMN_SuffixAcadYearCode = true;
                }
                else {
                    IMN_SuffixAcadYearCode = false;
                }

                var IMN_SuffixFinYearCode;
                if ($scope.Application.IMN_SuffixAcadYearCode == "2") {
                    IMN_SuffixFinYearCode = true;
                }
                else {
                    IMN_SuffixFinYearCode = false;
                }

                var IMN_SuffixCalYearCode;
                if ($scope.Application.IMN_SuffixAcadYearCode == "3") {
                    IMN_SuffixCalYearCode = true;
                }
                else {
                    IMN_SuffixCalYearCode = false;
                }

                swal({
                    title: "Are you sure?",
                    text: "Do you want to save Application Numbering Details ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {

                            var data9 = {
                                "IMN_Id": $scope.Application.IMN_Id,
                                "IMN_Flag": "Application",  //Application Numbering
                                "IMN_AutoManualFlag": $scope.Application.IMN_AutoManualFlag,
                                "IMN_DuplicatesFlag": $scope.Application.IMN_DuplicatesFlag,
                                "IMN_StartingNo": $scope.Application.IMN_StartingNo,
                                "IMN_WidthNumeric": $scope.Application.IMN_WidthNumeric,
                                "IMN_ZeroPrefixFlag": $scope.Application.IMN_ZeroPrefixFlag,
                                "IMN_PrefixAcadYearCode": IMN_PrefixAcadYearCode,
                                "IMN_PrefixParticular": $scope.Application.IMN_PrefixParticular,
                                "IMN_SuffixAcadYearCode": IMN_SuffixAcadYearCode,
                                "IMN_SuffixParticular": $scope.Application.IMN_SuffixParticular,
                                "IMN_RestartNumFlag": $scope.Application.IMN_RestartNumFlag,

                                "IMN_PrefixFinYearCode": IMN_PrefixFinYearCode,
                                "IMN_PrefixCalYearCode": IMN_PrefixCalYearCode,

                                "IMN_SuffixFinYearCode": IMN_SuffixFinYearCode,
                                "IMN_SuffixCalYearCode": IMN_SuffixCalYearCode
                            }
                            apiService.create("TransactionNumbering/", data9).then(function (promise) {
                                $scope.details();
                                swal('Record Saved Successfully');
                            });
                        }
                        else {
                            swal("Cancelled");
                        }
                    });

            }
            else {
                $scope.submittedApplication = true;
            }

        };

        //-----------------tc no generating----------------
        $scope.savetcNumbering = function (tcno) {

            if ($scope.myform123.$valid) {
                var IMN_PrefixAcadYearCode;
                if ($scope.tcno.IMN_PrefixAcadYearCode == "1") {
                    IMN_PrefixAcadYearCode = true;
                }
                else {
                    IMN_PrefixAcadYearCode = false;
                }



                var IMN_SuffixAcadYearCode;
                if ($scope.tcno.IMN_SuffixAcadYearCode == "1") {
                    IMN_SuffixAcadYearCode = true;
                }
                else {
                    IMN_SuffixAcadYearCode = false;
                }



                swal({
                    title: "Are you sure?",
                    text: "Do you want to save TC Numbering Details ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {

                            var data10 = {
                                "IMN_Id": $scope.tcno.IMN_Id,
                                "IMN_Flag": "tcno",  //tcno Numbering
                                "IMN_AutoManualFlag": $scope.tcno.IMN_AutoManualFlag,
                                "IMN_DuplicatesFlag": $scope.tcno.IMN_DuplicatesFlag,
                                "IMN_StartingNo": $scope.tcno.IMN_StartingNo,
                                "IMN_WidthNumeric": $scope.tcno.IMN_WidthNumeric,
                                "IMN_ZeroPrefixFlag": $scope.tcno.IMN_ZeroPrefixFlag,
                                "IMN_PrefixAcadYearCode": IMN_PrefixAcadYearCode,
                                "IMN_PrefixParticular": $scope.tcno.IMN_PrefixParticular,
                                "IMN_SuffixAcadYearCode": IMN_SuffixAcadYearCode,
                                "IMN_SuffixParticular": $scope.tcno.IMN_SuffixParticular,
                                "IMN_RestartNumFlag": $scope.tcno.IMN_RestartNumFlag,

                                //"IMN_PrefixFinYearCode": IMN_PrefixFinYearCode,
                                //"IMN_PrefixCalYearCode": IMN_PrefixCalYearCode,

                                //"IMN_SuffixFinYearCode": IMN_SuffixFinYearCode,
                                //"IMN_SuffixCalYearCode": IMN_SuffixCalYearCode
                            }
                            apiService.create("TransactionNumbering/", data10).then(function (promise) {
                                $scope.details();
                                swal('Record Saved Successfully');
                            });
                        }
                        else {
                            swal("Cancelled");
                        }
                    });

            }
            else {
                $scope.submittedtc = true;
            }

        };
        ////------------------ tc clear ----------------------
        $scope.CleartcNumbering = function () {

            $scope.tcno = {};
            $scope.submittedApplication = false;

            $scope.tcnomanual = true;
            $scope.tcnoautoParti = true;
            $scope.tcnotautoPartisuf = true;
            $scope.tcnoauto = true;
            $scope.myform123.$setPristine();
            $scope.myform123.$setUntouched();
        }

        $scope.ClearApplicationNumbering = function () {

            $scope.tcno = {};
            $scope.submittedApplication = false;

            $scope.Applicationtmanual = true;
            $scope.ApplicationtautoParti = true;
            $scope.ApplicationtautoPartisuf = true;
            $scope.Applicationtauto = true;
            $scope.myForm9.$setPristine();
            $scope.myForm9.$setUntouched();
        }





        //---- Loan --------


        $scope.loantmanual = true;
        $scope.loantautoParti = true;
        $scope.loantautoPartisuf = true;
        $scope.loanslnostarting = true;
        $scope.loantauto = true;

        $scope.manualloan = function (obj) {
            if (obj == "Manual") {
                $scope.loantmanual = false;
                $scope.loantauto = true;
                $scope.loantautoParti = true;
                $scope.loantautoPartisuf = true;
                $scope.Loan.IMN_RestartNumFlag = null;
                $scope.Loan.IMN_PrefixAcadYearCode = null;
                $scope.Loan.IMN_PrefixParticular = '';
                $scope.Loan.IMN_SuffixAcadYearCode = null;
                $scope.Loan.IMN_SuffixParticular = '';
                $scope.Loan.IMN_WidthNumeric = '';
                $scope.Loan.IMN_StartingNo = '';
                $scope.Loan.IMN_ZeroPrefixFlag = '';

            }
            else {
                $scope.loantmanual = true;
                $scope.loantauto = false;
                $scope.loantautoParti = true;
                $scope.loantautoPartisuf = true;
                $scope.Loan.IMN_DuplicatesFlag = '';

            }
            $scope.submittedLoan = false;
            $scope.myFormLoan.$setPristine();
            $scope.myFormLoan.$setUntouched();
        }
        $scope.particularpreloan = function (prepart) {
            if (prepart == 1) {
                $scope.loantautoParti = true;
            }
            else {
                $scope.loantautoParti = false;

                if ($scope.Loan.IMN_PrefixAcadYearCode == 0 && $scope.Loan.IMN_SuffixAcadYearCode == 0) {
                    $scope.Loan.IMN_RestartNumFlag = "Never";

                }
                else {
                    $scope.Loan.IMN_RestartNumFlag = null;
                    $scope.submittedLoan = false;
                    $scope.myFormLoan.$setPristine();
                    $scope.myFormLoan.$setUntouched();
                }
            }


        }
        $scope.particularsufloan = function (sufpart) {
            if (sufpart == 1) {
                $scope.loantautoPartisuf = true;
            }
            else {
                $scope.loantautoPartisuf = false;

                if ($scope.Loan.IMN_PrefixAcadYearCode == 0 && $scope.Loan.IMN_SuffixAcadYearCode == 0) {
                    $scope.Loan.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.Loan.IMN_RestartNumFlag = null;

                    $scope.submittedLoan = false;
                    $scope.myFormLoan.$setPristine();
                    $scope.myFormLoan.$setUntouched();

                }
            }

        }



        $scope.submittedLoan = false;
        $scope.SaveLoanNumbering = function (Loan) {

            if ($scope.myFormLoan.$valid) {
                var IMN_PrefixAcadYearCode;
                if ($scope.Loan.IMN_PrefixAcadYearCode == "0") {
                    IMN_PrefixAcadYearCode = false;
                }
                else {
                    IMN_PrefixAcadYearCode = true;
                }
                var IMN_SuffixAcadYearCode;
                if ($scope.Loan.IMN_SuffixAcadYearCode == "0") {
                    IMN_SuffixAcadYearCode = false;
                }
                else {
                    IMN_SuffixAcadYearCode = true;
                }

                swal({
                    title: "Are you sure?",
                    text: "Do you want to save  Loan  Details ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {

                            var data1 = {
                                "IMN_Id": $scope.Loan.IMN_Id,
                                "IMN_Flag": "Loan",  //Loan Numbering
                                "IMN_AutoManualFlag": $scope.Loan.IMN_AutoManualFlag,
                                "IMN_DuplicatesFlag": $scope.Loan.IMN_DuplicatesFlag,
                                "IMN_StartingNo": $scope.Loan.IMN_StartingNo,
                                "IMN_WidthNumeric": $scope.Loan.IMN_WidthNumeric,
                                "IMN_ZeroPrefixFlag": $scope.Loan.IMN_ZeroPrefixFlag,
                                "IMN_PrefixAcadYearCode": IMN_PrefixAcadYearCode,
                                "IMN_PrefixParticular": $scope.Loan.IMN_PrefixParticular,
                                "IMN_SuffixAcadYearCode": IMN_SuffixAcadYearCode,
                                "IMN_SuffixParticular": $scope.Loan.IMN_SuffixParticular,
                                "IMN_RestartNumFlag": $scope.Loan.IMN_RestartNumFlag,
                                //"IMN_RestartAcadYear": $scope.Loan.IMN_RestartAcadYear
                            }
                            apiService.create("TransactionNumbering/", data1).then(function (promise) {
                                $scope.ClearLoanNumbering();
                                $scope.details();
                                swal('Record Saved Successfully');
                            });
                        }
                        else {
                            swal("Cancelled");
                        }
                    });

            }
            else {
                $scope.submittedLoan = true;
            }

        };
        $scope.ClearLoanNumbering = function () {

            $scope.Loan = {};
            $scope.submittedLoan = false;

            $scope.loantmanual = true;
            $scope.loantautoParti = true;
            $scope.loantautoPartisuf = true;
            $scope.loantauto = true;
            $scope.myFormLoan.$setPristine();
            $scope.myFormLoan.$setUntouched();
        }


        //TripOnlineBooking Numbering

        /////TripOnlineBooking Numbering
        $scope.OnlineBookingmanual = true;
        $scope.OnlineBookingautoParti = true;
        $scope.OnlineBookingautoPartisuf = true;
        $scope.OnlineBookingslnostarting = true;
        $scope.OnlineBookingauto = true;

        $scope.manualOnlineBooking = function (obj) {
            if (obj == "Manual") {
                $scope.OnlineBookingmanual = false;
                $scope.OnlineBookingauto = true;
                $scope.OnlineBookingautoParti = true;
                $scope.OnlineBookingautoPartisuf = true;
                $scope.OnlineBooking.IMN_RestartNumFlag = null;
                $scope.OnlineBooking.IMN_PrefixAcadYearCode = null;
                // $scope.Receipt.IRN_PrefixFinYearCode = null;
                // $scope.Receipt.IRN_PrefixCalYearCode = null;
                $scope.OnlineBooking.IMN_PrefixParticular = '';
                $scope.OnlineBooking.IMN_SuffixAcadYearCode = null;
                //$scope.Receipt.IRN_SuffixFinYearCode = null;
                // $scope.Receipt.IRN_SuffixCalYearCode = null;
                $scope.OnlineBooking.IMN_SuffixParticular = '';
                $scope.OnlineBooking.IMN_WidthNumeric = '';
                $scope.OnlineBooking.IMN_StartingNo = '';
                $scope.OnlineBooking.IMN_ZeroPrefixFlag = '';
            }
            else {
                $scope.OnlineBookingmanual = true;
                $scope.OnlineBookingauto = false;
                $scope.OnlineBookingautoParti = true;
                $scope.OnlineBookingautoPartisuf = true;
                $scope.OnlineBooking.IMN_DuplicatesFlag = '';

            }
            $scope.submittedOnlineBooking = false;
            $scope.myForm10.$setPristine();
            $scope.myForm10.$setUntouched();
        }
        $scope.particularpreOnlineBooking = function (prepart) {
            if (prepart == 1 || prepart == 2 || prepart == 3) {
                $scope.OnlineBookingautoParti = true;
                $scope.OnlineBooking.IMN_PrefixParticular = '';
            }
            else {
                $scope.OnlineBookingautoParti = false;

                if ($scope.OnlineBooking.IMN_PrefixAcadYearCode == 0 && $scope.OnlineBooking.IMN_SuffixAcadYearCode == 0) {
                    $scope.OnlineBooking.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.OnlineBooking.IMN_RestartNumFlag = null;

                    $scope.submittedOnlineBooking = false;
                    $scope.myForm10.$setPristine();
                    $scope.myForm10.$setUntouched();
                }

            }

        }
        $scope.particularsufOnlineBooking = function (sufpart) {
            if (sufpart == 1 || sufpart == 2 || sufpart == 3) {
                $scope.OnlineBookingautoPartisuf = true;
                $scope.OnlineBooking.IMN_SuffixParticular = '';
            }
            else {
                $scope.OnlineBookingautoPartisuf = false;

                if ($scope.OnlineBooking.IMN_PrefixAcadYearCode == 0 && $scope.OnlineBooking.IMN_SuffixAcadYearCode == 0) {
                    $scope.OnlineBooking.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.OnlineBooking.IMN_RestartNumFlag = null;

                    $scope.submittedOnlineBooking = false;
                    $scope.myForm10.$setPristine();
                    $scope.myForm10.$setUntouched();
                }
            }


        }

        $scope.submittedOnlineBooking = false;
        $scope.SaveOnlineBookingNumbering = function (book) {

            if ($scope.myForm10.$valid) {
                var IMN_PrefixAcadYearCode;
                if ($scope.OnlineBooking.IMN_PrefixAcadYearCode == "1") {
                    IMN_PrefixAcadYearCode = true;
                }
                else {
                    IMN_PrefixAcadYearCode = false;
                }

                var IMN_PrefixFinYearCode;
                if ($scope.OnlineBooking.IMN_PrefixAcadYearCode == "2") {
                    IMN_PrefixFinYearCode = true;
                }
                else {
                    IMN_PrefixFinYearCode = false;
                }

                var IMN_PrefixCalYearCode;
                if ($scope.OnlineBooking.IMN_PrefixAcadYearCode == "3") {
                    IMN_PrefixCalYearCode = true;
                }
                else {
                    IMN_PrefixCalYearCode = false;
                }

                var IMN_SuffixAcadYearCode;
                if ($scope.OnlineBooking.IMN_SuffixAcadYearCode == "1") {
                    IMN_SuffixAcadYearCode = true;
                }
                else {
                    IMN_SuffixAcadYearCode = false;
                }

                var IMN_SuffixFinYearCode;
                if ($scope.OnlineBooking.IMN_SuffixAcadYearCode == "2") {
                    IMN_SuffixFinYearCode = true;
                }
                else {
                    IMN_SuffixFinYearCode = false;
                }

                var IMN_SuffixCalYearCode;
                if ($scope.OnlineBooking.IMN_SuffixAcadYearCode == "3") {
                    IMN_SuffixCalYearCode = true;
                }
                else {
                    IMN_SuffixCalYearCode = false;
                }


                swal({
                    title: "Are you sure?",
                    text: "Do you want to save TripOnlineBooking Numbering Details ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {


                            var data10 = {
                                "IMN_Id": $scope.OnlineBooking.IMN_Id,
                                "IMN_Flag": "TripOnlineBooking",  //TripOnlineBooking Numbering
                                "IMN_AutoManualFlag": $scope.OnlineBooking.IMN_AutoManualFlag,
                                "IMN_DuplicatesFlag": $scope.OnlineBooking.IMN_DuplicatesFlag,
                                "IMN_StartingNo": $scope.OnlineBooking.IMN_StartingNo,
                                "IMN_WidthNumeric": $scope.OnlineBooking.IMN_WidthNumeric,
                                "IMN_ZeroPrefixFlag": $scope.OnlineBooking.IMN_ZeroPrefixFlag,

                                "IMN_PrefixAcadYearCode": IMN_PrefixAcadYearCode,
                                "IMN_PrefixParticular": $scope.OnlineBooking.IMN_PrefixParticular,

                                "IMN_SuffixAcadYearCode": IMN_SuffixAcadYearCode,
                                "IMN_SuffixParticular": $scope.OnlineBooking.IMN_SuffixParticular,
                                "IMN_RestartNumFlag": "Never",


                                "IMN_PrefixFinYearCode": IMN_PrefixFinYearCode,
                                "IMN_PrefixCalYearCode": IMN_PrefixCalYearCode,

                                "IMN_SuffixFinYearCode": IMN_SuffixFinYearCode,
                                "IMN_SuffixCalYearCode": IMN_SuffixCalYearCode,

                            }
                            apiService.create("TransactionNumbering/", data10).then(function (promise) {
                                $scope.details();
                                swal('Record Saved Successfully');
                            });
                        }
                        else {
                            swal("Cancelled");
                        }
                    });

            }
            else {
                $scope.submittedOnlineBooking = true;
            }

        };
        $scope.ClearOnlineBookingNumbering = function () {

            $scope.OnlineBooking = {};
            $scope.submittedOnlineBooking = false;

            $scope.OnlineBookingmanual = true;
            $scope.OnlineBookingautoParti = true;
            $scope.OnlineBookingautoPartisuf = true;
            $scope.OnlineBookingauto = true;
            $scope.myForm10.$setPristine();
            $scope.myForm10.$setUntouched();
        }

        $scope.OnchangeRestartNumOnlineBooking = function (data) {
            if (data == "Never") {
                $scope.OnlineBooking.IRN_RestartAcadYear = "";
                $scope.OnlineBooking.IRN_RestartFinYear = "";
                $scope.OnlineBooking.IRN_RestartcalendYear = "";
            }
        }
        //Trip numbering.

        $scope.Tripmanual = true;
        $scope.TripautoParti = true;
        $scope.TripautoPartisuf = true;
        $scope.Tripslnostarting = true;
        $scope.Tripauto = true;

        $scope.manualTrip = function (obj) {
            if (obj == "Manual") {
                $scope.Tripmanual = false;
                $scope.Tripauto = true;
                $scope.TripautoParti = true;
                $scope.TripautoPartisuf = true;
                $scope.Trip.IMN_RestartNumFlag = null;
                $scope.Trip.IMN_PrefixAcadYearCode = null;

                $scope.Trip.IMN_PrefixParticular = '';
                $scope.Trip.IMN_SuffixAcadYearCode = null;

                $scope.Trip.IMN_SuffixParticular = '';
                $scope.Trip.IMN_WidthNumeric = '';
                $scope.Trip.IMN_StartingNo = '';
                $scope.Trip.IMN_ZeroPrefixFlag = '';
            }
            else {
                $scope.Tripmanual = true;
                $scope.Tripauto = false;
                $scope.TripautoParti = true;
                $scope.TripautoPartisuf = true;
                $scope.Trip.IMN_DuplicatesFlag = '';

            }
            $scope.submittedTrip = false;
            $scope.myForm11.$setPristine();
            $scope.myForm11.$setUntouched();
        }
        $scope.particularpreTrip = function (prepart) {
            if (prepart == 1 || prepart == 2 || prepart == 3) {
                $scope.TripautoParti = true;
                $scope.Trip.IMN_PrefixParticular = '';
            }
            else {
                $scope.TripautoParti = false;

                if ($scope.Trip.IMN_PrefixAcadYearCode == 0 && $scope.Trip.IMN_SuffixAcadYearCode == 0) {
                    $scope.Trip.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.Trip.IMN_RestartNumFlag = null;

                    $scope.submittedTrip = false;
                    $scope.myForm11.$setPristine();
                    $scope.myForm11.$setUntouched();
                }

            }

        }
        $scope.particularsufTrip = function (sufpart) {
            if (sufpart == 1 || sufpart == 2 || sufpart == 3) {
                $scope.TripautoPartisuf = true;
                $scope.Trip.IMN_SuffixParticular = '';
            }
            else {
                $scope.TripautoPartisuf = false;

                if ($scope.Trip.IMN_PrefixAcadYearCode == 0 && $scope.Trip.IMN_SuffixAcadYearCode == 0) {
                    $scope.Trip.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.Trip.IMN_RestartNumFlag = null;

                    $scope.submittedTrip = false;
                    $scope.myForm11.$setPristine();
                    $scope.myForm11.$setUntouched();
                }
            }


        }

        $scope.submittedTrip = false;
        $scope.SaveTripNumbering = function (book) {

            if ($scope.myForm11.$valid) {
                var IMN_PrefixAcadYearCode;
                if ($scope.Trip.IMN_PrefixAcadYearCode == "1") {
                    IMN_PrefixAcadYearCode = true;
                }
                else {
                    IMN_PrefixAcadYearCode = false;
                }

                var IMN_PrefixFinYearCode;
                if ($scope.Trip.IMN_PrefixAcadYearCode == "2") {
                    IMN_PrefixFinYearCode = true;
                }
                else {
                    IMN_PrefixFinYearCode = false;
                }

                var IMN_PrefixCalYearCode;
                if ($scope.Trip.IMN_PrefixAcadYearCode == "3") {
                    IMN_PrefixCalYearCode = true;
                }
                else {
                    IMN_PrefixCalYearCode = false;
                }

                var IMN_SuffixAcadYearCode;
                if ($scope.Trip.IMN_SuffixAcadYearCode == "1") {
                    IMN_SuffixAcadYearCode = true;
                }
                else {
                    IMN_SuffixAcadYearCode = false;
                }

                var IMN_SuffixFinYearCode;
                if ($scope.Trip.IMN_SuffixAcadYearCode == "2") {
                    IMN_SuffixFinYearCode = true;
                }
                else {
                    IMN_SuffixFinYearCode = false;
                }

                var IMN_SuffixCalYearCode;
                if ($scope.Trip.IMN_SuffixAcadYearCode == "3") {
                    IMN_SuffixCalYearCode = true;
                }
                else {
                    IMN_SuffixCalYearCode = false;
                }


                swal({
                    title: "Are you sure?",
                    text: "Do you want to save Trip Numbering Details ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {


                            var data11 = {
                                "IMN_Id": $scope.Trip.IMN_Id,
                                "IMN_Flag": "TripNo",  //Trip Numbering
                                "IMN_AutoManualFlag": $scope.Trip.IMN_AutoManualFlag,
                                "IMN_DuplicatesFlag": $scope.Trip.IMN_DuplicatesFlag,
                                "IMN_StartingNo": $scope.Trip.IMN_StartingNo,
                                "IMN_WidthNumeric": $scope.Trip.IMN_WidthNumeric,
                                "IMN_ZeroPrefixFlag": $scope.Trip.IMN_ZeroPrefixFlag,

                                "IMN_PrefixAcadYearCode": IMN_PrefixAcadYearCode,
                                "IMN_PrefixParticular": $scope.Trip.IMN_PrefixParticular,

                                "IMN_SuffixAcadYearCode": IMN_SuffixAcadYearCode,
                                "IMN_SuffixParticular": $scope.Trip.IMN_SuffixParticular,
                                "IMN_RestartNumFlag": "Never",


                                "IMN_PrefixFinYearCode": IMN_PrefixFinYearCode,
                                "IMN_PrefixCalYearCode": IMN_PrefixCalYearCode,

                                "IMN_SuffixFinYearCode": IMN_SuffixFinYearCode,
                                "IMN_SuffixCalYearCode": IMN_SuffixCalYearCode,

                            }
                            apiService.create("TransactionNumbering/", data11).then(function (promise) {
                                $scope.details();
                                swal('Record Saved Successfully');
                            });
                        }
                        else {
                            swal("Cancelled");
                        }
                    });

            }
            else {
                $scope.submittedTrip = true;
            }

        };
        $scope.ClearTripNumbering = function () {

            $scope.Trip = {};
            $scope.submittedTrip = false;

            $scope.Tripmanual = true;
            $scope.TripautoParti = true;
            $scope.TripautoPartisuf = true;
            $scope.Tripauto = true;
            $scope.myForm11.$setPristine();
            $scope.myForm11.$setUntouched();
        }

        $scope.OnchangeRestartNumTrip = function (data) {
            if (data == "Never") {
                $scope.Trip.IRN_RestartAcadYear = "";
                $scope.Trip.IRN_RestartFinYear = "";
                $scope.Trip.IRN_RestartcalendYear = "";
            }
        }

        //TripBill numbering.

        $scope.TripBillmanual = true;
        $scope.TripBillautoParti = true;
        $scope.TripBillautoPartisuf = true;
        $scope.TripBillslnostarting = true;
        $scope.TripBillauto = true;

        $scope.rollnumberauto = true;
        $scope.rollnumbermanual = true;
        //rollno
        $scope.manualrollnumber = function (obj) {
            if (obj == "Manual") {
                $scope.rollnumbermanual = false;
                $scope.rollnumberauto = true;

                $scope.Rollno.IMN_RestartNumFlag = null;
                $scope.Rollno.IMN_PrefixAcadYearCode = null;

                $scope.Rollno.IMN_PrefixParticular = '';
                $scope.Rollno.IMN_SuffixAcadYearCode = null;

                $scope.Rollno.IMN_SuffixParticular = '';
                $scope.Rollno.IMN_WidthNumeric = '';
                $scope.Rollno.IMN_StartingNo = '';
                $scope.Rollno.IMN_ZeroPrefixFlag = '';
            }
            else {
                $scope.rollnomanual = true;
                $scope.rollnumberauto = false;

                $scope.Rollno.IMN_DuplicatesFlag = '';

            }
            $scope.submittedrollno = false;
            $scope.myForm19.$setPristine();
            $scope.myForm19.$setUntouched();
        }

        //

        $scope.manualTripBill = function (obj) {
            if (obj == "Manual") {
                $scope.TripBillmanual = false;
                $scope.TripBillauto = true;
                $scope.TripBillautoParti = true;
                $scope.TripBillautoPartisuf = true;
                $scope.TripBill.IMN_RestartNumFlag = null;
                $scope.TripBill.IMN_PrefixAcadYearCode = null;

                $scope.TripBill.IMN_PrefixParticular = '';
                $scope.TripBill.IMN_SuffixAcadYearCode = null;

                $scope.TripBill.IMN_SuffixParticular = '';
                $scope.TripBill.IMN_WidthNumeric = '';
                $scope.TripBill.IMN_StartingNo = '';
                $scope.TripBill.IMN_ZeroPrefixFlag = '';
            }
            else {
                $scope.TripBillmanual = true;
                $scope.TripBillauto = false;
                $scope.TripBillautoParti = true;
                $scope.TripBillautoPartisuf = true;
                $scope.TripBill.IMN_DuplicatesFlag = '';

            }
            $scope.submittedTripBill = false;
            $scope.myForm12.$setPristine();
            $scope.myForm12.$setUntouched();
        }
        $scope.particularpreTripBill = function (prepart) {
            if (prepart == 1 || prepart == 2 || prepart == 3) {
                $scope.TripBillautoParti = true;
                $scope.TripBill.IMN_PrefixParticular = '';
            }
            else {
                $scope.TripBillautoParti = false;

                if ($scope.TripBill.IMN_PrefixAcadYearCode == 0 && $scope.TripBill.IMN_SuffixAcadYearCode == 0) {
                    $scope.TripBill.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.TripBill.IMN_RestartNumFlag = null;

                    $scope.submittedTripBill = false;
                    $scope.myForm12.$setPristine();
                    $scope.myForm12.$setUntouched();
                }

            }

        }
        $scope.particularsufTripBill = function (sufpart) {
            if (sufpart == 1 || sufpart == 2 || sufpart == 3) {
                $scope.TripBillautoPartisuf = true;
                $scope.TripBill.IMN_SuffixParticular = '';
            }
            else {
                $scope.TripBillautoPartisuf = false;

                if ($scope.TripBill.IMN_PrefixAcadYearCode == 0 && $scope.TripBill.IMN_SuffixAcadYearCode == 0) {
                    $scope.TripBill.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.TripBill.IMN_RestartNumFlag = null;

                    $scope.submittedTripBill = false;
                    $scope.myForm12.$setPristine();
                    $scope.myForm12.$setUntouched();
                }
            }


        }

        $scope.submittedTripBill = false;
        $scope.SaveTripBillNumbering = function (book) {

            if ($scope.myForm12.$valid) {
                var IMN_PrefixAcadYearCode;
                if ($scope.TripBill.IMN_PrefixAcadYearCode == "1") {
                    IMN_PrefixAcadYearCode = true;
                }
                else {
                    IMN_PrefixAcadYearCode = false;
                }

                var IMN_PrefixFinYearCode;
                if ($scope.TripBill.IMN_PrefixAcadYearCode == "2") {
                    IMN_PrefixFinYearCode = true;
                }
                else {
                    IMN_PrefixFinYearCode = false;
                }

                var IMN_PrefixCalYearCode;
                if ($scope.TripBill.IMN_PrefixAcadYearCode == "3") {
                    IMN_PrefixCalYearCode = true;
                }
                else {
                    IMN_PrefixCalYearCode = false;
                }

                var IMN_SuffixAcadYearCode;
                if ($scope.TripBill.IMN_SuffixAcadYearCode == "1") {
                    IMN_SuffixAcadYearCode = true;
                }
                else {
                    IMN_SuffixAcadYearCode = false;
                }

                var IMN_SuffixFinYearCode;
                if ($scope.TripBill.IMN_SuffixAcadYearCode == "2") {
                    IMN_SuffixFinYearCode = true;
                }
                else {
                    IMN_SuffixFinYearCode = false;
                }

                var IMN_SuffixCalYearCode;
                if ($scope.TripBill.IMN_SuffixAcadYearCode == "3") {
                    IMN_SuffixCalYearCode = true;
                }
                else {
                    IMN_SuffixCalYearCode = false;
                }


                swal({
                    title: "Are you sure?",
                    text: "Do you want to save Trip Bill Numbering Details ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {


                            var data12 = {
                                "IMN_Id": $scope.TripBill.IMN_Id,
                                "IMN_Flag": "TripBill",  //TripBill Numbering
                                "IMN_AutoManualFlag": $scope.TripBill.IMN_AutoManualFlag,
                                "IMN_DuplicatesFlag": $scope.TripBill.IMN_DuplicatesFlag,
                                "IMN_StartingNo": $scope.TripBill.IMN_StartingNo,
                                "IMN_WidthNumeric": $scope.TripBill.IMN_WidthNumeric,
                                "IMN_ZeroPrefixFlag": $scope.TripBill.IMN_ZeroPrefixFlag,

                                "IMN_PrefixAcadYearCode": IMN_PrefixAcadYearCode,
                                "IMN_PrefixParticular": $scope.TripBill.IMN_PrefixParticular,

                                "IMN_SuffixAcadYearCode": IMN_SuffixAcadYearCode,
                                "IMN_SuffixParticular": $scope.TripBill.IMN_SuffixParticular,
                                "IMN_RestartNumFlag": "Never",


                                "IMN_PrefixFinYearCode": IMN_PrefixFinYearCode,
                                "IMN_PrefixCalYearCode": IMN_PrefixCalYearCode,

                                "IMN_SuffixFinYearCode": IMN_SuffixFinYearCode,
                                "IMN_SuffixCalYearCode": IMN_SuffixCalYearCode,

                            }
                            apiService.create("TransactionNumbering/", data12).then(function (promise) {
                                $scope.details();
                                swal('Record Saved Successfully');
                            });
                        }
                        else {
                            swal("Cancelled");
                        }
                    });

            }
            else {
                $scope.submittedTripBill = true;
            }

        };
        $scope.ClearTripBillNumbering = function () {

            $scope.TripBill = {};
            $scope.submittedTripBill = false;

            $scope.TripBillmanual = true;
            $scope.TripBillautoParti = true;
            $scope.TripBillautoPartisuf = true;
            $scope.TripBillauto = true;
            $scope.myForm12.$setPristine();
            $scope.myForm12.$setUntouched();
        }

        $scope.OnchangeRestartNumTripBill = function (data) {
            if (data == "Never") {
                $scope.TripBill.IRN_RestartAcadYear = "";
                $scope.TripBill.IRN_RestartFinYear = "";
                $scope.TripBill.IRN_RestartcalendYear = "";
            }
        }


        //roll numbering

        $scope.submittedrollno = false;
        $scope.SaveRollNoNumbering = function (data) {

            if ($scope.myForm19.$valid) {
                var IMN_PrefixAcadYearCode;
                if ($scope.TripBill.IMN_PrefixAcadYearCode == "1") {
                    IMN_PrefixAcadYearCode = true;
                }
                else {
                    IMN_PrefixAcadYearCode = false;
                }

                var IMN_PrefixFinYearCode;
                if ($scope.TripBill.IMN_PrefixAcadYearCode == "2") {
                    IMN_PrefixFinYearCode = true;
                }
                else {
                    IMN_PrefixFinYearCode = false;
                }

                var IMN_PrefixCalYearCode;
                if ($scope.TripBill.IMN_PrefixAcadYearCode == "3") {
                    IMN_PrefixCalYearCode = true;
                }
                else {
                    IMN_PrefixCalYearCode = false;
                }

                var IMN_SuffixAcadYearCode;
                if ($scope.TripBill.IMN_SuffixAcadYearCode == "1") {
                    IMN_SuffixAcadYearCode = true;
                }
                else {
                    IMN_SuffixAcadYearCode = false;
                }

                var IMN_SuffixFinYearCode;
                if ($scope.TripBill.IMN_SuffixAcadYearCode == "2") {
                    IMN_SuffixFinYearCode = true;
                }
                else {
                    IMN_SuffixFinYearCode = false;
                }

                var IMN_SuffixCalYearCode;
                if ($scope.TripBill.IMN_SuffixAcadYearCode == "3") {
                    IMN_SuffixCalYearCode = true;
                }
                else {
                    IMN_SuffixCalYearCode = false;
                }

                var rollnonumberingconfig = $scope.rollnoauto;
                swal({
                    title: "Are you sure?",
                    text: "Do you want to save Trip Bill Numbering Details ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {


                            var data12 = {
                                "IMN_Id": $scope.Rollno.IMN_Id,
                                "IMN_Flag": "RollNumber",  //TripBill Numbering
                                "IMN_AutoManualFlag": $scope.Rollno.IMN_AutoManualFlag,
                                "IMN_DuplicatesFlag": $scope.Rollno.IMN_DuplicatesFlag,
                                "IMN_StartingNo": $scope.Rollno.IMN_StartingNo,
                                "IMN_WidthNumeric": $scope.Rollno.IMN_WidthNumeric,
                                "IMN_ZeroPrefixFlag": $scope.Rollno.IMN_ZeroPrefixFlag,

                                "IMN_PrefixAcadYearCode": IMN_PrefixAcadYearCode,
                                "IMN_PrefixParticular": $scope.Rollno.IMN_PrefixParticular,

                                "IMN_SuffixAcadYearCode": IMN_SuffixAcadYearCode,
                                "IMN_SuffixParticular": $scope.Rollno.IMN_SuffixParticular,
                                "IMN_RestartNumFlag": "Never",


                                "IMN_PrefixFinYearCode": IMN_PrefixFinYearCode,
                                "IMN_PrefixCalYearCode": IMN_PrefixCalYearCode,

                                "IMN_SuffixFinYearCode": IMN_SuffixFinYearCode,
                                "IMN_SuffixCalYearCode": IMN_SuffixCalYearCode,
                                "RollNumberingconfig": rollnonumberingconfig

                            }
                            apiService.create("TransactionNumbering/", data12).then(function (promise) {
                                $scope.details();
                                swal('Record Saved Successfully');
                            });
                        }
                        else {
                            swal("Cancelled");
                        }
                    });

            }
            else {
                $scope.submittedrollno = true;
            }

        };
        $scope.ClearRollNoNumbering = function () {

            $scope.TripBill = {};
            $scope.submittedTripBill = false;

            $scope.TripBillmanual = true;
            $scope.TripBillautoParti = true;
            $scope.TripBillautoPartisuf = true;
            $scope.TripBillauto = true;
            $scope.myForm12.$setPristine();
            $scope.myForm12.$setUntouched();
        }
        //


        //Leave Numbering.

        $scope.Leavemanual = true;
        $scope.LeaveautoParti = true;
        $scope.LeaveautoPartisuf = true;
        $scope.Leaveslnostarting = true;
        $scope.Leaveauto = true;

        $scope.manualLeave = function (obj) {
            if (obj == "Manual") {
                $scope.Leavemanual = false;
                $scope.Leaveauto = true;
                $scope.LeaveautoParti = true;
                $scope.LeaveautoPartisuf = true;
                $scope.Leave.IMN_RestartNumFlag = null;
                $scope.Leave.IMN_PrefixAcadYearCode = null;
                $scope.Leave.IMN_PrefixParticular = '';
                $scope.Leave.IMN_SuffixAcadYearCode = null;
                $scope.Leave.IMN_SuffixParticular = '';
                $scope.Leave.IMN_WidthNumeric = '';
                $scope.Leave.IMN_StartingNo = '';
                $scope.Leave.IMN_ZeroPrefixFlag = '';

            }
            else {
                $scope.Leavemanual = true;
                $scope.Leaveauto = false;
                $scope.LeaveautoParti = true;
                $scope.LeaveautoPartisuf = true;
                $scope.Leave.IMN_DuplicatesFlag = '';

            }
            $scope.submittedLeave = false;
            $scope.myFormLeave.$setPristine();
            $scope.myFormLeave.$setUntouched();
        }
        $scope.particularpreLeave = function (prepart) {
            if (prepart == 1) {
                $scope.LeaveautoParti = true;
            }
            else {
                $scope.LeaveautoParti = false;

                if ($scope.Leave.IMN_PrefixAcadYearCode == 0 && $scope.Leave.IMN_SuffixAcadYearCode == 0) {
                    $scope.Leave.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.Leave.IMN_RestartNumFlag = null;
                    $scope.submittedLeave = false;
                    $scope.myFormLeave.$setPristine();
                    $scope.myFormLeave.$setUntouched();
                }
            }

        }
        $scope.particularsufLeave = function (sufpart) {
            if (sufpart == 1) {
                $scope.LeaveautoPartisuf = true;
            }
            else {
                $scope.LeaveautoPartisuf = false;

                if ($scope.Leave.IMN_PrefixAcadYearCode == 0 && $scope.Leave.IMN_SuffixAcadYearCode == 0) {
                    $scope.Leave.IMN_RestartNumFlag = "Never";
                }
                else {
                    $scope.Leave.IMN_RestartNumFlag = null;

                    $scope.submittedLeave = false;
                    $scope.myFormLeave.$setPristine();
                    $scope.myFormLeave.$setUntouched();
                }
            }

        }

        $scope.submittedLeave = false;
        $scope.SaveLeaveNumbering = function (Leave) {

            if ($scope.myFormLeave.$valid) {
                var IMN_PrefixAcadYearCode;
                if ($scope.Leave.IMN_PrefixAcadYearCode == "0") {
                    IMN_PrefixAcadYearCode = false;
                }
                else {
                    IMN_PrefixAcadYearCode = true;
                }
                var IMN_SuffixAcadYearCode;
                if ($scope.Leave.IMN_SuffixAcadYearCode == "0") {
                    IMN_SuffixAcadYearCode = false;
                }
                else {
                    IMN_SuffixAcadYearCode = true;
                }
                swal({
                    title: "Are you sure?",
                    text: "Do you want to save Leave Numbering Details ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {


                            var dataLeave = {
                                "IMN_Id": $scope.Leave.IMN_Id,
                                "IMN_Flag": "LeaveNo",  //Admission Numbering
                                "IMN_AutoManualFlag": $scope.Leave.IMN_AutoManualFlag,
                                "IMN_DuplicatesFlag": $scope.Leave.IMN_DuplicatesFlag,
                                "IMN_StartingNo": $scope.Leave.IMN_StartingNo,
                                "IMN_WidthNumeric": $scope.Leave.IMN_WidthNumeric,
                                "IMN_ZeroPrefixFlag": $scope.Leave.IMN_ZeroPrefixFlag,
                                "IMN_PrefixAcadYearCode": IMN_PrefixAcadYearCode,
                                "IMN_PrefixParticular": $scope.Leave.IMN_PrefixParticular,
                                "IMN_SuffixAcadYearCode": IMN_SuffixAcadYearCode,
                                "IMN_SuffixParticular": $scope.Leave.IMN_SuffixParticular,
                                "IMN_RestartNumFlag": $scope.Leave.IMN_RestartNumFlag,
                                //"IMN_RestartAcadYear": $scope.Admission.IMN_RestartAcadYear
                            }
                            apiService.create("TransactionNumbering/", dataLeave).then(function (promise) {
                                $scope.details();
                                swal('Record Saved Successfully');
                            });
                        }
                        else {
                            swal("Cancelled");
                        }
                    });

            }
            else {
                $scope.submitteddataLeave = true;
            }

        };
        $scope.ClearLeaveNumbering = function () {

            $scope.Leave = {};
            $scope.submittedLeave = false;

            $scope.Leavemanual = true;
            $scope.LeaveautoParti = true;
            $scope.LeaveautoPartisuf = true;
            $scope.Leaveauto = true;
            $scope.myFormLeave.$setPristine();
            $scope.myFormLeave.$setUntouched();
        }
    }
})();

