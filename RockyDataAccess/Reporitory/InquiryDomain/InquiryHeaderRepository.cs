﻿using RockyDataAccess.Data;
using RockyModels.InquiryDomain;

namespace RockyDataAccess.Reporitory.InquiryDomain
{
    public class InquiryHeaderRepository : Repository<InquiryHeader>, IInquiryHeaderRepository
    {
        private readonly AppDbContext _appDbContext;

        public InquiryHeaderRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void RemoveWithDeatails(InquiryHeader header, IInquiryDetailRepository detailRepository)
        {
            var details = detailRepository.GetAll(x => x.InquiryHeaderId == header.Id);
            detailRepository.RemoveRange(details);
            base.Remove(header);
        }

        public void Update(InquiryHeader header)
        {
            _appDbContext.InquiryHeader.Update(header);
        }
    }
}
