#include "AsyncHandlerRanking.h"
#include "GlobalDefine.h"
#include "Cache.h"

namespace yyb
{
	void AsyncHandlerRanking::OnRead(
		const RankingRequest& request,
		RankingReply& reply)
	{
		std::cout << __FUNCTION__ << " : " << status_ << std::endl;

		auto redis = Cache::Instance().GetCache(CACHE_INDEX_GLOBAL);
		if (redis)
		{
			std::string key = "ranking";
			std::string value;

			auto user = GetUser();
			if (user)
			{
				value = user->GetNickName();
			}

			//// �� ����� ��ŷ ����
			//auto reply = redis->zrank(key, value);
			//redis->sync_commit();

			//if (reply.get().is_integer())
			//{
			//	storedScore = reply.get().as_integer();
			//}

			//// �� ����� ��ŷ ������ ���� ȹ���� ���� ��
			//if (storedScore < score)
			//{
			//	std::vector< std::string > members;

			//	members.push_back(value);

			//	// ���� ȹ���� ������ �� ũ�� ����� ��ŷ ���� ����
			//	redis->zrem(key, members);
			//}

			if (false == value.empty())
			{
				// ���� ȹ���� ���� ����
				std::multimap<std::string, std::string> score_members;
				std::vector<std::string> options;

				score_members.insert({ std::to_string(request.score()), value });

				redis->zadd(key, options, score_members);

				redis->commit();
			}
		}
	}

	void AsyncHandlerRanking::OnWrite()
	{

	}
}