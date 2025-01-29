# REST
All the available REST API on `https://cscore.net/api` are listed in this document.

```
/system/organizations
  /get-all
  /sync-badges
  /sync-documents

 /{program}/badges
  /current/get-bundle
  /current/get-validations
  /current/get-badges
  /github/get-bundle?url={githuburl}
  /github/get-validations={githuburl}
  /github/get-badges={githuburl}

/{org}/docs
  /get-documents
  /get-document-html/{doc-code}
```

# Pages

```
/home
/settings
  /general
  /programs

/p/{program-code}
  /home
  /badges
    /
    /{badge-code}
  /learners
  /docs
    /
    /{doc-code}
  /feed

/u/{learner-code}
  /home
  /badges
  /feed
```  
