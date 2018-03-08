// JScript 檔

//for Login and Set Card
        function readcard_onclick(){
	        CertIssuer.CardInfor();
        }

        function submit2_onclick() {
        CertIssuer.CardInfor();
        var aobject = document.getElementById('hrecive1');
        aobject.value=CertIssuer.CertRDNFromDN;
        aobject.value+=';';
        aobject.value+=CertIssuer.CertPersonIdno4;
        aobject.value+=';';
        aobject.value+=CertIssuer.CertSerialNumber;
        }
        
         function readcard_onclick2() {
            if(document.all.ctl00_ContentPlaceHolder1_txtPassword.value == ''){
            alert('請輸入您的自然人卡片密碼！');
            return false;
            } else {
                if(CertIssuer.PinCodeCheck(document.all.ctl00_ContentPlaceHolder1_txtPassword.value) == 1){
                CertIssuer.CardInfor();
                var aobject = document.getElementById('hrecive1');
                aobject.value=CertIssuer.CertRDNFromDN;
                aobject.value+=';';
                aobject.value+=CertIssuer.CertPersonIdno4;
                aobject.value+=';';
                aobject.value+=CertIssuer.CertSerialNumber;
                document.getElementById('text1').value=CertIssuer.CertSerialNumber;
                }
            }
           }
               
          
        function checkcardvial2(){
         CertIssuer.CardInfor();
         document.getElementById('text1').value=CertIssuer.CertIssuerDN;
        }
        
         function abortPB() {
            var obj = Sys.WebForms.PageRequestManager.getInstance();
            if (obj.get_isInAsyncPostBack()) {
                obj.abortPostBack();
			    }
			}
			         
            
// //for POMNoProcess
              function changefunction2(divid)
            {
            object0=document.getElementById('div_function_0');
            object1=document.getElementById('div_function_1');
            object2=document.getElementById('div_function_2');
            object3=document.getElementById('div_function_3');
            if(divid==0){
            object0.style.display='block';
            object1.style.display='none';
            object2.style.display='none';
            object3.style.display='none';
            }
            else if(divid==1){
            object0.style.display='none';
            object1.style.display='block';
            object2.style.display='none';
            object3.style.display='none';
            }
            else if(divid==2){
            object0.style.display='none';
            object1.style.display='none';
            object2.style.display='block';
            object3.style.display='none';
            }
            else if(divid==3){
            object0.style.display='none';
            object1.style.display='none';
            object2.style.display='none';
            object3.style.display='block';
            }
            }
            
			
//for POMNoProcess
			function doDisable(count)
			{
			var j=0;
			for(i=0;i<count;){
			var theCheckbox=document.getElementById('DynamicUserControl_cblMyDepartment_' + i);
			if(theCheckbox.checked==true)
							{
							j+=1;
							}
							i+=1;
			}
			if(j>1){
				document.all.DynamicUserControl_ActingCheckBox.checked=false;
			    document.all.DynamicUserControl_ActingCheckBox.disabled=true;
			    document.all.DynamicUserControl_btnSenToOther.value='委請子單位回覆資料'
			}
			else{
			    document.all.DynamicUserControl_ActingCheckBox.disabled=false;
			    document.all.DynamicUserControl_btnSenToOther.value='分派至子單位'
			}
			}
			
			
//for Defautl Admin Announcement Message Display
        function show()
			{
			document.all.float_bg.style.display='BLOCK';			
			document.all.float_function.style.display='BLOCK';
			}
		function hide()
			{
			document.all.float_bg.style.display='NONE';
			document.all.float_function.style.display='NONE';
			}
		function float_list()
			{
			document.all.float_list.style.display='BLOCK';
			document.all.float_add.style.display='NONE';
			//document.all.label_add.className.replace('active','idle');
			//document.all.label_list.className.replace('idle','active');
			document.all.label_list.className='active';
			document.all.label_add.className='idle';
			}
		function float_add()
			{
			document.all.float_list.style.display='NONE';
			document.all.float_add.style.display='BLOCK';
			//document.all.label_list.className.replace('active','idle');
			//document.all.label_add.className.replace('idle','active');
			document.all.label_list.className='idle';
			document.all.label_add.className='active';
			}
	
		function set_float_bg_height() {
			var floatbg = document.getElementById('DynamicUserControl_float_bg');
			floatbg.style.height = getMaxHeight();
			
			var float_function = document.getElementById('DynamicUserControl_float_function');
			float_function.style.top = getScrollY() + 60;
			//alert(getScrollY());
			}