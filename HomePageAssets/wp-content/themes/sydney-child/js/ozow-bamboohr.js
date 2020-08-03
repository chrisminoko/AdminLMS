var jobUrl = "";

jQuery(document).ready(function ($) {
    ResetJobRequirementsModal();
    $(".BambooHR-ATS-Jobs-Item").append('<span class="btn btn-success btn-lg"> Apply <i class="fa fa-chevron-right" aria-hidden="true"></i></span>');
    $(".BambooHR-ATS-Location").prepend('<i class="fa fa-map-marker" aria-hidden="true"></i>&nbsp;');
    $(".BambooHR-ATS-Jobs-Item a").prepend('<i class="fa fa-user" aria-hidden="true"></i>&nbsp; ');
  

    jQuery(".BambooHR-ATS-Department-List .BambooHR-ATS-Department-Item .BambooHR-ATS-Jobs-List .BambooHR-ATS-Jobs-Item").on("click", function (e) {
        e.preventDefault()
        jobUrl = ""
        jobUrl = jQuery(this).find("a:eq(0)").attr("href");
        let jobTitle = jQuery.trim(jQuery(this).find("a").text());
        let jobIdentifier = jobTitle.replace(/\s/g, '').replace("/","").replace(/[^\w\s]/gi, '');
        let jobRequirements = [];
        jQuery.ajax({
            method: "GET",
            url: `${hostUrl}/wp-json/jobRequirements/v1/job/${jobIdentifier}/`,
            success: function (data) {
                if (data[0] !== "" && typeof (data[0]) !== 'undefined') {
                    jobRequirements = data[0].JobRequirements.split(";")
                    ul = document.createElement('ul');
                    document.getElementById('requirements').appendChild(ul);
                    jobRequirements.forEach(function (requirement) {
                        let li = document.createElement('li');
                        ul.appendChild(li);
                        li.innerHTML += `<input type='checkbox' onclick='CheckRequirementsStatus()'/><span>${requirement}</span>`;
                    });
                    jQuery("#jobRequirementsModal .modal-header span").append(jobTitle);
                    jQuery("#jobRequirementsModal").modal("show");
                } else {
                    RedirectToSelectedJobSpecification();
                }
            },
            error: function (xhr, status, error) {
                console.log(error);
            }
        });
    });   

});


function CheckRequirementsStatus(){   
        let checkBoxes = jQuery('#jobRequirementsModal input[type=checkbox]');
        if (checkBoxes.filter(':checked').length === checkBoxes.length) {
            jQuery('#jobRequirementsModal #continue').removeAttr("disabled");
            jQuery('#jobRequirementsModal #continue').removeClass('disable');
        }
        else {
            jQuery('#jobRequirementsModal #continue').prop('disabled', 'disabled');
            jQuery('#jobRequirementsModal #continue').addClass('disable');
        }  
}

function RedirectToSelectedJobSpecification() {
    if (jobUrl !== "") {
        window.open(jobUrl, '_blank');
        jobUrl = "";
    }
    jQuery("#jobRequirementsModal").modal("hide");
    ResetJobRequirementsModal();
}


function CloseJobRequirementsModal() {
    jobUrl = "";
    jQuery("#jobRequirementsModal").modal("hide");
    ResetJobRequirementsModal();
}

function ResetJobRequirementsModal() {
    jQuery('#jobRequirementsModal #continue').prop('disabled', 'disabled');
    jQuery('#jobRequirementsModal #continue').addClass('disable');
    jQuery('#jobRequirementsModal input[type=checkbox]').prop('checked', false);
    jQuery('#jobRequirementsModal #continue').prop('disabled', 'disabled');
    jQuery('#jobRequirementsModal .modal-body').html('');
    jQuery("#jobRequirementsModal .modal-header span").html('');
}
