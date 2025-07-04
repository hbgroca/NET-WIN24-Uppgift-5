﻿// Creds to Hasse

const validateField = (field) => {
    //console.log("Validating fields")
    // Get the error span for the field
    let errorSpan = document.querySelector(`span[data-valmsg-for='${field.name}']`)
    if (!errorSpan) {
        //console.error(`Could not find a span with the data-valmsg-for attribute equal to ${field.name}`)
        return;
    }

    let errorMessage = ""
    let value = field.value.trim()


    // Check if the value is required
    if (field.hasAttribute("data-val-required") && value === "")
        errorMessage = field.getAttribute("data-val-required")


    // Check if the value is a valid email
    if (field.hasAttribute("data-val-regex") && value !== "") {
        let pattern = new RegExp(field.getAttribute("data-val-regex-pattern"))
        if (!pattern.test(value))
            errorMessage = field.getAttribute("data-val-regex")
    }

    // Show or remove error msg
    if (errorMessage) {
        field.classList.add("input-validation-error")
        errorSpan.classList.remove("field-validation-valid")
        errorSpan.classList.add("field-validation-error")
        errorSpan.textContent = errorMessage
    } else {
        field.classList.remove("input-validation-error")
        errorSpan.classList.remove("field-validation-error")
        errorSpan.classList.add("field-validation-valid")
        errorSpan.textContent = ""
    }
}

document.addEventListener('DOMContentLoaded', function () {
    //console.log("Loading Validation script")
    const form = document.querySelector('.forms-to-validate')

    if (!form) {
        //console.error("Could not find a form element")
        return;
    }

    // Get all the fields that need to be validated
    const fields = form.querySelectorAll("input[data-val='true']")
    //console.log("Fields found to validate: ", fields)

    // Add an event listener to each field
    fields.forEach(field => {
        field.addEventListener("input", function () {
            //console.log("Validating: ", field)
            validateField(field)
        })
    })
})

