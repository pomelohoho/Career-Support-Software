const jobsApiService = {
    async getLatestJobs(includeNonSponsors = true) {
        const params = new URLSearchParams({
            limit: 100,
            includeNonSponsors: includeNonSponsors.toString()
        });
        const response = await fetch(`/api/jobs?${params}`);
        if (!response.ok) {
            throw new Error(`API error: ${response.status}`);
        }
        return await response.json();
    },
};

export default jobsApiService;
